using Plugin.Hunk.Catalog.Mappers;
using Plugin.Hunk.Catalog.Test.Components;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class VariantComponentMapper : BaseVariantComponentMapper<SourceProduct, SourceProductVariant, CommerceEntity, VariantComponent>
    {
        public VariantComponentMapper(SourceProduct product, SourceProductVariant productVariant, CommerceEntity commerceEntity, Component parentComponent, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            :base(product, productVariant, commerceEntity, parentComponent, commerceCommander, context)
        { }

        protected override void Map(VariantComponent component)
        {
            component.Breadth = SourceVariant.Breadth;
            component.Length = SourceVariant.Length;
        }

        protected override void MapLocalizeValues(VariantComponent component)
        {
            component.Breadth = SourceVariant.Breadth;
            component.Length = SourceVariant.Length;
        }
    }
}