using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Plugin.Hunk.Catalog
{
    public static class TypePropertyListManager
    {
        private static readonly IDictionary<Type, IList<PropertyInfo>> TypeProperties = new ConcurrentDictionary<Type, IList<PropertyInfo>>();

        private static readonly object Lock = new object();

        public static IList<PropertyInfo> GetProperties(Type type)
        {
            if (TypeProperties.ContainsKey(type))
            {
                return TypeProperties[type];
            }

            lock (Lock)
            {
                if (TypeProperties.ContainsKey(type))
                {
                    return TypeProperties[type];
                }

                TypeProperties.Add(type, type.GetProperties());
                return TypeProperties[type];
            }
        }
    }
}