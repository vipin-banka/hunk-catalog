using Plugin.Hunk.Catalog.ImportHandlers;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.PriceBookEntityImport
{
    public class SourcePriceBookImportHandler : PriceBookImportHandler<SourcePriceBook>
    {
        public SourcePriceBookImportHandler(string sourcePriceBook, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(sourcePriceBook, commerceCommander, context)
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