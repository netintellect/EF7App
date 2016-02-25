using System;
using System.Globalization;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Validates a string by checking it represents a value for a given type.
    /// </summary>
    public class TypeConversionValidator : ValueValidator<string>
    {
        private readonly Type _targetType;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="TypeConversionValidator"/>.</para>
        /// </summary>
        /// <param name="targetType">The supplied type used to determine if the string can be converted to it.</param>
        public TypeConversionValidator(Type targetType) : this(targetType, false)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="TypeConversionValidator"/>.</para>
        /// </summary>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        /// <param name="targetType">The supplied type used to determine if the string can be converted to it.</param>
        public TypeConversionValidator(Type targetType, bool negated) : this(targetType, null, negated)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="TypeConversionValidator"/>.</para>
        /// </summary>
        /// <param name="messageTemplate">The message template to use when logging results.</param>
        /// <param name="targetType">The supplied type used to determine if the string can be converted to it.</param>
        public TypeConversionValidator(Type targetType, string messageTemplate)
            : this(targetType, messageTemplate, false)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="TypeConversionValidator"/>.</para>
        /// </summary>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        /// <param name="messageTemplate">The message template to use when logging results.</param>
        /// <param name="targetType">The supplied type used to determine if the string can be converted to it.</param>
        public TypeConversionValidator(Type targetType, string messageTemplate, bool negated)
            : base(messageTemplate, null, negated)
        {
            ValidatorArgumentsValidatorHelper.ValidateTypeConversionValidator(targetType);

            _targetType = targetType;
        }

        /// <summary>
        /// Implements the validation logic for the receiver.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
        /// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
        /// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            bool logError = false;
            bool isObjectToValidateNull = objectToValidate == null;

            if (!isObjectToValidateNull)
            {
                if (string.Empty.Equals(objectToValidate) && IsTheTargetTypeAValueTypeDifferentFromString())
                {
                    logError = true;
                }
                else
                {
                    try
                    {
                        object convertedValue = Convert.ChangeType(objectToValidate, _targetType, CultureInfo.CurrentCulture);
                        if (convertedValue == null)
                        {
                            logError = true;
                        }
                    }
                    catch (Exception)
                    {
                        logError = true;
                    }
                }
            }

            if (isObjectToValidateNull || (logError != Negated))
            {
                LogValidationResult(validationResults,
                    GetMessage(objectToValidate, key),
                    currentTarget,
                    key);
                return;
            }
        }

        private bool IsTheTargetTypeAValueTypeDifferentFromString()
        {
            TypeCode targetTypeCode = Type.GetTypeCode(_targetType);
            return targetTypeCode != TypeCode.Object && targetTypeCode != TypeCode.String;
        }


        /// <summary>
        /// Gets the message representing a failed validation.
        /// </summary>
        /// <param name="objectToValidate">The object for which validation was performed.</param>
        /// <param name="key">The key representing the value being validated for <paramref name="objectToValidate"/>.</param>
        /// <returns>The message representing the validation failure.</returns>
        protected internal override string GetMessage(object objectToValidate, string key)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                MessageTemplate,
                objectToValidate,
                key,
                Tag,
                _targetType.FullName);
        }

        /// <summary>
        /// Gets the Default Message Template when the validator is non negated.
        /// </summary>
        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return Translations.TypeConversionNonNegatedDefaultMessageTemplate; }
        }

        /// <summary>
        /// Gets the Default Message Template when the validator is negated.
        /// </summary>
        protected override string DefaultNegatedMessageTemplate
        {
            get { return Translations.TypeConversionNegatedDefaultMessageTemplate; }
        }

        /// <summary>
        /// Target type for conversion.
        /// </summary>
        public Type TargetType
        {
            get { return _targetType; }
        }
    }
}

