using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public class MetadataValidatedType : MetadataValidatedElement, IValidatedType
    {
        public MetadataValidatedType(Type targetType, string ruleset) : base(targetType, ruleset)
        { }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedProperties()
        {
            var flyweight = new MetadataValidatedElement(Ruleset);

            foreach (PropertyInfo propertyInfo in TargetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (ValidationReflectionHelper.IsValidProperty(propertyInfo))
                {
                    flyweight.UpdateFlyweight(propertyInfo);
                    yield return flyweight;
                }
            }
        }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedFields()
        {
            var flyweight = new MetadataValidatedElement(Ruleset);

            foreach (FieldInfo fieldInfo in TargetType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                flyweight.UpdateFlyweight(fieldInfo);
                yield return flyweight;
            }
        }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedMethods()
        {
            var flyweight = new MetadataValidatedElement(Ruleset);

            foreach (MethodInfo methodInfo in TargetType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();

                if (ValidationReflectionHelper.IsValidMethod(methodInfo))
                {
                    flyweight.UpdateFlyweight(methodInfo);
                    yield return flyweight;
                }
            }
        }

        IEnumerable<MethodInfo> IValidatedType.GetSelfValidationMethods()
        {
            Type type = TargetType;

            if (ValidationReflectionHelper.GetCustomAttributes(type, typeof(HasSelfValidationAttribute), false).Length == 0)
                yield break;		// no self validation for the current type, ignore type

            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                bool hasReturnType = methodInfo.ReturnType != typeof(void);
                ParameterInfo[] parameters = methodInfo.GetParameters();

                if (!hasReturnType && parameters.Length == 1 && parameters[0].ParameterType == typeof(ValidationResults))
                {
                    foreach (SelfValidationAttribute attribute in ValidationReflectionHelper.GetCustomAttributes(methodInfo, typeof(SelfValidationAttribute), false))
                    {
                        if (Ruleset.Equals(attribute.Ruleset))
                        {
                            yield return methodInfo;
                        }
                    }
                }
            }
        }
    }
}
