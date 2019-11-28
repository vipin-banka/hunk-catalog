using Plugin.Hunk.Catalog.ImportHandlers;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.PromotionBookEntityImport
{
    public class SourcePromotionBookImportHandler : PromotionBookImportHandler<SourcePromotionBook>
    {
        public SourcePromotionBookImportHandler(string sourcePromotionBook, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(sourcePromotionBook, commerceCommander, context)
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