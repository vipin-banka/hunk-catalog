using Plugin.Hunk.Catalog.ImportHandlers;
using Plugin.Hunk.Catalog.Test.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.EntityImportHandlers
{
    public class SourceCategoryImportHandler : CategoryImportHandler<SourceCategory>
    {
        public SourceCategoryImportHandler(string sourceCategory, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceCategory, commerceCommander, context)
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

        protected override void MapLocalizeValues(SourceCategory localizedSourceEntity, Sitecore.Commerce.Plugin.Catalog.Category localizedTargetEntity)
        {
            localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
            localizedTargetEntity.Description = localizedSourceEntity.Description;
        }
    }
}