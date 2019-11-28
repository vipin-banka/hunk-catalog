using Plugin.Hunk.Catalog.ImportHandlers;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.PriceCardEntityImport
{
    public class SourcePriceCardImportHandler : PriceCardImportHandler<SourcePriceCard>
    {
        public SourcePriceCardImportHandler(string sourcePriceCard, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(sourcePriceCard, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            Name = SourceEntity.Name;
            DisplayName = SourceEntity.DisplayName;
            Description = SourceEntity.Description;
            BookName = SourceEntity.BookName;
        }

        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
        }
    }
}