using System;
using System.Globalization;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Validates a <see cref="DateTime"/> value by checking it belongs to a range relative to the current date.
   /// </summary>
    public class RelativeDateTimeValidator : ValueValidator<DateTime>
    {
        private readonly int _lowerBound;
        private readonly DateTimeUnit _lowerUnit;
        private readonly RangeBoundaryType _lowerBoundType;
        private readonly int _upperBound;
        private readonly DateTimeUnit _upperUnit;
        private readonly RangeBoundaryType _upperBoundType;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit)
            : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, false)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, bool negated)
            : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, negated)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="messageTemplate">The message template to use when logging results.</param>
        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, string messageTemplate)
            : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, messageTemplate)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="messageTemplate">The message template to use when logging results.</param>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, string messageTemplate, bool negated)
            : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, messageTemplate, negated)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
            : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, false)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, bool negated)
            : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, negated)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
        /// <param name="messageTemplate">The message template to use when logging results.</param>
        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate)
            : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, messageTemplate)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
        /// <param name="messageTemplate">The message template to use when logging results.</param>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate, bool negated)
            : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, messageTemplate, negated)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="lowerUnit">The lower bound unit of time.</param>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit)
            : this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive, false)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="lowerUnit">The lower bound unit of time.</param>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit, bool negated)
            : this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive, negated)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="lowerUnit">The lower bound unit of time.</param>
        /// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
            int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
            : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, false)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="lowerUnit">The lower bound unit of time.</param>
        /// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
            int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType,
            bool negated)
            : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, null, negated)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="messageTemplate">The message template to use when logging results.</param>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="lowerUnit">The lower bound unit of time.</param>
        /// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
            int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType,
            string messageTemplate)
            : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, messageTemplate, false)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
        /// </summary>
        /// <param name="negated">True if the validator must negate the result of the validation.</param>
        /// <param name="messageTemplate">The message template to use when logging results.</param>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="lowerUnit">The lower bound unit of time.</param>
        /// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
        /// <param name="upperBound">The upper bound</param>
        /// <param name="upperUnit">The upper bound unit of time.</param>
        /// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
            int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType,
            string messageTemplate, bool negated)
            : base(messageTemplate, null, negated)
        {
            ValidatorArgumentsValidatorHelper.ValidateRelativeDatimeValidator(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType);

            _lowerBound = lowerBound;
            _lowerUnit = lowerUnit;
            _lowerBoundType = lowerBoundType;
            _upperBound = upperBound;
            _upperUnit = upperUnit;
            _upperBoundType = upperBoundType;

            BuildRangeChecker();    // force validation of parameters
        }

        /// <summary>
        /// Implements the validation logic for the receiver.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
        /// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
        /// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
        /// <remarks>
        /// The implementation for this method will perform type checking and conversation before forwarding the 
        /// validation request to method <see cref="Validator{T}.DoValidate(T, object, string, ValidationResults)"/>.
        /// </remarks>
        /// <see cref="Validator.DoValidate"/>
        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate == null)
            {
                LogValidationResult(validationResults,
                    GetMessage(objectToValidate, key),
                    currentTarget,
                    key);
            }
            else
            {
                base.DoValidate(objectToValidate, currentTarget, key, validationResults);
            }
        }

        /// <summary>
        /// Implements the validation logic for the receiver.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
        /// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
        /// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
        protected override void DoValidate(DateTime objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (BuildRangeChecker().IsInRange(objectToValidate) == Negated)
            {
                LogValidationResult(validationResults,
                    GetMessage(objectToValidate, key),
                    currentTarget,
                    key);
            }
        }

        private RangeChecker<DateTime> BuildRangeChecker()
        {
            var generator = new RelativeDateTimeGenerator();
            var nowDateTime = DateTime.Now;
            var lowerDateTime =
                _lowerBoundType != RangeBoundaryType.Ignore
                    ? generator.GenerateBoundDateTime(_lowerBound, _lowerUnit, nowDateTime)
                    : default(DateTime);
            var upperDateTime =
                _upperBoundType != RangeBoundaryType.Ignore
                    ? generator.GenerateBoundDateTime(_upperBound, _upperUnit, nowDateTime)
                    : default(DateTime);
            var rangeChecker = new RangeChecker<DateTime>(lowerDateTime, _lowerBoundType, upperDateTime, _upperBoundType);

            return rangeChecker;
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
                _lowerBound,
                LowerUnit,
                _upperBound,
                UpperUnit);
        }

        /// <summary>
        /// Gets the Default Message Template when the validator is non negated.
        /// </summary>
        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return Translations.RelativeDateTimeNonNegatedDefaultMessageTemplate; }
        }

        /// <summary>
        /// Gets the Default Message Template when the validator is negated.
        /// </summary>
        protected override string DefaultNegatedMessageTemplate
        {
            get { return Translations.RelativeDateTimeNegatedDefaultMessageTemplate; }
        }

        /// <summary>
        /// Lower bound for range comparison.
        /// </summary>
        public int LowerBound
        {
            get { return _lowerBound; }
        }

        /// <summary>
        /// Time units for lower bound.
        /// </summary>
        public DateTimeUnit LowerUnit
        {
            get { return _lowerUnit; }
        }

        /// <summary>
        /// Is lower bound included, excluded, or ignored?
        /// </summary>
        public RangeBoundaryType LowerBoundType
        {
            get { return _lowerBoundType; }
        }

        /// <summary>
        /// Upper bound for range comparison.
        /// </summary>
        public int UpperBound
        {
            get { return _upperBound; }
        }

        /// <summary>
        /// Time units for upper bound.
        /// </summary>
        public DateTimeUnit UpperUnit
        {
            get { return _upperUnit; }
        }

        /// <summary>
        /// Is upper bound included, excluded, or ignored?
        /// </summary>
        public RangeBoundaryType UpperBoundType
        {
            get { return _upperBoundType; }
        }
    }
}

