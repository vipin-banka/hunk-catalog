using Plugin.Hunk.Catalog.Entity;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ImportInventoryInformationBlock)]
    public class ImportInventoryInformationBlock : BaseImportInventoryInformationBlock
    {
        public ImportInventoryInformationBlock(CommerceCommander commerceCommander)
        : base(commerceCommander)
        {
        }

        protected override async Task ImportVariantsInventory(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context)
        {
            var sellableItemInventory = importEntityArgument.ImportHandler.GetSourceEntity() as SellableItemInventory;
            if (sellableItemInventory == null)
                return;

            var inventorySetName = sellableItemInventory.InventorySetName;

            if (!string.IsNullOrEmpty(inventorySetName))
            {
                var inventorySetId = await GetInventorySetName(context, inventorySetName)
                    .ConfigureAwait(false);

                if (!string.IsNullOrEmpty(inventorySetId))
                {
                    var inventoryDetail = sellableItemInventory
                        .InventoryDetail;

                    await ImportInventoryInformation(context, commerceEntity, inventorySetId, sellableItemInventory.VariantId, inventoryDetail).ConfigureAwait(false);

                }
            }
        }
    }
}