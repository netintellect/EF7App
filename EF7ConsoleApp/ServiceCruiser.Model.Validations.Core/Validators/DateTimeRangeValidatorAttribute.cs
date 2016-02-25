using System;
using System.Globalization;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Attribute to specify date range validation on a property, method or field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
                    AllowMultiple = true, Inherited = false)]
    public sealed class DateTimeRangeValidatorAttribute : ValueValidatorAttribute
    {
        private readonly object _lowerBound;
        private readonly DateTime _effectiveLowerBound;
        private readonly RangeBoundaryType _lowerBoundType;
        private readonly object _upperBound;
        private readonly DateTime _effectiveUpperBound;
        private readonly RangeBoundaryType _upperBoundType;
        
        public DateTimeRangeValidatorAttribute(string upperBound)
            : this(null, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
        { }

        public DateTimeRangeValidatorAttribute(DateTime upperBound)
            : this(default(DateTime), RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
        { }

        public DateTimeRangeValidatorAttribute(string lowerBound, string upperBound)
            : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
        { }

        public DateTimeRangeValidatorAttribute(DateTime lowerBound, DateTime upperBound)
            : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
        { }

        public DateTimeRangeValidatorAttribute(string lowerBound, RangeBoundaryType lowerBoundType,
                                               string upperBound, RangeBoundaryType upperBoundType)
            : this(lowerBound, ConvertToISO8601Date(lowerBound), lowerBoundType, upperBound, ConvertToISO8601Date(upperBound), upperBoundType)
        { }
        
        public DateTimeRangeValidatorAttribute(DateTime lowerBound, RangeBoundaryType lowerBoundType,
                                               DateTime upperBound, RangeBoundaryType upperBoundType)
            : this(lowerBound, lowerBound, lowerBoundType, upperBound, upperBound, upperBoundType)
        { }

        private DateTimeRangeValidatorAttribute(object lowerBound, DateTime effectiveLowerBound, RangeBoundaryType lowerBoundType,
                                                object upperBound, DateTime effectiveUpperBound, RangeBoundaryType upperBoundType)
        {
            _lowerBound = lowerBound;
            _effectiveLowerBound = effectiveLowerBound;
            _lowerBoundType = lowerBoundType;
            _upperBound = upperBound;
            _effectiveUpperBound = effectiveUpperBound;
            _upperBoundType = upperBoundType;
        }

        public object LowerBound
        {
            get { return _lowerBound; }
        }

        public RangeBoundaryType LowerBoundType
        {
            get { return _lowerBoundType; }
        }

        public object UpperBound
        {
            get { return _upperBound; }
        }

        public RangeBoundaryType UpperBoundType
        {
            get { return _upperBoundType; }
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new DateTimeRangeValidator(_effectiveLowerBound, _lowerBoundType,
                                              _effectiveUpperBound, _upperBoundType,
                                              Negated);
        }

        private static DateTime ConvertToISO8601Date(string iso8601DateString)
        {
            if (string.IsNullOrEmpty(iso8601DateString))
            {
                return default(DateTime);
            }
            try
            {
                return DateTime.ParseExact(iso8601DateString, "s", CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                throw new ArgumentException(Translations.ExceptionInvalidDate, e);
            }
        }

        private readonly Guid typeId = Guid.NewGuid();

        /// <summary>
        /// Gets a unique identifier for this attribute.
        /// </summary>
        public object TypeId
        {
            get
            {
                return typeId;
            }
        }
    }
}
