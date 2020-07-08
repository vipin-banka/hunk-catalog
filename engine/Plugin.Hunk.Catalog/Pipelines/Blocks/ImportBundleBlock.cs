using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
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

                //get existing relationship data from IFindEntityListReferencesPipeline
                var findEntityListReferencesArg = new FindEntityListReferencesArgument(commerceEntity);
                findEntityListReferencesArg = await _commerceCommander.Pipeline<IFindEntityListReferencesPipeline>().Run(findEntityListReferencesArg, context).ConfigureAwait(false);

                var existingRelationshipDictionary = findEntityListReferencesArg.ListReferences.ToDictionary(x => ParseRelationshipTypeFromKey(x.Key), x => x.Value);

                //define a separate dictionary to store the "valid" relationships (this is used to remove orphaned relations later)
                var newRelationshipDictionary = new Dictionary<string, List<string>>()
                {
                    { CatalogConstants.BundleToSellableItem, new List<string>() },
                    { CatalogConstants.BundleToSellableItemVariant, new List<string>() }
                };

                //impot relationships
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

                    //create BundleToSellableItem/BundleToSellableItemVariant relationships (if not already present)
                    if (!isVariantRelationship && !HasRelationship(sellableItemId, CatalogConstants.BundleToSellableItem, existingRelationshipDictionary))
                    {
                        await CreateRelationship(commerceEntity.Id, sellableItemId, CatalogConstants.BundleToSellableItem, context);
                    }
                    else if (isVariantRelationship && !HasRelationship(sellableItemId, CatalogConstants.BundleToSellableItemVariant, existingRelationshipDictionary))
                    {
                        await CreateRelationship(commerceEntity.Id, sellableItemId, CatalogConstants.BundleToSellableItemVariant, context);
                    }

                    //create SellableItemToBundle relationships (we should first check if this exists before creating it, but the ICreateRelationshipPipeline seems to prevent duplicates already...)
                    await CreateRelationship(sellableItemId, commerceEntity.Id, CatalogConstants.SellableItemToBundle, context);
                }

                //remove orphaned relationships
                if (existingRelationshipDictionary.ContainsKey(CatalogConstants.BundleToSellableItem))
                {
                    var orphanedSellableItemIds = existingRelationshipDictionary[CatalogConstants.BundleToSellableItem].Where(x => !newRelationshipDictionary[CatalogConstants.BundleToSellableItem].Contains(x));
                    foreach (var sellableItemId in orphanedSellableItemIds)
                    {
                        await DeleteRelationship(commerceEntity.Id, sellableItemId, CatalogConstants.BundleToSellableItem, context);
                        await DeleteRelationship(sellableItemId, commerceEntity.Id, CatalogConstants.SellableItemToBundle, context);
                    }
                }

                if (existingRelationshipDictionary.ContainsKey(CatalogConstants.BundleToSellableItemVariant))
                {
                    var orphanedSellableItemVariantIds = existingRelationshipDictionary[CatalogConstants.BundleToSellableItemVariant].Where(x => !newRelationshipDictionary[CatalogConstants.BundleToSellableItemVariant].Contains(x));
                    foreach (var sellableItemId in orphanedSellableItemVariantIds)
                    {
                        await DeleteRelationship(commerceEntity.Id, sellableItemId, CatalogConstants.BundleToSellableItemVariant, context);
                        await DeleteRelationship(sellableItemId, commerceEntity.Id, CatalogConstants.SellableItemToBundle, context);
                    }
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
            return relationshipDictionary.ContainsKey(relationshipType) && relationshipDictionary[relationshipType].Any(x => string.Equals(x, target, StringComparison.InvariantCultureIgnoreCase));
        }

        private string ParseRelationshipTypeFromKey(string key)
        {
            //the key that comes back from IFindEntityListReferencesPipeline is formatted as follows: "BundleToSellableItem-TestBundle01"
            //we want the values from CatalogConstants

            var parts = key.Split('-');
            var relationshipType = parts[0];

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