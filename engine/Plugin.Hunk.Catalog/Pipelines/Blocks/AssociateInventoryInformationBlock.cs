using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Metadata;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Inventory;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateInventoryInformationBlock)]
    public class AssociateInventoryInformationBlock : PipelineBlock<CommerceEntity, CommerceEntity, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public AssociateInventoryInformationBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<CommerceEntity> Run(CommerceEntity arg, CommercePipelineExecutionContext context)
        {
            var importEntityArgument = context.CommerceContext.GetObject<ImportEntityArgument>();
            if (importEntityArgument?.SourceEntity != null)
            {
                await ImportVariantsInventory(arg, importEntityArgument, context)
                    .ConfigureAwait(false);
            }

            return arg;
        }

        private async Task ImportVariantsInventory(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context)
        {
            var sourceEntityHasVariants = importEntityArgument.ImportHandler.HasVariants();

            if (sourceEntityHasVariants)
            {
                var inventorySetName = importEntityArgument.ImportHandler
                    .GetPropertyValueFromSource<InventorySetNameAttribute, string>();

                if (!string.IsNullOrEmpty(inventorySetName))
                {
                    var inventorySetId = await context.ValidateEntityId(_commerceCommander.Pipeline<IDoesEntityExistPipeline>(), typeof(InventorySet), inventorySetName.ToEntityId<InventorySet>())
                        .ConfigureAwait(false);

                    var variants = importEntityArgument.ImportHandler.GetVariants();

                    foreach (var variant in variants)
                    {
                        var inventoryDetail = variant.GetType()
                            .GetPropertyValueWithAttribute<InventoryDetailAttribute, InventoryDetail>(variant);

                        if (inventoryDetail != null)
                        {
                            await _commerceCommander.Command<AssociateSellableItemToInventorySetCommand>()
                                .Process(context.CommerceContext, commerceEntity.Id, variant.Id, inventorySetId, ConvertToEntityView(inventoryDetail))
                                .ConfigureAwait(false);
                        }
                    }
                }
            }
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
                Value = inventoryDetail.InvoiceUnitPrice.ToString()
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