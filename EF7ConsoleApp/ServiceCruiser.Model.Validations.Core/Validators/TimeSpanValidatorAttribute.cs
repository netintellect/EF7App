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
    public sealed class TimeSpanRangeValidatorAttribute : ValueValidatorAttribute
    {
        private readonly object _lowerBound;
        private readonly TimeSpan _effectiveLowerBound;
        private readonly RangeBoundaryType _lowerBoundType;
        private readonly object _upperBound;
        private readonly TimeSpan _effectiveUpperBound;
        private readonly RangeBoundaryType _upperBoundType;
        
        public TimeSpanRangeValidatorAttribute(string upperBound)
            : this(null, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
        { }

        public TimeSpanRangeValidatorAttribute(TimeSpan upperBound)
            : this(default(TimeSpan), RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
        { }

        public TimeSpanRangeValidatorAttribute(string lowerBound, string upperBound)
            : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
        { }

        public TimeSpanRangeValidatorAttribute(TimeSpan lowerBound, TimeSpan upperBound)
            : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
        { }

        public TimeSpanRangeValidatorAttribute(string lowerBound, RangeBoundaryType lowerBoundType,
                                               string upperBound, RangeBoundaryType upperBoundType)
            : this(lowerBound, ConvertToTimeSpan(lowerBound), lowerBoundType, upperBound, ConvertToTimeSpan(upperBound), upperBoundType)
        { }
        
        public TimeSpanRangeValidatorAttribute(TimeSpan lowerBound, RangeBoundaryType lowerBoundType,
                                               TimeSpan upperBound, RangeBoundaryType upperBoundType)
            : this(lowerBound, lowerBound, lowerBoundType, upperBound, upperBound, upperBoundType)
        { }

        private TimeSpanRangeValidatorAttribute(object lowerBound, TimeSpan effectiveLowerBound, RangeBoundaryType lowerBoundType,
                                                object upperBound, TimeSpan effectiveUpperBound, RangeBoundaryType upperBoundType)
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
            return new TimeSpanRangeValidator(_effectiveLowerBound, _lowerBoundType,
                                              _effectiveUpperBound, _upperBoundType,
                                              Negated);
        }

        private static TimeSpan ConvertToTimeSpan(string timeString)
        {
            if (string.IsNullOrEmpty(timeString))
            {
                return default(TimeSpan);
            }
            try
            {
                return TimeSpan.ParseExact(timeString, "t", CultureInfo.InvariantCulture);
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
