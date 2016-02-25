using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.DataAnnotations;
using ServiceCruiser.Model.Validations.Core.Resources;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public static class ValidationReflectionHelper
    {
        internal static PropertyInfo GetProperty(Type type, string propertyName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (!IsValidProperty(propertyInfo))
            {
                if (throwIfInvalid)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Translations.ExceptionInvalidProperty,
                                                              propertyName, type.FullName));
                }
                return null;
            }

            return propertyInfo;
        }

        internal static bool IsValidProperty(PropertyInfo propertyInfo)
        {
            return null != propertyInfo				// exists
                    && propertyInfo.CanRead			// and it's readable
                    && propertyInfo.GetIndexParameters().Length == 0;	// and it's not an indexer
        }

        internal static FieldInfo GetField(Type type, string fieldName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentNullException("fieldName");

            FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

            if (!IsValidField(fieldInfo))
            {
                if (throwIfInvalid)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Translations.ExceptionInvalidField,
                                                fieldName, type.FullName));
                }
                return null;
            }

            return fieldInfo;
        }

        internal static bool IsValidField(FieldInfo fieldInfo)
        {
            return null != fieldInfo;
        }

        internal static MethodInfo GetMethod(Type type, string methodName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }

            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            if (!IsValidMethod(methodInfo))
            {
                if (throwIfInvalid)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Translations.ExceptionInvalidMethod,
                                                methodName, type.FullName));
                }
                return null;
            }

            return methodInfo;
        }

        internal static bool IsValidMethod(MethodInfo methodInfo)
        {
            return null != methodInfo
                && typeof(void) != methodInfo.ReturnType
                && methodInfo.GetParameters().Length == 0;
        }

        internal static T ExtractValidationAttribute<T>(MemberInfo attributeProvider, string ruleset) where T : BaseValidationAttribute
        {
            if (attributeProvider != null)
            {
                foreach (T attribute in GetCustomAttributes(attributeProvider, typeof(T), false))
                {
                    if (ruleset.Equals(attribute.Ruleset))
                    {
                        return attribute;
                    }
                }
            }

            return null;
        }

        internal static T ExtractValidationAttribute<T>(ParameterInfo attributeProvider, string ruleset) where T : BaseValidationAttribute
        {
            if (attributeProvider != null)
            {
                foreach (T attribute in attributeProvider.GetCustomAttributes(typeof(T), false))
                {
                    if (ruleset.Equals(attribute.Ruleset))
                    {
                        return attribute;
                    }
                }
            }

            return null;
        }
        
        public static Attribute[] GetCustomAttributes(MemberInfo element, Type attributeType, bool inherit)
        {
            MemberInfo matchingElement = GetMatchingElement(element);

            return Attribute.GetCustomAttributes(matchingElement, attributeType, inherit);
        }

        private static MemberInfo GetMatchingElement(MemberInfo element)
        {
            var sourceType = element as Type;
            bool elementIsType = sourceType != null;
            if (sourceType == null)
            {
                sourceType = element.DeclaringType;
            }

            var metadataTypeAttribute = (MetadataTypeAttribute)Attribute.GetCustomAttribute(sourceType, typeof(MetadataTypeAttribute), false);

            if (metadataTypeAttribute == null)
            {
                return element;
            }

            sourceType = metadataTypeAttribute.MetadataClassType;

            if (elementIsType)
            {
                return sourceType;
            }

            MemberInfo[] matchingMembers =
                sourceType.GetMember(element.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (matchingMembers.Length > 0)
            {
                var methodBase = element as MethodBase;
                if (methodBase == null)
                {
                    return matchingMembers[0];
                }

                Type[] parameterTypes = methodBase.GetParameters().Select(pi => pi.ParameterType).ToArray();
                return matchingMembers.Cast<MethodBase>().FirstOrDefault(mb => MatchMethodBase(mb, parameterTypes)) ?? element;
            }

            return element;
        }

        private static bool MatchMethodBase(MethodBase mb, Type[] parameterTypes)
        {
            ParameterInfo[] parameters = mb.GetParameters();

            if (parameters.Length != parameterTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != parameterTypes[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
