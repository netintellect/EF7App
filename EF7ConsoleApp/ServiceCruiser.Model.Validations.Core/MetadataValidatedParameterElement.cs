using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    /// <summary>
    /// Describes how validation must be performed on a parameter as defined by attributes.
    /// </summary>
    public class MetadataValidatedParameterElement : IValidatedElement
    {
        private ParameterInfo _parameterInfo;
        private IgnoreNullsAttribute _ignoreNullsAttribute;
        private ValidatorCompositionAttribute _validatorCompositionAttribute;

        public IEnumerable<IValidatorDescriptor> GetValidatorDescriptors()
        {
            if (_parameterInfo != null)
            {
                foreach (object attribute in _parameterInfo.GetCustomAttributes(typeof(ValidatorAttribute), false))
                {
                    yield return (IValidatorDescriptor)attribute;
                }
            }
        }

        public CompositionType CompositionType
        {
            get
            {
                return _validatorCompositionAttribute != null
                    ? _validatorCompositionAttribute.CompositionType
                    : CompositionType.And;
            }
        }

        public string CompositionMessageTemplate
        {
            get
            {
                return _validatorCompositionAttribute != null
                    ? _validatorCompositionAttribute.GetMessageTemplate()
                    : null;
            }
        }

        public string CompositionTag
        {
            get
            {
                return _validatorCompositionAttribute != null
                    ? _validatorCompositionAttribute.Tag
                    : null;
            }
        }

        public bool IgnoreNulls
        {
            get
            {
                return _ignoreNullsAttribute != null;
            }
        }

        public string IgnoreNullsMessageTemplate
        {
            get
            {
                return _ignoreNullsAttribute != null
                    ? _ignoreNullsAttribute.GetMessageTemplate()
                    : null;
            }
        }
        
        public string IgnoreNullsTag
        {
            get
            {
                return _ignoreNullsAttribute != null
                    ? _ignoreNullsAttribute.Tag
                    : null;
            }
        }
        
        public MemberInfo MemberInfo
        {
            get { return null; }
        }
        
        public Type TargetType
        {
            get { return null; }
        }
        
        public void UpdateFlyweight(ParameterInfo parameterInfo)
        {
            _parameterInfo = parameterInfo;
            _ignoreNullsAttribute = ValidationReflectionHelper.ExtractValidationAttribute<IgnoreNullsAttribute>(parameterInfo, string.Empty);
            _validatorCompositionAttribute = ValidationReflectionHelper.ExtractValidationAttribute<ValidatorCompositionAttribute>(parameterInfo, string.Empty);
        }
    }
}
