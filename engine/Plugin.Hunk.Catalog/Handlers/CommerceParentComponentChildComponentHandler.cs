using System;
using System.Linq;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Handlers
{
    public class CommerceParentComponentChildComponentHandler : CommerceEntityComponentHandler
    {
        public Component ParentComponent { get; }

        public CommerceParentComponentChildComponentHandler(CommerceEntity commerceEntity, Component parentComponent) : base(commerceEntity)
        {
            ParentComponent = parentComponent;
        }

        public override Component GetComponent(Type type)
        {
            return ParentComponent.ChildComponents.FirstOrDefault(c => c.GetType() == type);
        }

        public override Component GetComponent(Type type, string id)
        {
            return ParentComponent.ChildComponents.FirstOrDefault(c => c.GetType() == type && c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public override void SetComponent(Component component)
        {
            ParentComponent.SetComponent(component);
        }

        public override Component AddComponent(Component component)
        {
            var existingComponent = GetComponent(component.GetType(), component.Id);
            if (existingComponent == null)
            {
                ParentComponent.ChildComponents.Add(component);
                return component;
            }

            return existingComponent;
        }

        public override Component RemoveComponent<T>()
        {
            var component = GetComponent(typeof(T));
            if (component != null)
            {
                ParentComponent.RemoveComponent(typeof(T));
            }

            return component;
        }
    }
}