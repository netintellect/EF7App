using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceCruiser.Model.Validations.Core
{
    public class ValidationAttributeValidatedType : ValidationAttributeValidatedElement, IValidatedType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttributeValidatedType"/> for a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to represent.</param>
        public ValidationAttributeValidatedType(Type type) : base(type, type)
        { }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedProperties()
        {
            foreach (PropertyInfo propertyInfo in
                ((IValidatedElement)this).TargetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (ValidationReflectionHelper.IsValidProperty(propertyInfo))
                {
                    yield return new ValidationAttributeValidatedElement(propertyInfo);
                }
            }
        }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedFields()
        {
            foreach (FieldInfo fieldInfo in
                ((IValidatedElement)this).TargetType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (ValidationReflectionHelper.IsValidField(fieldInfo))
                {
                    yield return new ValidationAttributeValidatedElement(fieldInfo);
                }
            }
        }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedMethods()
        {
            return Enumerable.Empty<IValidatedElement>(); ;
        }

        IEnumerable<MethodInfo> IValidatedType.GetSelfValidationMethods()
        {
            return Enumerable.Empty<MethodInfo>(); ;
        }
    }
}
