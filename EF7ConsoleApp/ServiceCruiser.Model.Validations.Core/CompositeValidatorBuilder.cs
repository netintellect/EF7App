using System.Collections.Generic;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public class CompositeValidatorBuilder
    {
        private readonly IValidatedElement _validatedElement;
        private readonly List<Validator> _valueValidators;
        private Validator _builtValidator;

        public CompositeValidatorBuilder(IValidatedElement validatedElement)
        {
            _validatedElement = validatedElement;
            _valueValidators = new List<Validator>();
        }

        public Validator GetValidator()
        {
            _builtValidator = DoGetValidator();

            return _builtValidator;
        }

        protected virtual Validator DoGetValidator()
        {
            // create the appropriate validator
            Validator validator;

            if (_valueValidators.Count == 1)
            {
                validator = _valueValidators[0];
            }
            else
            {

                if (CompositionType.And == _validatedElement.CompositionType)
                {
                    validator = new AndCompositeValidator(_valueValidators.ToArray());
                }
                else
                {
                    validator = new OrCompositeValidator(_valueValidators.ToArray());
                    validator.MessageTemplate = _validatedElement.CompositionMessageTemplate;
                    validator.Tag = _validatedElement.CompositionTag;
                }
            }

            // add support for ignoring nulls
            Validator valueValidator;
            if (_validatedElement.IgnoreNulls)
            {
                valueValidator = new NullIgnoringValidatorWrapper(validator);
            }
            else
            {
                valueValidator = validator;
            }

            return valueValidator;
        }

        /// <summary>
        /// Adds a value validator to the composite validator.
        /// </summary>
        /// <param name="valueValidator">The validator to add.</param>
        public void AddValueValidator(Validator valueValidator)
        {
            _valueValidators.Add(valueValidator);
        }

        #region test only properties

        /// <summary>
        /// This member supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public bool IgnoreNulls
        {
            get { return _validatedElement.IgnoreNulls; }
        }

        /// <summary>
        /// This member supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public CompositionType CompositionType
        {
            get { return _validatedElement.CompositionType; }
        }

        /// <summary>
        /// This member supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public Validator BuiltValidator
        {
            get { return _builtValidator; }
        }

        /// <summary>
        /// This member supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public IList<Validator> ValueValidators
        {
            get { return _valueValidators; }
        }

        #endregion
    }
}
