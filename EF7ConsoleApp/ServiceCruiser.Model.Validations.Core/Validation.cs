using System;

namespace ServiceCruiser.Model.Validations.Core
{
    public static class Validation
    {
        public static ValidationResults Validate<T>(T target)
        {
            return Validate<T>(target, ValidationSpecificationSource.All);
        }

        public static ValidationResults Validate<T>(T target, params string[] rulesets)
        {
            return Validate<T>(target, ValidationSpecificationSource.All, rulesets);
        }

        public static ValidationResults Validate<T>(T target, ValidationSpecificationSource source)
        {
            Type targetType = target != null ? target.GetType() : typeof(T);

            Validator validator = ValidationFactory.CreateValidator(targetType, source);

            return validator.Validate(target);
        }

        public static ValidationResults Validate<T>(T target, ValidationSpecificationSource source, params string[] rulesets)
        {
            if (rulesets == null)
            {
                throw new ArgumentNullException("rulesets");
            }

            Type targetType = target != null ? target.GetType() : typeof(T);
            var resultsReturned = new ValidationResults();
            foreach (string ruleset in rulesets)
            {
                Validator validator = ValidationFactory.CreateValidator(targetType, ruleset, source);
                foreach (ValidationResult validationResult in validator.Validate(target))
                {
                    resultsReturned.AddResult(validationResult);
                }
            }
            return resultsReturned;
        }
        
        public static ValidationResults ValidateFromAttributes<T>(T target)
        {
            Validator<T> validator = ValidationFactory.CreateValidatorFromAttributes<T>();

            return validator.Validate(target);
        }
        
        public static ValidationResults ValidateFromAttributes<T>(T target, params string[] rulesets)
        {
            if (rulesets == null)
            {
                throw new ArgumentNullException("rulesets");
            }

            var resultsReturned = new ValidationResults();
            foreach (string ruleset in rulesets)
            {
                Validator<T> validator = ValidationFactory.CreateValidatorFromAttributes<T>(ruleset);

                ValidationResults results = validator.Validate(target);

                foreach (ValidationResult validationResult in results)
                {
                    resultsReturned.AddResult(validationResult);
                }
            }
            return resultsReturned;
        }
    }
}
