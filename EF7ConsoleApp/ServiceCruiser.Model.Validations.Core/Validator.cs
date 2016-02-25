using System;
using System.Collections.Generic;
using System.Globalization;

namespace ServiceCruiser.Model.Validations.Core
{
    public abstract class Validator
    {
        private string _messageTemplate;
        private string _tag;
        
        protected Validator(string messageTemplate, string tag)
        {
            _messageTemplate = messageTemplate;
            _tag = tag;
        }

        public ValidationResults Validate(object target)
        {
            var validationResults = new ValidationResults();

            DoValidate(target, target, null, validationResults);

            return validationResults;
        }

        public void Validate(object target, ValidationResults validationResults)
        {
            if (null == validationResults)
                throw new ArgumentNullException("validationResults");

            DoValidate(target, target, null, validationResults);
        }

        public abstract void DoValidate(object objectToValidate, object currentTarget, string key,
                                        ValidationResults validationResults);

        
        protected void LogValidationResult(ValidationResults validationResults, string message, object target, string key)
        {
            if (validationResults == null) throw new ArgumentNullException("validationResults");

            validationResults.AddResult(new ValidationResult(message, target, key, Tag, this));
        }

        protected void LogValidationResult(ValidationResults validationResults, string message, object target, string key,
                                           IEnumerable<ValidationResult> nestedValidationResults)
        {
            if (validationResults == null) throw new ArgumentNullException("validationResults");

            validationResults.AddResult(new ValidationResult(message, target, key, this.Tag, this, nestedValidationResults));
        }

        protected internal virtual string GetMessage(object objectToValidate, string key)
        {
            return string.Format(CultureInfo.CurrentCulture, MessageTemplate, objectToValidate,
                                 key, Tag);
        }


        protected abstract string DefaultMessageTemplate { get; }

        public string MessageTemplate
        {
            get { return _messageTemplate ?? DefaultMessageTemplate; }
            set { _messageTemplate = value; }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
    }
}
