namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Validator that succeeds on null values and delegates validation of non-null values to another validator.
    /// </summary>
    public class NullIgnoringValidatorWrapper : Validator
    {
        private readonly Validator _wrappedValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullIgnoringValidatorWrapper"/> with a validator to wrap.
        /// </summary>
        /// <param name="wrappedValidator">The validator to wrap.</param>
        public NullIgnoringValidatorWrapper(Validator wrappedValidator) : base(string.Empty, string.Empty)
        {
            _wrappedValidator = wrappedValidator;
        }

        /// <summary>
        /// Implements the validation logic for the receiver.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
        /// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
        /// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate != null)
            {
                _wrappedValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);
            }
        }

        /// <summary>
        /// Gets the message template to use when logging results no message is supplied.
        /// </summary>
        /// <remarks>
        /// Not used for this validator.
        /// </remarks>
        protected override string DefaultMessageTemplate
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Test-only property. Gets the wrapped validator.
        /// </summary>
        public Validator WrappedValidator
        {
            get { return this._wrappedValidator; }
        }
    }
}
