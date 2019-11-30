using Plugin.Hunk.Catalog.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public class SellableItemInventoryImportHandler : BaseEntityImportHandler<SellableItemInventory, SellableItem>
    {
        public SellableItemInventoryImportHandler(string sourceEntity,
            CommerceCommander commerceCommander,
            CommercePipelineExecutionContext context)
            : base(sourceEntity, commerceCommander, context)
        {
        }

        public override bool IsEntityImport => false;
    }
}