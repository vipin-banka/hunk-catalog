using System;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface IComponentHandler
    {
        Component GetComponent(Type type);

        Component GetComponent(Type type, string id);

        void SetComponent(Component component);

        Component AddComponent(Component component);

        Component RemoveComponent<T>() where T : Component;

        Type GetEntityType();
    }
}