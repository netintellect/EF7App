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
    public sealed class DateTimeOffsetRangeValidatorAttribute : ValueValidatorAttribute
    {
        private readonly object _lowerBound;
        private readonly DateTimeOffset _effectiveLowerBound;
        private readonly RangeBoundaryType _lowerBoundType;
        private readonly object _upperBound;
        private readonly DateTimeOffset _effectiveUpperBound;
        private readonly RangeBoundaryType _upperBoundType;
        
        public DateTimeOffsetRangeValidatorAttribute(string upperBound)
            : this(null, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
        { }

        public DateTimeOffsetRangeValidatorAttribute(DateTimeOffset upperBound)
            : this(default(DateTimeOffset), RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
        { }

        public DateTimeOffsetRangeValidatorAttribute(string lowerBound, string upperBound)
            : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
        { }

        public DateTimeOffsetRangeValidatorAttribute(DateTimeOffset lowerBound, DateTimeOffset upperBound)
            : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
        { }

        public DateTimeOffsetRangeValidatorAttribute(string lowerBound, RangeBoundaryType lowerBoundType,
                                               string upperBound, RangeBoundaryType upperBoundType)
            : this(lowerBound, ConvertToISO8601Date(lowerBound), lowerBoundType, upperBound, ConvertToISO8601Date(upperBound), upperBoundType)
        { }
        
        public DateTimeOffsetRangeValidatorAttribute(DateTimeOffset lowerBound, RangeBoundaryType lowerBoundType,
                                                     DateTimeOffset upperBound, RangeBoundaryType upperBoundType)
            : this(lowerBound, lowerBound, lowerBoundType, upperBound, upperBound, upperBoundType)
        { }

        private DateTimeOffsetRangeValidatorAttribute(object lowerBound, DateTimeOffset effectiveLowerBound, RangeBoundaryType lowerBoundType,
                                                      object upperBound, DateTimeOffset effectiveUpperBound, RangeBoundaryType upperBoundType)
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
            return new DateTimeOffsetRangeValidator(_effectiveLowerBound, _lowerBoundType,
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
