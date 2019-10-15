using Plugin.Hunk.Catalog.Mappers;
using Plugin.Hunk.Catalog.Test.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SellableItemComponentMapper : BaseCommerceEntityComponentMapper<SourceProduct, SellableItemComponent>
    {
        public SellableItemComponentMapper(SourceProduct product, 
            SellableItem commerceEntity, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(product, commerceEntity, commerceCommander, context)
        { }

        protected override void Map(SellableItemComponent component)
        {
            component.Accessories = SourceEntity.Accessories;
            component.Dimensions = SourceEntity.Dimensions;
        }

        protected override void MapLocalizeValues(SellableItemComponent component)
        {
            component.Accessories = SourceEntity.Accessories;
            component.Dimensions = SourceEntity.Dimensions;
        }
    }
}