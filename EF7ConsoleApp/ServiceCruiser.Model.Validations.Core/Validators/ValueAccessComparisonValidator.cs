using System;
using System.Globalization;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Performs validation by comparing the provided value with another value extracted from the validation target.
    /// </summary>
    public class ValueAccessComparisonValidator : ValueValidator
    {
        private readonly ValueAccess _valueAccess;
        private readonly ComparisonOperator _comparisonOperator;

        public ValueAccessComparisonValidator(ValueAccess valueAccess, 
                                              ComparisonOperator comparisonOperator) : this(valueAccess, comparisonOperator, null, null)
        { }

        public ValueAccessComparisonValidator(ValueAccess valueAccess,
                                              ComparisonOperator comparisonOperator,
                                              string messageTemplate,
                                              string tag) : this(valueAccess, comparisonOperator, messageTemplate, tag, false)
        {
            _valueAccess = valueAccess;
            _comparisonOperator = comparisonOperator;
        }

        public ValueAccessComparisonValidator(ValueAccess valueAccess,
                                              ComparisonOperator comparisonOperator,
                                              string messageTemplate,
                                              bool negated) : this(valueAccess, comparisonOperator, messageTemplate, null, negated)
        {
            _valueAccess = valueAccess;
            _comparisonOperator = comparisonOperator;
        }

        public ValueAccessComparisonValidator(ValueAccess valueAccess,
                                            ComparisonOperator comparisonOperator,
                                            string messageTemplate,
                                            string tag,
                                            bool negated) : base(messageTemplate, tag, negated)
        {
            if (valueAccess == null)
            {
                throw new ArgumentNullException("valueAccess");
            }

            _valueAccess = valueAccess;
            _comparisonOperator = comparisonOperator;
        }

        public override void DoValidate(object objectToValidate,
                                        object currentTarget,
                                        string key,
                                        ValidationResults validationResults)
        {
            object comparand;
            string valueAccessFailureMessage;
            bool status = _valueAccess.GetValue(currentTarget, out comparand, out valueAccessFailureMessage);

            if (!status)
            {
                LogValidationResult(validationResults, string.Format(CultureInfo.CurrentCulture, Translations.ValueAccessComparisonValidatorFailureToRetrieveComparand, _valueAccess.Key, valueAccessFailureMessage),
                                    currentTarget, key);
                return;
            }

            bool valid = false;

            if (_comparisonOperator == ComparisonOperator.Equal || _comparisonOperator == ComparisonOperator.NotEqual)
            {
                valid = (objectToValidate != null ? objectToValidate.Equals(comparand) : comparand == null)
                    ^ (_comparisonOperator == ComparisonOperator.NotEqual)
                    ^ Negated;
            }
            else
            {
                var comparableObjectToValidate = objectToValidate as IComparable;
                if (comparableObjectToValidate != null
                    && comparand != null
                    && comparableObjectToValidate.GetType() == comparand.GetType())
                {
                    int comparison = comparableObjectToValidate.CompareTo(comparand);

                    switch (_comparisonOperator)
                    {
                        case ComparisonOperator.GreaterThan:
                            valid = comparison > 0;
                            break;
                        case ComparisonOperator.GreaterThanEqual:
                            valid = comparison >= 0;
                            break;
                        case ComparisonOperator.LessThan:
                            valid = comparison < 0;
                            break;
                        case ComparisonOperator.LessThanEqual:
                            valid = comparison <= 0;
                            break;
                    }

                    valid = valid ^ Negated;
                }
            }

            if (!valid)
            {
                LogValidationResult(validationResults, string.Format(CultureInfo.CurrentCulture, MessageTemplate, objectToValidate, key, Tag, comparand, _valueAccess.Key, _comparisonOperator),
                                    currentTarget, key);
            }
        }
        
        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return Translations.ValueAccessComparisonValidatorNonNegatedDefaultMessageTemplate; }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get { return Translations.ValueAccessComparisonValidatorNegatedDefaultMessageTemplate; }
        }

        public ValueAccess ValueAccess
        {
            get { return _valueAccess; }
        }
        
        public ComparisonOperator ComparisonOperator
        {
            get { return _comparisonOperator; }
        }
    }
}
