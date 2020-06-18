using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Serilog;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.SQL;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ImportBundleBlock)]
    public class ImportBundleBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ImportBundleBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg?.SourceEntity != null)
            {
                await ImportBundle(arg.ImportHandler.GetCommerceEntity(), arg, context)
                    .ConfigureAwait(false);
            }

            return arg;
        }

        private async Task ImportBundle(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context)
        {
            if (commerceEntity.HasComponent<BundleComponent>())
            {
                var bundleComponent = commerceEntity.GetComponent<BundleComponent>();

                //get existing relationship data from IGetListsEntityIdsPipeline (super intuitive name)
                var relationshipListNames = new[] {
                    CatalogConstants.BundleToSellableItem,
                    CatalogConstants.BundleToSellableItemVariant,
                    CatalogConstants.SellableItemToBundle //TOOD - this is broken
                };

                relationshipListNames = relationshipListNames.Select(x =>
                {
                    x = $"{x}-{commerceEntity.FriendlyId}";
                    if (commerceEntity.EntityVersion > 1)
                    {
                        x = $"{x}-{commerceEntity.EntityVersion}";
                    }
                    return x;
                }).ToArray();

                var existingRelationshipDictionary = await _commerceCommander.Pipeline<IGetListsEntityIdsPipeline>().Run(new GetListsEntityIdsArgument(relationshipListNames), context).ConfigureAwait(false);

                //define a separate dictionary to store the "valid" relationships (this is used to remove orphaned relations later)
                var newRelationshipDictionary = new Dictionary<string, List<string>>()
                {
                    { CatalogConstants.BundleToSellableItem, new List<string>() },
                    { CatalogConstants.BundleToSellableItemVariant, new List<string>() }
                };

                //add new relationships
                foreach (var bundleItem in bundleComponent.BundleItems)
                {
                    var isVariantRelationship = false;
                    var sellableItemId = bundleItem.SellableItemId;
                    if (bundleItem.SellableItemId.Contains('|'))
                    {
                        isVariantRelationship = true;
                        sellableItemId = sellableItemId.Split('|')[0];
                    }

                    if (!isVariantRelationship)
                    {
                        newRelationshipDictionary[CatalogConstants.BundleToSellableItem].Add(sellableItemId);
                    }
                    else
                    {
                        newRelationshipDictionary[CatalogConstants.BundleToSellableItemVariant].Add(sellableItemId);
                    }

                    if (!isVariantRelationship && !HasRelationship(sellableItemId, CatalogConstants.BundleToSellableItem, existingRelationshipDictionary))
                    {
                        await CreateRelationship(commerceEntity.Id, sellableItemId, CatalogConstants.BundleToSellableItem, context);
                    }
                    else if (isVariantRelationship && !HasRelationship(sellableItemId, CatalogConstants.BundleToSellableItemVariant, existingRelationshipDictionary))
                    {
                        await CreateRelationship(commerceEntity.Id, sellableItemId, CatalogConstants.BundleToSellableItemVariant, context);
                    }

                    //TOOD - this is broken -- the SellableItemToBundle link does not come back on the query above
                    //if (!HasRelationship(sellableItemId, CatalogConstants.SellableItemToBundle, relationshipDictionary))
                    //{
                    //    //create new sellable item to bundle relation
                    //    await CreateRelationship(sellableItemId, commerceEntity.Id, CatalogConstants.SellableItemToBundle, context);
                    //}
                }

                //remove orphaned relationships
                var orphanedSellableItemIds = existingRelationshipDictionary.Single(dict => ParseRelationshipTypeFromKey(dict.Key) == CatalogConstants.BundleToSellableItem).Value.Where(x => !newRelationshipDictionary[CatalogConstants.BundleToSellableItem].Contains(x));
                foreach (var sellableItemId in orphanedSellableItemIds)
                {
                    await DeleteRelationship(commerceEntity.Id, sellableItemId, CatalogConstants.BundleToSellableItem, context);
                }

                var orphanedSellableItemVariantIds = existingRelationshipDictionary.Single(dict => ParseRelationshipTypeFromKey(dict.Key) == CatalogConstants.BundleToSellableItemVariant).Value.Where(x => !newRelationshipDictionary[CatalogConstants.BundleToSellableItemVariant].Contains(x));
                foreach (var sellableItemId in orphanedSellableItemVariantIds)
                {
                    await DeleteRelationship(commerceEntity.Id, sellableItemId, CatalogConstants.BundleToSellableItemVariant, context);
                }
            }
        }

        private async Task CreateRelationship(string source, string target, string relationshipType, CommercePipelineExecutionContext context)
        {
            var relationshipArg = new RelationshipArgument(source, target, relationshipType);
            await _commerceCommander.Pipeline<ICreateRelationshipPipeline>().Run(relationshipArg, context).ConfigureAwait(false);
        }

        private async Task DeleteRelationship(string source, string target, string relationshipType, CommercePipelineExecutionContext context)
        {
            var relationshipArg = new RelationshipArgument(source, target, relationshipType);
            await _commerceCommander.Pipeline<IDeleteRelationshipPipeline>().Run(relationshipArg, context).ConfigureAwait(false);
        }

        private bool HasRelationship(string target, string relationshipType, IDictionary<string, IList<string>> relationshipDictionary)
        {
            return relationshipDictionary.Any(dict => ParseRelationshipTypeFromKey(dict.Key) == relationshipType && dict.Value.Any(x => string.Equals(x, target, StringComparison.InvariantCultureIgnoreCase)));
        }

        private string ParseRelationshipTypeFromKey(string key)
        {
            //the key that comes back from IGetListsEntityIdsPipeline is formatted as follows: "List-BUNDLETOSELLABLEITEM-TESTBUNDLE01-ByDate"
            //however, based on CreateBundleRelationshipsBlock, we need to use the values from CatalogConstants

            var parts = key.Split('-');
            var relationshipType = parts[1];

            if (string.Equals(relationshipType, CatalogConstants.BundleToSellableItem, StringComparison.InvariantCultureIgnoreCase))
            {
                relationshipType = CatalogConstants.BundleToSellableItem;
            }
            else if (string.Equals(relationshipType, CatalogConstants.BundleToSellableItemVariant, StringComparison.InvariantCultureIgnoreCase))
            {
                relationshipType = CatalogConstants.BundleToSellableItemVariant;
            }
            else if (string.Equals(relationshipType, CatalogConstants.SellableItemToBundle, StringComparison.InvariantCultureIgnoreCase))
            {
                relationshipType = CatalogConstants.SellableItemToBundle;
            }

            return relationshipType;
        }

    }
}