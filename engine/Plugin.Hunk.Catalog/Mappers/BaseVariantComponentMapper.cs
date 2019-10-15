using System.Globalization;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Handlers;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Mappers
{
    public abstract class BaseVariantComponentMapper<TSourceEntity, TSourceVariant, TComponent> : BaseCommerceEntityComponentMapper<TSourceEntity, TComponent>
        where TSourceEntity : IEntity
        where TSourceVariant : IEntity
        where TComponent : Component, new()
    {
        protected TSourceVariant SourceVariant { get; }

        protected BaseVariantComponentMapper(TSourceEntity sourceEntity, TSourceVariant sourceVariant, CommerceEntity commerceEntity, Component parentComponent, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        : base(sourceEntity, new CommerceParentComponentChildComponentHandler(commerceEntity, parentComponent), commerceCommander, context)
        {
            SourceVariant = sourceVariant;
        }

        protected override string GetLocalizableComponentPath(TComponent component)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}",
                typeof(ItemVariationsComponent).Name,
                typeof(ItemVariationComponent).Name,
                component.GetType().Name);
        }
    }
}