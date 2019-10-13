using System;
using System.Linq;
using System.Reflection;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class TypeExtensions
    {
        public static TOutput GetPropertyValueWithAttribute<TCustomAttribute, TOutput>(this Type type, object instance)
            where TCustomAttribute : Attribute
            where TOutput : class
        {
            var property = type.GetPropertyWithAttribute<TCustomAttribute>();
            if (property != null)
            {
                return property.GetValue(instance) as TOutput;
            }

            return default(TOutput);
        }

        public static PropertyInfo GetPropertyWithAttribute<T>(this Type type) where T : Attribute
        {
            return type
                .GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(T)));
        }
    }
}