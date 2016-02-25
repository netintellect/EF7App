using System;
using System.Globalization;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core
{
    public abstract class Validator<T> : Validator
    {
        protected Validator(string messageTemplate, string tag) : base(messageTemplate, tag) { }
        
        public ValidationResults Validate(T target)
        {
            var validationResults = new ValidationResults();

            Validate(target, validationResults);

            return validationResults;
        }

        public void Validate(T target, ValidationResults validationResults)
        {
            if (null == validationResults)
                throw new ArgumentNullException("validationResults");

            DoValidate(target, target, null, validationResults);
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key,
                                        ValidationResults validationResults)
        {
            // null values need to be avoided when checking for type compliance for value types
            if (objectToValidate == null)
            {
                if (typeof(T).IsValueType)
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Translations.ExceptionValidatingNullOnValueType,
                                                   typeof(T).FullName);
                    LogValidationResult(validationResults, message, currentTarget, key);
                    return;
                }
            }
            else
            {
                if (!(objectToValidate is T))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Translations.ExceptionInvalidTargetType,
                                                   typeof(T).FullName, objectToValidate.GetType().FullName);
                                                   LogValidationResult(validationResults, message, currentTarget, key);
                    return;
                }
            }

            DoValidate((T)objectToValidate, currentTarget, key, validationResults);
        }
        
        protected abstract void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults);
    }
}
