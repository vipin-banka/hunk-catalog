using System;
using System.Linq;
using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Handlers
{
    public class CommerceEntityComponentHandler : IComponentHandler
    {
        public CommerceEntity ParenEntity { get; }

        public CommerceEntityComponentHandler(CommerceEntity parenEntity)
        {
            ParenEntity = parenEntity;
        }

        public virtual Component GetComponent(Type type)
        {
            return ParenEntity.EntityComponents.FirstOrDefault(c => c.GetType() == type);
        }

        public virtual Component GetComponent(Type type, string id)
        {
            return ParenEntity.EntityComponents.FirstOrDefault(c => 
                (!string.IsNullOrEmpty(id) && c.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                || c.GetType() == type);
        }

        public virtual void SetComponent(Component component)
        {
            ParenEntity.SetComponent(component);
        }

        public virtual Component AddComponent(Component component)
        {
            var existingComponent = GetComponent(component.GetType(), component.Id);
            if (existingComponent == null)
            {
                ParenEntity.SetComponent(component);
                return component;
            }

            return existingComponent;
        }

        public virtual Component RemoveComponent<T>() where T : Component
        {
            var component = GetComponent(typeof(T));
            if (component != null)
            {
                ParenEntity.RemoveComponent(typeof(T));
            }

            return component;
        }

        public virtual Type GetEntityType()
        {
            return ParenEntity.GetType();
        }
    }
}