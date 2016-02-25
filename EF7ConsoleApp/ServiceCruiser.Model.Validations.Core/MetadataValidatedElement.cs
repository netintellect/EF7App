using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public class MetadataValidatedElement : IValidatedElement
    {
        private MemberInfo _memberInfo;
        private IgnoreNullsAttribute _ignoreNullsAttribute;
        private ValidatorCompositionAttribute _validatorCompositionAttribute;
        private Type _targetType;
        private readonly string _ruleset;

        public MetadataValidatedElement(string ruleset)
        {
            _ruleset = ruleset;
        }

        public MetadataValidatedElement(FieldInfo fieldInfo, string ruleset) : this(ruleset)
        {
            UpdateFlyweight(fieldInfo);
        }

        public MetadataValidatedElement(MethodInfo methodInfo, string ruleset) : this(ruleset)
        {
            UpdateFlyweight(methodInfo);
        }
        
        public MetadataValidatedElement(PropertyInfo propertyInfo, string ruleset) : this(ruleset)
        {
            UpdateFlyweight(propertyInfo);
        }
        
        public MetadataValidatedElement(Type type, string ruleset) : this(ruleset)
        {
            UpdateFlyweight(type);
        }

        public void UpdateFlyweight(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");

            UpdateFlyweight(fieldInfo, fieldInfo.FieldType);
        }

        public void UpdateFlyweight(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException("methodInfo");

            UpdateFlyweight(methodInfo, methodInfo.ReturnType);
        }

        public void UpdateFlyweight(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");

            UpdateFlyweight(propertyInfo, propertyInfo.PropertyType);
        }

        public void UpdateFlyweight(Type type)
        {
            UpdateFlyweight(type, type);
        }

        private void UpdateFlyweight(MemberInfo memberInfo, Type targetType)
        {
            _memberInfo = memberInfo;
            _targetType = targetType;
            _ignoreNullsAttribute = ValidationReflectionHelper.ExtractValidationAttribute<IgnoreNullsAttribute>(memberInfo, _ruleset);
            _validatorCompositionAttribute = ValidationReflectionHelper.ExtractValidationAttribute<ValidatorCompositionAttribute>(memberInfo, _ruleset);
        }

        protected string Ruleset
        {
            get { return _ruleset; }
        }

        protected Type TargetType
        {
            get { return _targetType; }
        }

        IEnumerable<IValidatorDescriptor> IValidatedElement.GetValidatorDescriptors()
        {
            if (_memberInfo == null) yield break;

            foreach (var attribute in ValidationReflectionHelper.GetCustomAttributes(_memberInfo, typeof(ValidatorAttribute), false))
            {
                var validationAttribute = attribute as ValidatorAttribute;
                if (validationAttribute == null) continue;
                if (_ruleset.Equals(validationAttribute.Ruleset))
                    yield return validationAttribute;
            }
        }

        CompositionType IValidatedElement.CompositionType
        {
            get
            {
                return _validatorCompositionAttribute != null
                    ? _validatorCompositionAttribute.CompositionType
                    : CompositionType.And;
            }
        }

        string IValidatedElement.CompositionMessageTemplate
        {
            get
            {
                return this._validatorCompositionAttribute != null
                    ? this._validatorCompositionAttribute.GetMessageTemplate()
                    : null;
            }
        }

        string IValidatedElement.CompositionTag
        {
            get
            {
                return _validatorCompositionAttribute != null
                    ? _validatorCompositionAttribute.Tag
                    : null;
            }
        }

        bool IValidatedElement.IgnoreNulls
        {
            get
            {
                return _ignoreNullsAttribute != null;
            }
        }

        string IValidatedElement.IgnoreNullsMessageTemplate
        {
            get
            {
                return _ignoreNullsAttribute != null
                    ? _ignoreNullsAttribute.GetMessageTemplate()
                    : null;
            }
        }

        string IValidatedElement.IgnoreNullsTag
        {
            get
            {
                return _ignoreNullsAttribute != null
                    ? _ignoreNullsAttribute.Tag
                    : null;
            }
        }

        MemberInfo IValidatedElement.MemberInfo
        {
            get
            {
                return _memberInfo;
            }
        }

        Type IValidatedElement.TargetType
        {
            get
            {
                return _targetType;
            }
        }
    }
}
