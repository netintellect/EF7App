using System;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Performs validation on objects by applying the validation rules specified for a supplied type.
    /// </summary>
    /// <seealso cref="ValidationFactory"/>
    public class ObjectValidator : Validator
    {
        private readonly Type _targetType;
        private readonly string _targetRuleset;
        private readonly Validator _targetTypeValidator;
        private readonly ValidatorFactory _validatorFactory;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidator"/> for a target type
        /// using the supplied ruleset.</para>
        /// </summary>
        public ObjectValidator()
            : this(ValidationFactory.DefaultCompositeValidatorFactory)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidator"/> for a target type
        /// using the supplied ruleset.</para>
        /// </summary>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="validatorFactory"/> is <see langword="null"/>.</exception>
        public ObjectValidator(ValidatorFactory validatorFactory)
            : this(validatorFactory, string.Empty)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidator"/> for a target type
        /// using the supplied ruleset.</para>
        /// </summary>
        /// <param name="targetRuleset">The ruleset to use.</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="validatorFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
        public ObjectValidator(ValidatorFactory validatorFactory, string targetRuleset)
            : base(null, null)
        {
            if (validatorFactory == null)
            {
                throw new ArgumentNullException("validatorFactory");
            }
            if (targetRuleset == null)
            {
                throw new ArgumentNullException("targetRuleset");
            }

            _targetType = null;
            _targetTypeValidator = null;
            _targetRuleset = targetRuleset;
            _validatorFactory = validatorFactory;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidator"/> for a target type.</para>
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <remarks>
        /// The default ruleset for <paramref name="targetType"/> will be used.
        /// </remarks>
        /// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
        public ObjectValidator(Type targetType)
            : this(targetType, string.Empty)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidator"/> for a target type
        /// using the supplied ruleset.</para>
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="targetRuleset">The ruleset to use.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
        public ObjectValidator(Type targetType, string targetRuleset)
            : this(targetType, ValidationFactory.DefaultCompositeValidatorFactory, targetRuleset)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidator"/> for a target type
        /// using the supplied ruleset.</para>
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="targetRuleset">The ruleset to use.</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="validatorFactory"/> is <see langword="null"/>.</exception>
        public ObjectValidator(Type targetType, ValidatorFactory validatorFactory, string targetRuleset)
            : base(null, null)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (validatorFactory == null)
            {
                throw new ArgumentNullException("validatorFactory");
            }
            if (targetRuleset == null)
            {
                throw new ArgumentNullException("targetRuleset");
            }

            _targetType = targetType;
            _targetTypeValidator = validatorFactory.CreateValidator(targetType, targetRuleset);
            _targetRuleset = targetRuleset;
            _validatorFactory = null;
        }

        /// <summary>
        /// Validates by applying the validation rules for the target type specified for the receiver.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
        /// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
        /// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
        /// <remarks>
        /// If <paramref name="objectToValidate"/> is <see langword="null"/> validation is ignored.
        /// <para/>
        /// A reference to an instance of a type not compatible with the configured target type
        /// causes a validation failure.
        /// </remarks>
        public override void DoValidate(object objectToValidate,
            object currentTarget,
            string key,
            ValidationResults validationResults)
        {
            if (objectToValidate != null)
            {
                Type objectToValidateType = objectToValidate.GetType();
                if (_targetType == null || _targetType.IsAssignableFrom(objectToValidateType))
                {
                    // reset the current target and the key
                    GetValidator(objectToValidateType).DoValidate(objectToValidate, objectToValidate, null, validationResults);
                }
                else
                {
                    // unlikely
                    LogValidationResult(validationResults, Translations.ObjectValidatorInvalidTargetType, currentTarget, key);
                }
            }
        }

        private Validator GetValidator(Type objectToValidateType)
        {
            if (_targetTypeValidator != null)
            {
                return _targetTypeValidator;
            }
            else
            {
                return _validatorFactory.CreateValidator(objectToValidateType, _targetRuleset);
            }
        }

        /// <summary>
        /// Gets the message template to use when logging results no message is supplied.
        /// </summary>
        protected override string DefaultMessageTemplate
        {
            get { return null; }
        }

        /// <summary>
        /// Target type being validated.
        /// </summary>
        public Type TargetType
        {
            get { return _targetType; }
        }

        /// <summary>
        /// Rule set to use when creating target validators.
        /// </summary>
        public string TargetRuleset
        {
            get { return _targetRuleset; }
        }
    }
}
