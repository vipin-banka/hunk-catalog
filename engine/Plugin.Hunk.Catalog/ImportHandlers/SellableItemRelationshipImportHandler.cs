using Plugin.Hunk.Catalog.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public class SellableItemRelationshipImportHandler : BaseEntityImportHandler<EntityRelationship, SellableItem>
    {
        public SellableItemRelationshipImportHandler(string sourceEntity,
            CommerceCommander commerceCommander,
            CommercePipelineExecutionContext context)
            : base(sourceEntity, commerceCommander, context)
        {
        }

        public override bool IsEntityImport => false;
    }
}