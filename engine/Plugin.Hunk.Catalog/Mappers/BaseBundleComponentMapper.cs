using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Mappers
{
    public abstract class BaseBundleComponentMapper<TSourceEntity> : BaseCommerceEntityComponentMapper<TSourceEntity, BundleComponent>
    where TSourceEntity : IEntity
    {
        protected BaseBundleComponentMapper(TSourceEntity sourceEntity, CommerceEntity commerceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        : base(sourceEntity, commerceEntity, commerceCommander, context)
        {

        }

        protected BaseBundleComponentMapper(TSourceEntity sourceEntity, IComponentHandler componentHandler, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceEntity, componentHandler, commerceCommander, context)
        {

        }
    }
}