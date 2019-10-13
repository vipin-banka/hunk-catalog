using Plugin.Hunk.Catalog.Policy;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class ComponentExtensions
    {
        public static ComponentMetadataPolicy GetComponentMetadataPolicy(this Component component)
        {
            if (component.HasPolicy<ComponentMetadataPolicy>())
            {
                return component.GetPolicy<ComponentMetadataPolicy>();
            }

            return new ComponentMetadataPolicy();
        }

        public static void SetComponentMetadataPolicy(this Component component, string componentName)
        {
            component?.SetComponentMetadataPolicy(new ComponentMetadataPolicy() { MapperKey = componentName });
        }

        public static void SetComponentMetadataPolicy(this Component component, ComponentMetadataPolicy componentMetadataPolicy)
        {
            component?.SetPolicy(componentMetadataPolicy);
        }
    }
}