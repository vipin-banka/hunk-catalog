using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog
{
    public static class LocalizablePropertyListManager
    {
        private static readonly IDictionary<Type, IList<LocalizablePropertyValues>> EntityLocalizableProperties = new ConcurrentDictionary<Type, IList<LocalizablePropertyValues>>();

        private static readonly IDictionary<Type, IList<LocalizableComponentPropertiesValues>> EntityComponentLocalizableProperties = new ConcurrentDictionary<Type, IList<LocalizableComponentPropertiesValues>>();

        private static readonly object LockEntityProperties = new object();

        private static readonly object LockEntityComponentProperties = new object();

        public static IList<LocalizablePropertyValues> GetEntityProperties(Type type, CommercePipelineExecutionContext context)
        {
            if (EntityLocalizableProperties.ContainsKey(type))
            {
                return EntityLocalizableProperties[type];
            }

            lock (LockEntityProperties)
            {
                if (EntityLocalizableProperties.ContainsKey(type))
                {
                    return EntityLocalizableProperties[type];
                }

                EntityLocalizableProperties.Add(type, context.GetEntityLocalizableProperties(type));
                return EntityLocalizableProperties[type];
            }
        }

        public static IList<LocalizableComponentPropertiesValues> GetEntityComponentProperties(Type type, CommercePipelineExecutionContext context)
        {
            if (EntityComponentLocalizableProperties.ContainsKey(type))
            {
                return EntityComponentLocalizableProperties[type];
            }

            lock (LockEntityComponentProperties)
            {
                if (EntityComponentLocalizableProperties.ContainsKey(type))
                {
                    return EntityComponentLocalizableProperties[type];
                }

                EntityComponentLocalizableProperties.Add(type, context.GetEntityComponentsLocalizableProperties(type));
                return EntityComponentLocalizableProperties[type];
            }
        }
    }
}