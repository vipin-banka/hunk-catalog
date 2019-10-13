using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class CommercePipelineExecutionContextExtensions
    {
        public static IList<LocalizableComponentPropertiesValues> GetEntityComponentsLocalizableProperties(this CommercePipelineExecutionContext context, Type type)
        {
            var result = new List<LocalizableComponentPropertiesValues>();
            var localizeEntityPolicy = LocalizeEntityPolicy.GetPolicyByType(context.CommerceContext, type);
            if (localizeEntityPolicy?.ComponentsPolicies == null || !localizeEntityPolicy.ComponentsPolicies.Any())
            {
                return result;
            }

            foreach (var componentsPolicy in localizeEntityPolicy.ComponentsPolicies)
            {
                if (componentsPolicy.Properties == null || !componentsPolicy.Properties.Any())
                    continue;

                var componentProperties = new LocalizableComponentPropertiesValues();
                result.Add(componentProperties);
                componentProperties.Path = componentsPolicy.Path;
                componentProperties.PropertyValues = new List<LocalizablePropertyValues>();
                foreach (var componentsPolicyProperty in componentsPolicy.Properties)
                {
                    componentProperties.PropertyValues.Add(new LocalizablePropertyValues
                    {
                        PropertyName = componentsPolicyProperty
                    });
                }
            }

            return result;
        }

        public static IList<LocalizablePropertyValues> GetEntityLocalizableProperties(this CommercePipelineExecutionContext context, Type type)
        {
            var result = new List<LocalizablePropertyValues>();
            var localizeEntityPolicy = LocalizeEntityPolicy.GetPolicyByType(context.CommerceContext, type);
            if (localizeEntityPolicy?.Properties == null || !localizeEntityPolicy.Properties.Any())
            {
                return result;
            }

            foreach (var property in localizeEntityPolicy.Properties)
            {
                result.Add(new LocalizablePropertyValues()
                {
                    PropertyName = property
                });
            }

            return result;
        }
    }
}