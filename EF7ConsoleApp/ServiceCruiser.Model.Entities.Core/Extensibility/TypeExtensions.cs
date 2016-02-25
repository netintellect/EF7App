using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceCruiser.Model.Entities.Core.Extensibility
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetPropertiesOfType<T>(this Type type)
        {
            var propertyInfos = new List<PropertyInfo>();

            var results = type.GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(T)));
            if (results.Any()) propertyInfos.AddRange(results);

            results = type.GetProperties().Where(p => p.IsCollectionOf<T>());
            if (results.Any()) propertyInfos.AddRange(results);

            return propertyInfos;
        }

        /// <summary>
        /// Returns all properties marked with the specified Attribute
        /// </summary>
        public static IEnumerable<PropertyInfo> GetPropertiesByAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            var attrProps = new List<PropertyInfo>();
            var properties = type.GetProperties().ToList();

            attrProps.AddRange(properties.Where(p => p.GetCustomAttributes(true).OfType<TAttribute>().Any()));

            return attrProps;
        }

        /// <summary>
        /// Returns property matching the given propertyName
        /// </summary>
        public static PropertyInfo GetPropertyByName(this Type type, string propertyName)
        {
            return type.GetProperties().FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// Returns true in case when the specified type is a collection Type
        /// </summary>
        public static bool IsCollection(this Type type)
        {
            if (type == null) return false;
            var tCollection = typeof(ICollection<>);

            if (type.IsGenericType && tCollection.IsAssignableFrom(type.GetGenericTypeDefinition()) ||
                type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == tCollection))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return true in case when the property is a collection of a specified type
        /// </summary>
        public static bool IsCollectionOf<T>(this Type type)
        {

            if (type.IsCollection() && type.GetGenericArguments().Any(a => a.IsSubclassOf(typeof(T))))
            {
                return true;
            }

            return false;
        }
    }
}
