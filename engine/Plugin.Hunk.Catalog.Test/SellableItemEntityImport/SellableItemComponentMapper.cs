using Plugin.Hunk.Catalog.Mappers;
using Plugin.Hunk.Catalog.Test.Components;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SellableItemComponentMapper : BaseEntityComponentMapper<SourceProduct, CommerceEntity, SellableItemComponent>
    {
        public SellableItemComponentMapper(SourceProduct product, CommerceEntity commerceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
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