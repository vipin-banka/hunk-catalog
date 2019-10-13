using Plugin.Hunk.Catalog.ImportHandlers;
using Plugin.Hunk.Catalog.Test.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Test.EntityImportHandlers
{
    public class SourceProductImportHandler : SellableItemImportHandler<SourceProduct>
    {
        public SourceProductImportHandler(string sourceProduct, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceProduct, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            ProductId = SourceEntity.Id;
            Name = SourceEntity.Name;
            DisplayName = SourceEntity.DisplayName;
            Description = SourceEntity.Description;
            Brand = SourceEntity.Brand;
            Manufacturer = SourceEntity.Manufacturer;
            TypeOfGood = SourceEntity.TypeOfGood;
            Tags = SourceEntity.Tags;
        }
        
        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
        }

        protected override void MapLocalizeValues(SourceProduct localizedSourceEntity, SellableItem localizedTargetEntity)
        {
            localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
            localizedTargetEntity.Description = localizedSourceEntity.Description;
        }
    }
}