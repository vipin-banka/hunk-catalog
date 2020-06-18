using Plugin.Hunk.Catalog.Mappers;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Linq;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class BundleComponentMapper : BaseBundleComponentMapper<SourceBundle>
    {
        public BundleComponentMapper(SourceBundle bundle, 
            CommerceEntity commerceEntity, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            :base(bundle, commerceEntity, commerceCommander, context)
        { }

        protected override void Map(BundleComponent component)
        {
            component.BundleType = SourceEntity.BundleType;

            component.BundleItems.RemoveAll(bi => !SourceEntity.BundleItems.Any(sbi => sbi.ItemId == bi.SellableItemId));

            foreach (var sbi in SourceEntity.BundleItems)
            {
                var bundleItem = component.BundleItems.FirstOrDefault(bi => bi.SellableItemId == sbi.ItemId);
                if (bundleItem == null)
                {
                    bundleItem = new BundleItem();

                    bundleItem.SellableItemId = sbi.ItemId;
                    component.BundleItems.Add(bundleItem);
                }

                bundleItem.Name = sbi.Name;
                bundleItem.Quantity = sbi.Quantity;
            }
        }

        protected override void MapLocalizeValues(BundleComponent component)
        {

        }
    }
}