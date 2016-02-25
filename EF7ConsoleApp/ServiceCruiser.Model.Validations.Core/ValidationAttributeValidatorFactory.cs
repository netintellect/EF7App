using System;
using ServiceCruiser.Model.Validations.Core.Instrumentation;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public class ValidationAttributeValidatorFactory : ValidatorFactory
    {
        private static readonly Validator EmptyValidator = new AndCompositeValidator();

        public ValidationAttributeValidatorFactory(IValidationInstrumentationProvider instrumentationProvider) : base(instrumentationProvider)
        { }

        protected internal override Validator InnerCreateValidator(Type targetType, string ruleset, ValidatorFactory mainValidatorFactory)
        {
            if (string.IsNullOrEmpty(ruleset))
            {
                var builder = new ValidationAttributeValidatorBuilder(MemberAccessValidatorBuilderFactory.Default, mainValidatorFactory);

                return builder.CreateValidator(targetType);
            }
            return EmptyValidator;
        }
    }
}
