using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Inventory;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    public abstract class BaseImportInventoryInformationBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public BaseImportInventoryInformationBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg != null)
            {
                await ImportVariantsInventory(arg.ImportHandler.GetCommerceEntity(), arg, context)
                    .ConfigureAwait(false);
            }

            return arg;
        }

        protected abstract Task ImportVariantsInventory(CommerceEntity commerceEntity,
            ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context);


        protected async Task ImportInventoryInformation(CommercePipelineExecutionContext context, CommerceEntity commerceEntity, string inventorySetId, string variantId, InventoryDetail inventoryDetail)
        {
            var result = await GetInventoryInformation(context, commerceEntity, inventorySetId, variantId)
                .ConfigureAwait(false);

            if (inventoryDetail != null)
            {
                if (result.Item2)
                {
                    await _commerceCommander.Command<EditInventoryInformationCommand>()
                        .Process(context.CommerceContext, commerceEntity.Id, variantId, inventorySetId,
                            ConvertToEntityView(inventoryDetail));
                }
                else
                {
                    await _commerceCommander.Command<AssociateSellableItemToInventorySetCommand>()
                        .Process(context.CommerceContext, commerceEntity.Id, variantId, inventorySetId,
                            ConvertToEntityView(inventoryDetail))
                        .ConfigureAwait(false);
                }
            }
            else
            {
                if (result.Item2)
                {
                    await _commerceCommander.Command<DisassociateSellableItemFromInventorySetCommand>()
                        .Process(context.CommerceContext, commerceEntity.Id, variantId, inventorySetId);
                }
                else if (result.Item1 != null)
                {
                    await _commerceCommander.DeleteEntity(context.CommerceContext, result.Item1)
                        .ConfigureAwait(false);
                }
            }
        }

        protected async Task<string> GetInventorySetName(CommercePipelineExecutionContext context, string inventorySetName)
        {
            return await context.ValidateEntityId(_commerceCommander.Pipeline<IDoesEntityExistPipeline>(), typeof(InventorySet), inventorySetName.ToEntityId<InventorySet>())
                .ConfigureAwait(false);
        }

        protected async Task<Tuple<InventoryInformation, bool>> GetInventoryInformation(CommercePipelineExecutionContext context, CommerceEntity commerceEntity, string inventorySetId, string variantId)
        {
            bool associationExists = false;
            InventoryInformation inventoryInformation = await _commerceCommander
                .Command<GetInventoryInformationCommand>().Process(context.CommerceContext,
                    inventorySetId, commerceEntity.Id, variantId).ConfigureAwait(false);

            FindEntitiesInListArgument entitiesInListArgument1 =
                new FindEntitiesInListArgument(typeof(SellableItem),
                    "InventorySetToInventoryInformation-" + inventorySetId.SimplifyEntityName(), 0,
                    int.MaxValue);
            entitiesInListArgument1.LoadEntities = false;

            FindEntitiesInListArgument entitiesInListArgument2 = await _commerceCommander
                .Pipeline<FindEntitiesInListPipeline>().Run(entitiesInListArgument1, context)
                .ConfigureAwait(false);

            if (inventoryInformation != null && entitiesInListArgument2 != null)
            {
                IList<ListEntityReference> entityReferences =
                    entitiesInListArgument2.EntityReferences;
                bool? nullable = entityReferences != null
                    ? entityReferences.Any(x => x.EntityUniqueId.Equals(inventoryInformation.UniqueId))
                    : new bool?();
                associationExists = nullable.HasValue && nullable.Value;
            }

            return new Tuple<InventoryInformation, bool>(inventoryInformation, associationExists);
        }


        private EntityView ConvertToEntityView(InventoryDetail inventoryDetail)
        {
            var entityView = new EntityView();

            var property1 = new ViewProperty()
            {
                Name = "Quantity",
                DisplayName = "Quantity",
                Value = inventoryDetail.Quantity.ToString()
            };
            entityView.Properties.Add(property1);

            var property2 = new ViewProperty()
            {
                Name = "InvoiceUnitPrice",
                DisplayName = "InvoiceUnitPrice",
                Value = inventoryDetail.InvoiceUnitPrice.ToString(CultureInfo.InvariantCulture)
            };
            entityView.Properties.Add(property2);

            var property3 = new ViewProperty()
            {
                Name = "InvoiceUnitPriceCurrency",
                DisplayName = "InvoiceUnitPriceCurrency",
                Value = inventoryDetail.InvoiceUnitPriceCurrency
            };
            entityView.Properties.Add(property3);

            var property4 = new ViewProperty()
            {
                Name = "Preorderable",
                DisplayName = "Preorderable",
                Value = inventoryDetail.Preorderable.ToString()
            };
            entityView.Properties.Add(property4);

            var property5 = new ViewProperty()
            {
                Name = "PreorderAvailabilityDate",
                DisplayName = "PreorderAvailabilityDate",
                Value = inventoryDetail.PreorderAvailabilityDate.HasValue ? inventoryDetail.PreorderAvailabilityDate.ToString() : string.Empty
            };
            entityView.Properties.Add(property5);

            var property6 = new ViewProperty()
            {
                Name = "PreorderLimit",
                DisplayName = "PreorderLimit",
                Value = inventoryDetail.PreorderLimit.HasValue ? inventoryDetail.PreorderLimit.ToString() : string.Empty
            };
            entityView.Properties.Add(property6);

            var property7 = new ViewProperty()
            {
                Name = "Backorderable",
                DisplayName = "Backorderable",
                Value = inventoryDetail.Backorderable.ToString()
            };
            entityView.Properties.Add(property7);

            var property8 = new ViewProperty()
            {
                Name = "BackorderAvailabilityDate",
                DisplayName = "BackorderAvailabilityDate",
                Value = inventoryDetail.BackorderAvailabilityDate.HasValue ? inventoryDetail.BackorderAvailabilityDate.ToString() : string.Empty
            };
            entityView.Properties.Add(property8);

            var property9 = new ViewProperty()
            {
                Name = "BackorderLimit",
                DisplayName = "BackorderLimit",
                Value = inventoryDetail.BackorderLimit.HasValue ? inventoryDetail.BackorderLimit.ToString() : string.Empty
            };
            entityView.Properties.Add(property9);

            return entityView;
        }
    }
}