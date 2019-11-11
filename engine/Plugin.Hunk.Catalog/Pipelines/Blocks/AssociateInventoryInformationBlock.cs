using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Metadata;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateInventoryInformationBlock)]
    public class AssociateInventoryInformationBlock : BaseImportInventoryInformationBlock
    {
        public AssociateInventoryInformationBlock(CommerceCommander commerceCommander)
        :base(commerceCommander)
        {
        }

        protected override async Task ImportVariantsInventory(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context)
        {
            var sourceEntityHasVariants = importEntityArgument.ImportHandler.HasVariants();

            if (sourceEntityHasVariants)
            {
                var inventorySetName = importEntityArgument.ImportHandler
                    .GetPropertyValueFromSource<InventorySetNameAttribute, string>();

                if (!string.IsNullOrEmpty(inventorySetName))
                {
                    var inventorySetId = await GetInventorySetName(context, inventorySetName)
                        .ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(inventorySetId))
                    {
                        var variants = importEntityArgument.ImportHandler.GetVariants();

                        foreach (var variant in variants)
                        {
                            var inventoryDetail = variant.GetType()
                                .GetPropertyValueWithAttribute<InventoryDetailAttribute, InventoryDetail>(variant);

                            await ImportInventoryInformation(context, commerceEntity, inventorySetId, 
                                variant.Id, inventoryDetail).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}