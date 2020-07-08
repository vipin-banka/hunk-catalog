using System.Linq;
using Plugin.Hunk.Catalog.ImportHandlers;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SourceBundleImportHandler : BundleImportHandler<SourceBundle>
    {
        public SourceBundleImportHandler(string sourceProduct, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
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
            Tags = SourceEntity.Tags.Select(t=>t.Name).ToList();

            BundleType = SourceEntity.BundleType;

            BundleItems = SourceEntity.BundleItems.Select(x => new BundleItem {
                SellableItemId = x.ItemId,
                Name = x.Name,
                Quantity = x.Quantity 
            }).ToList();
        }
        
        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
            CommerceEntity.Brand = SourceEntity.Brand;
            CommerceEntity.Manufacturer = SourceEntity.Manufacturer;
            CommerceEntity.TypeOfGood = SourceEntity.TypeOfGood;
            CommerceEntity.Tags = SourceEntity.Tags;
        }

        protected override void MapLocalizeValues(SourceBundle localizedSourceEntity, SellableItem localizedTargetEntity)
        {
            localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
            localizedTargetEntity.Description = localizedSourceEntity.Description;
        }
    }
}