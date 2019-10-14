using System;
using System.Linq;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Mappers
{
    public abstract class BaseComponentMapper<T> : IComponentMapper, IComponentLocalizationMapper
    where T : Component, new()
    {
        public IComponentHandler ComponentHandler { get; }

        public CommerceCommander CommerceCommander { get; }

        public CommercePipelineExecutionContext Context { get; }

        protected BaseComponentMapper(IComponentHandler componentHandler, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            ComponentHandler = componentHandler;
            CommerceCommander = commerceCommander;
            Context = context;
        }

        public virtual Component Map()
        {
            Type t = typeof(T);

            T component;

            if (!AllowMultipleComponents)
            {
                component = ComponentHandler.GetComponent(t) as T;
                if (component == null)
                {
                    component = Activator.CreateInstance(t) as T;
                    ComponentHandler.SetComponent(component);
                }
            }
            else
            {
                component = ComponentHandler.GetComponent(t, ComponentId) as T;
                if (component == null)
                {
                    component = Activator.CreateInstance(t) as T;
                    component = ComponentHandler.AddComponent(component) as T;
                }
            }

            Map(component);
            return component;
        }

        public virtual LocalizableComponentPropertiesValues Map(ILanguageEntity languageEntity, LocalizableComponentPropertiesValues localizableComponentPropertiesValues)
        {
            Type t = typeof(T);

            var component = Activator.CreateInstance(t) as T;

            MapLocalizeValues(component);

            if (localizableComponentPropertiesValues == null)
            {
                var entityComponentsLocalizableProperties =
                    LocalizablePropertyListManager.GetEntityComponentProperties(ComponentHandler.GetEntityType(),
                        Context);

                if (entityComponentsLocalizableProperties == null || !entityComponentsLocalizableProperties.Any())
                    return null;

                var path = GetLocalizableComponentPath(component);

                var componentProperties =
                    entityComponentsLocalizableProperties.FirstOrDefault(x =>
                        x.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

                if (componentProperties == null || !componentProperties.PropertyValues.Any())
                    return null;

                localizableComponentPropertiesValues = componentProperties.Clone();
            }


            var properties = TypePropertyListManager.GetProperties(t);
            
            foreach (var localizablePropertyValues in localizableComponentPropertiesValues.PropertyValues)
            {
                if (!string.IsNullOrEmpty(localizablePropertyValues.PropertyName))
                {
                    var propertyInfo = properties.FirstOrDefault(x =>
                        x.Name.Equals(localizablePropertyValues.PropertyName, StringComparison.OrdinalIgnoreCase));
                    if (propertyInfo != null)
                    {
                        var propertyValue = propertyInfo.GetValue(component);
                        var parameter = localizablePropertyValues.Parameters.FirstOrDefault(x =>
                            x.Key.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));

                        if (parameter == null)
                        {
                            parameter = new Parameter {Key = languageEntity.Language, Value = null};
                            localizablePropertyValues.Parameters.Add(parameter);
                        }

                        parameter.Value = propertyValue;
                    }
                }
            }

            return localizableComponentPropertiesValues;
        }

        protected virtual void Map(T component)
        {
        }

        protected virtual void MapLocalizeValues(T component)
        {
        }

        protected virtual string GetLocalizableComponentPath(T component)
        {
            return component.GetType().Name;
        }

        protected virtual bool AllowMultipleComponents => false;

        protected virtual string ComponentId => string.Empty;

        public virtual ComponentAction GetComponentAction()
        {
            return ComponentAction.Map;
        }

        public virtual Component Remove()
        {
            return ComponentHandler.RemoveComponent<T>();
        }
    }
}