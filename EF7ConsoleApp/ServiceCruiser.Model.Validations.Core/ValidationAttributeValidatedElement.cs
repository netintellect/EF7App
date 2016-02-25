using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public class ValidationAttributeValidatedElement : IValidatedElement
    {
        private readonly MemberInfo _memberInfo;
        private readonly Type _targetType;

        public ValidationAttributeValidatedElement(PropertyInfo propertyInfo) : this(propertyInfo, GetPropertyType(propertyInfo))
        { }

        public ValidationAttributeValidatedElement(FieldInfo fieldInfo) : this(fieldInfo, GetFieldType(fieldInfo))
        { }
        
        protected ValidationAttributeValidatedElement(MemberInfo memberInfo, Type targetType)
        {
            _memberInfo = memberInfo;
            _targetType = targetType;
        }

        private static Type GetPropertyType(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");
            return propertyInfo.PropertyType;
        }

        private static Type GetFieldType(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");
            return fieldInfo.FieldType;
        }

        IEnumerable<IValidatorDescriptor> IValidatedElement.GetValidatorDescriptors()
        {
            var attributes = ValidationReflectionHelper.GetCustomAttributes(_memberInfo, typeof(ValidationAttribute), true)
                                                       .Cast<ValidationAttribute>()
                                                       .Where(a => !typeof(BaseValidationAttribute).IsAssignableFrom(a.GetType()))
                                                       .ToArray();
            if (attributes.Length == 0)
            {
                return new IValidatorDescriptor[0];
            }
            return new IValidatorDescriptor[] 
            {
                new ValidationAttributeValidatorDescriptor(attributes)
            };
        }

        CompositionType IValidatedElement.CompositionType
        {
            get { return CompositionType.And; }
        }

        string IValidatedElement.CompositionMessageTemplate
        {
            get { return null; }
        }

        string IValidatedElement.CompositionTag
        {
            get { return null; }
        }

        bool IValidatedElement.IgnoreNulls
        {
            get { return false; }
        }

        string IValidatedElement.IgnoreNullsMessageTemplate
        {
            get { return null; }
        }

        string IValidatedElement.IgnoreNullsTag
        {
            get { return null; }
        }

        MemberInfo IValidatedElement.MemberInfo
        {
            get { return _memberInfo; }
        }

        Type IValidatedElement.TargetType
        {
            get { return _targetType; }
        }

        internal class ValidationAttributeValidatorDescriptor : IValidatorDescriptor
        {
            private readonly IEnumerable<ValidationAttribute> attributes;

            internal ValidationAttributeValidatorDescriptor(IEnumerable<ValidationAttribute> attributes)
            {
                this.attributes = attributes;
            }

            public Validator CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder, ValidatorFactory ignored)
            {
                return new ValidationAttributeValidator(attributes);
            }
        }
    }
}
