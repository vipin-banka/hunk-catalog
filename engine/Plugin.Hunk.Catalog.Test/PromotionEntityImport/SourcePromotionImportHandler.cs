using Plugin.Hunk.Catalog.ImportHandlers;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.PromotionEntityImport
{
    public class SourcePromotionImportHandler : PromotionImportHandler<SourcePromotion>
    {
        public SourcePromotionImportHandler(string sourcePromotion, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(sourcePromotion, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            Name = SourceEntity.Name;
            DisplayName = SourceEntity.DisplayName;
            Description = SourceEntity.Description;
            BookName = SourceEntity.BookName;
            ValidFrom = SourceEntity.ValidFrom;
            ValidTo = SourceEntity.ValidTo;
            Text = SourceEntity.Text;
            CartText = SourceEntity.CartText;
            IsExclusive = SourceEntity.IsExclusive;
        }

        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
            CommerceEntity.DisplayText = SourceEntity.Text;
            CommerceEntity.DisplayCartText = SourceEntity.CartText;
            CommerceEntity.ValidFrom = SourceEntity.ValidFrom;
            CommerceEntity.ValidTo = SourceEntity.ValidTo;
        }
    }
}