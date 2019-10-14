using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class ComponentMapperExtensions
    {
        public static Component Execute(this IComponentMapper mapper,  ComponentAction? componentAction = null)
        {
            var action = componentAction ?? mapper.GetComponentAction();
            Component component = null;
            switch (action)
            {
                case ComponentAction.Map:
                    component = mapper.Map();
                    break;
                case ComponentAction.Remove:
                    component = mapper.Remove();
                    break;
            }

            return component;
        }
    }
}