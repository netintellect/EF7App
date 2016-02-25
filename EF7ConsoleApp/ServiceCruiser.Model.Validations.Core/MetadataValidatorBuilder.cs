using System;
using System.Reflection;

namespace ServiceCruiser.Model.Validations.Core
{
    public class MetadataValidatorBuilder : ValidatorBuilderBase
    {
        public MetadataValidatorBuilder() : this(MemberAccessValidatorBuilderFactory.Default)
        { }
        
        public MetadataValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory)
            : this(memberAccessValidatorFactory, ValidationFactory.DefaultCompositeValidatorFactory)
        { }

        public MetadataValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory,
            ValidatorFactory validatorFactory) : base(memberAccessValidatorFactory, validatorFactory)
        { }
        
        public Validator CreateValidator(Type type, string ruleset)
        {
            return CreateValidator(new MetadataValidatedType(type, ruleset));
        }

        #region test only methods
        
        public Validator CreateValidatorForType(Type type, string ruleset)
        {
            return CreateValidatorForValidatedElement(new MetadataValidatedType(type, ruleset),
                                                      GetCompositeValidatorBuilderForType);
        }
        
        public Validator CreateValidatorForProperty(PropertyInfo propertyInfo, string ruleset)
        {
            return CreateValidatorForValidatedElement(new MetadataValidatedElement(propertyInfo, ruleset),
                GetCompositeValidatorBuilderForProperty);
        }
        
        public Validator CreateValidatorForField(FieldInfo fieldInfo, string ruleset)
        {
            return CreateValidatorForValidatedElement(new MetadataValidatedElement(fieldInfo, ruleset),
                GetCompositeValidatorBuilderForField);
        }

        public Validator CreateValidatorForMethod(MethodInfo methodInfo, string ruleset)
        {
            return CreateValidatorForValidatedElement(new MetadataValidatedElement(methodInfo, ruleset),
                GetCompositeValidatorBuilderForMethod);
        }

        #endregion
    }
}
