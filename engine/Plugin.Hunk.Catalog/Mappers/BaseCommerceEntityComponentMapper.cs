using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Handlers;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Mappers
{
    public abstract class BaseCommerceEntityComponentMapper<TSourceEntity, TComponent> : BaseComponentMapper<TComponent>
    where TSourceEntity : IEntity
    where TComponent : Component, new()
    {
        public TSourceEntity SourceEntity { get; }

        protected BaseCommerceEntityComponentMapper(TSourceEntity sourceEntity, CommerceEntity commerceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(new CommerceEntityComponentHandler(commerceEntity), commerceCommander, context)
        {
            SourceEntity = sourceEntity;
        }

        protected BaseCommerceEntityComponentMapper(TSourceEntity sourceEntity, IComponentHandler componentHandler, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(componentHandler, commerceCommander, context)
        {
            SourceEntity = sourceEntity;
        }
    }
}