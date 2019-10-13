using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class ComponentLocalizationMapperExtensions
    {
        public static IList<LocalizableComponentPropertiesValues> Execute(this IComponentLocalizationMapper mapper, IList<LocalizableComponentPropertiesValues> componentPropertiesList, Component component, ILanguageEntity languageEntity)
        {
            if (mapper == null)
                return componentPropertiesList;

            var componentProperties = componentPropertiesList.FirstOrDefault(x =>
                component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase));

            componentProperties = mapper.Map(languageEntity, componentProperties);
            if (componentProperties != null)
            {
                componentProperties.Id = component.Id;
                if (!componentPropertiesList.Any(x =>
                    component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase)))
                {
                    componentPropertiesList.Add(componentProperties);
                }
            }

            return componentPropertiesList;
        }
    }
}