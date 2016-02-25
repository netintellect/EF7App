using System;
using System.Reflection;

namespace ServiceCruiser.Model.Validations.Core
{
    public class ValidationAttributeValidatorBuilder : ValidatorBuilderBase
    {
        public ValidationAttributeValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory,
                                                   ValidatorFactory validatorFactory) : base(memberAccessValidatorFactory, validatorFactory)
        { }

        public Validator CreateValidator(Type type)
        {
            return CreateValidator(new ValidationAttributeValidatedType(type));
        }

        internal Validator CreateValidatorForProperty(PropertyInfo propertyInfo)
        {
            return CreateValidatorForValidatedElement(new ValidationAttributeValidatedElement(propertyInfo),
                                                      GetCompositeValidatorBuilderForProperty);
        }
    }
}
