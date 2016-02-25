using System.Collections.Generic;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Validates an object by checking if it belongs to a set.
    /// </summary>
    public class DomainValidator<T> : ValueValidator<T>
    {
        private readonly IEnumerable<T> _domain;
        
        public DomainValidator(IEnumerable<T> domain) : this(domain, false)
        { }

        
        public DomainValidator(bool negated, params T[] domain) : this(new List<T>(domain), null, negated)
        {
        }
        
        public DomainValidator(string messageTemplate, params T[] domain) : this(new List<T>(domain), messageTemplate, false)
        { }
        
        public DomainValidator(IEnumerable<T> domain, bool negated) : this(domain, null, negated)
        { }
        
        public DomainValidator(IEnumerable<T> domain, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
        {
            ValidatorArgumentsValidatorHelper.ValidateDomainValidator(domain);

            _domain = domain;
        }
        
        protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            bool logError = false;
            bool isObjectToValidateNull = objectToValidate == null;

            if (!isObjectToValidateNull)
            {
                logError = true;
                foreach (T element in _domain)
                {
                    if (element.Equals(objectToValidate))
                    {
                        logError = false;
                        break;
                    }
                }
            }

            if (isObjectToValidateNull || (logError != Negated))
            {
                LogValidationResult(validationResults, GetMessage(objectToValidate, key), currentTarget, key);
            }
        }
        
        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return Translations.DomainNonNegatedDefaultMessageTemplate; }
        }

        /// <summary>
        /// Gets the Default Message Template when the validator is negated.
        /// </summary>
        protected override string DefaultNegatedMessageTemplate
        {
            get { return Translations.DomainNegatedDefaultMessageTemplate; }
        }

        /// <summary>
        /// The set of items we're checking for membership in.
        /// </summary>
        public List<T> Domain
        {
            get { return new List<T>(_domain); }
        }
    }
}
