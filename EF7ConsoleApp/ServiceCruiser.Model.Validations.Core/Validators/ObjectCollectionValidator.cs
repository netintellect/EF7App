using System;
using System.Collections;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    public class ObjectCollectionValidator : Validator
    {
        private readonly Type _targetType;
        private readonly string _targetRuleset;
        private readonly Validator _targetTypeValidator;
        private readonly ValidatorFactory _validatorFactory;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectCollectionValidator"/>.</para>
        /// </summary>
        /// <remarks>
        /// The default ruleset will be used for validation.
        /// </remarks>
        public ObjectCollectionValidator()
            : this(ValidationFactory.DefaultCompositeValidatorFactory)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectCollectionValidator"/> 
        /// using the supplied ruleset.</para>
        /// </summary>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        public ObjectCollectionValidator(ValidatorFactory validatorFactory)
            : this(validatorFactory, string.Empty)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectCollectionValidator"/> 
        /// using the supplied ruleset.</para>
        /// </summary>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <param name="targetRuleset">The ruleset to use.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="validatorFactory"/> is <see langword="null"/>.</exception>
        public ObjectCollectionValidator(ValidatorFactory validatorFactory, string targetRuleset)
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
        /// <para>Initializes a new instance of the <see cref="ObjectCollectionValidator"/> for a target type.</para>
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <remarks>
        /// The default ruleset for <paramref name="targetType"/> will be used.
        /// </remarks>
        /// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
        public ObjectCollectionValidator(Type targetType)
            : this(targetType, string.Empty)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectCollectionValidator"/> for a target type
        /// using the supplied ruleset.</para>
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="targetRuleset">The ruleset to use.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
        public ObjectCollectionValidator(Type targetType, string targetRuleset)
            : this(targetType, ValidationFactory.DefaultCompositeValidatorFactory, targetRuleset)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectCollectionValidator"/> for a target type
        /// using the supplied ruleset.</para>
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <param name="targetRuleset">The ruleset to use.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when <paramref name="validatorFactory"/> is <see langword="null"/>.</exception>
        public ObjectCollectionValidator(Type targetType, ValidatorFactory validatorFactory, string targetRuleset)
            : base(null, null)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (targetRuleset == null)
            {
                throw new ArgumentNullException("targetRuleset");
            }
            if (validatorFactory == null)
            {
                throw new ArgumentNullException("validatorFactory");
            }

            _targetType = targetType;
            _targetTypeValidator = validatorFactory.CreateValidator(targetType, targetRuleset);
            _targetRuleset = targetRuleset;
            _validatorFactory = null;
        }

        /// <summary>
        /// Validates by applying the validation rules for the target type specified for the receiver to the elements
        /// in <paramref name="objectToValidate"/>.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
        /// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
        /// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
        /// <remarks>
        /// If <paramref name="objectToValidate"/> is <see langword="null"/> validation is ignored.
        /// <para/>
        /// A reference to a non collection object causes a validation failure and the validation rules
        /// for the configured target type will not be applied.
        /// <para/>
        /// Elements in the collection of a type not compatible with the configured target type causes a validation failure but
        /// do not affect validation on other elements.
        /// </remarks>
        public override void DoValidate(object objectToValidate,
            object currentTarget,
            string key,
            ValidationResults validationResults)
        {
            if (objectToValidate != null)
            {
                IEnumerable enumerable = objectToValidate as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object element in enumerable)
                    {
                        if (element != null)
                        {
                            var elementType = element.GetType();

                            if (_targetType == null || _targetType.IsAssignableFrom(elementType))
                            {
                                // reset the current target and the key
                                GetValidator(elementType).DoValidate(element, element, null, validationResults);
                            }
                            else
                            {
                                // unlikely
                                LogValidationResult(validationResults,
                                    Translations.ObjectCollectionValidatorIncompatibleElementInTargetCollection,
                                    element,
                                    null);
                            }
                        }
                    }
                }
                else
                {
                    LogValidationResult(validationResults, Translations.ObjectCollectionValidatorTargetNotCollection, currentTarget, key);
                }
            }
        }

        private Validator GetValidator(Type elementType)
        {
            if (_targetTypeValidator != null)
            {
                return _targetTypeValidator;
            }
            else
            {
                return _validatorFactory.CreateValidator(elementType, _targetRuleset);
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
        /// Type of target being validated.
        /// </summary>
        public Type TargetType
        {
            get { return _targetType; }
        }

        /// <summary>
        /// Ruleset to use when creating target validators.
        /// </summary>
        public string TargetRuleset
        {
            get { return _targetRuleset; }
        }
    }
}
