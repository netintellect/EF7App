using System.Collections.Generic;

namespace ServiceCruiser.Model.Validations.Core
{
    public class ValidationResult
    {
        private readonly string _message;
        private readonly string _key;
        private readonly string _tag;
        private readonly object _target;
        private readonly Validator _validator;
        private readonly IEnumerable<ValidationResult> _nestedValidationResults;

        private static readonly IEnumerable<ValidationResult> NoNestedValidationResults = new ValidationResult[0];

        public ValidationResult(string message, object target, string key, 
                                string tag, Validator validator) : this(message, target, key, tag, validator, NoNestedValidationResults)
        { }

        /// <summary>
        /// Initializes this object with a message.
        /// </summary>
        public ValidationResult(string message, object target, string key, 
                                string tag, Validator validator,
                                IEnumerable<ValidationResult> nestedValidationResults)
        {
            _message = message;
            _key = key;
            _target = target;
            _tag = tag;
            _validator = validator;
            _nestedValidationResults = nestedValidationResults;
        }

        public string Key
        {
            get { return _key; }
        }

        public string Message
        {
            get { return _message; }
        }

        public string Tag
        {
            get { return _tag; }
        }

        public object Target
        {
            get { return _target; }
        }

        public Validator Validator
        {
            get { return _validator; }
        }

        public IEnumerable<ValidationResult> NestedValidationResults
        {
            get { return _nestedValidationResults; }
        }

        public override string ToString()
        {
            return _message;
        }
    }
}
