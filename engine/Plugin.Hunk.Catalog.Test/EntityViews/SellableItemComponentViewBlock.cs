using System;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Test.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Test.EntityViews
{
    [PipelineDisplayName(Constants.SellableItemComponentViewBlock)]
    public class SellableItemComponentViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override async Task<EntityView> Run(
          EntityView arg,
          CommercePipelineExecutionContext context)
        {
            KnownCatalogViewsPolicy policy = context.GetPolicy<KnownCatalogViewsPolicy>();
            EntityViewArgument entityViewArgument = context.CommerceContext.GetObject<EntityViewArgument>();
            if (string.IsNullOrEmpty(entityViewArgument?.ViewName) || !entityViewArgument.ViewName.Equals(policy.Master, StringComparison.OrdinalIgnoreCase))
                return await Task.FromResult(arg);

            if (!entityViewArgument.Entity.HasComponent<SellableItemComponent>())
            {
                return arg;
            }

            var component = entityViewArgument.Entity.GetComponent<SellableItemComponent>();
            var childView = new EntityView
            {
                Name = "SellableItemComponentDetails",
                DisplayName = "Sellable Item Custom Component Details",
                EntityId = entityViewArgument.EntityId,
                EntityVersion = entityViewArgument.Entity.EntityVersion
            };

            arg.ChildViews.Add(childView);

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemComponent.Accessories),
                DisplayName = "Accessories",
                IsRequired = false,
                RawValue = component?.Accessories,
                Value = component?.Accessories,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(SellableItemComponent.Dimensions),
                DisplayName = "Dimensions",
                IsRequired = false,
                RawValue = component?.Dimensions,
                Value = component?.Dimensions,
                IsReadOnly = true
            });
            
            return arg;
        }
    }
}
