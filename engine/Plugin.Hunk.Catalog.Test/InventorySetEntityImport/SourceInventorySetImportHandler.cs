using Plugin.Hunk.Catalog.ImportHandlers;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.InventorySetEntityImport
{
    public class SourceInventorySetImportHandler : InventorySetImportHandler<SourceInventorySet>
    {
        public SourceInventorySetImportHandler(string sourceInventorySet, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(sourceInventorySet, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            Name = SourceEntity.Name;
            DisplayName = SourceEntity.DisplayName;
            Description = SourceEntity.Description;
        }

        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
        }
    }
}