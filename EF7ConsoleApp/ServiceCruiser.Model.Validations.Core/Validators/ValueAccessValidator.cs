using System;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Performs validation on objects by validating a value extracted from them through an 
    /// instance of <see cref="ValueAccess"/> with a specified <see cref="Validator"/>.
    /// </summary>
    public class ValueAccessValidator : Validator
    {
        private readonly ValueAccess _valueAccess;
        private readonly Validator _valueValidator;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ValueAccessValidator"/> class with an upper bound constraint.</para>
        /// </summary>
        /// <param name="valueAccess">The <see cref="ValueAccess"/> to use when extracting values from the 
        /// validated objects.</param>
        /// <param name="valueValidator">The <see cref="Validator"/> to use when validating the values extracted
        /// from the validated objects.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="valueAccess"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="valueValidator"/> is <see langword="null"/>.</exception>
        public ValueAccessValidator(ValueAccess valueAccess, Validator valueValidator)
            : base(null, null)
        {
            if (null == valueAccess)
                throw new ArgumentNullException("valueAccess");
            if (null == valueValidator)
                throw new ArgumentNullException("valueValidator");

            _valueAccess = valueAccess;
            _valueValidator = valueValidator;
        }

        /// <summary>
        /// Validates extracting a value from <paramref name="objectToValidate"/> and applying a validator
        /// to it.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
        /// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
        /// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
        /// <remarks>
        /// <see langword="null"/> is considered a failed validation.
        /// </remarks>
        public override void DoValidate(object objectToValidate,
            object currentTarget,
            string key,
            ValidationResults validationResults)
        {
            object value;
            string valueAccessFailureMessage;
            bool retrievalSucceeded = _valueAccess.GetValue(objectToValidate, out value, out valueAccessFailureMessage);

            if (retrievalSucceeded)
            {
                // override the key and the current target for validations down the chain
                _valueValidator.DoValidate(value, objectToValidate, _valueAccess.Key, validationResults);
            }
            else
            {
                LogValidationResult(validationResults, valueAccessFailureMessage, currentTarget, _valueAccess.Key);
            }
        }

        /// <summary>
        /// Gets the message template to use when logging results no message is supplied.
        /// </summary>
        protected override string DefaultMessageTemplate
        {
            get { return Translations.ValueValidatorDefaultMessageTemplate; }
        }

        /// <summary>
        /// Key used to access the member being validated.
        /// </summary>
        public string Key
        {
            get { return _valueAccess.Key; }
        }
    }
}
