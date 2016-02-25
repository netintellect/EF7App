using System;
using System.Collections.Generic;
using System.Linq;
using ServiceCruiser.Model.Validations.Core.Common.Utility;
using ServiceCruiser.Model.Validations.Core.Instrumentation;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public class CompositeValidatorFactory : ValidatorFactory
    {
        private readonly IEnumerable<ValidatorFactory> _validatorFactories;
        
        public CompositeValidatorFactory(IValidationInstrumentationProvider instrumentationProvider, IEnumerable<ValidatorFactory> validatorFactories) : base(instrumentationProvider)
        {
            _validatorFactories = validatorFactories;
        }
        
        public CompositeValidatorFactory(
            IValidationInstrumentationProvider instrumentationProvider,
            AttributeValidatorFactory attributeValidatorFactory)
            : this(instrumentationProvider, new ValidatorFactory[] { attributeValidatorFactory })
        {
        }
        
        public CompositeValidatorFactory(IValidationInstrumentationProvider instrumentationProvider,
                                         AttributeValidatorFactory attributeValidatorFactory,
                                         ValidationAttributeValidatorFactory validationAttributeValidatorFactory) 
            : this(instrumentationProvider,
                   new ValidatorFactory[] { attributeValidatorFactory, validationAttributeValidatorFactory })
        {
        }
        
        protected internal override Validator InnerCreateValidator(Type targetType, string ruleset, ValidatorFactory mainValidatorFactory)
        {
            Validator validator = GetValidator(_validatorFactories.Select(f => f.InnerCreateValidator(targetType, ruleset, mainValidatorFactory)));

            return validator;
        }

        public override void ResetCache()
        {
            _validatorFactories.ForEach(f => f.ResetCache());
            base.ResetCache();
        }

        private static Validator GetValidator(IEnumerable<Validator> validators)
        {
            var validValidators = validators.Where(CheckIfValidatorIsAppropiate);

            if (validValidators.Count() == 1)
            {
                return validValidators.First();
            }

            return new AndCompositeValidator(validValidators.ToArray());
        }

        private static bool CheckIfValidatorIsAppropiate(Validator validator)
        {
            if (IsComposite(validator))
            {
                return CompositeHasValidators(validator);
            }
            return true;
        }

        private static bool IsComposite(Validator validator)
        {
            return validator is AndCompositeValidator || validator is OrCompositeValidator;
        }

        private static bool CompositeHasValidators(Validator validator)
        {
            var andValidator = validator as AndCompositeValidator;

            if (andValidator != null)
            {
                return ((Validator[])andValidator.Validators).Length > 0;
            }

            var orValidator = validator as OrCompositeValidator;

            if (orValidator != null)
            {
                return ((Validator[])orValidator.Validators).Length > 0;
            }

            return false;
        }
    }
}
