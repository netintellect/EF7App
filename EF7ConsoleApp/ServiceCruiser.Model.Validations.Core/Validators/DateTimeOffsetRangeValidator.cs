using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    public class DateTimeOffsetRangeValidator : RangeValidator<DateTimeOffset>
    {
        public DateTimeOffsetRangeValidator(DateTimeOffset upperBound) : base(upperBound)
        { }
        
        public DateTimeOffsetRangeValidator(DateTimeOffset upperBound, bool negated) : base(upperBound, negated)
        { }
        
        public DateTimeOffsetRangeValidator(DateTimeOffset lowerBound, DateTimeOffset upperBound) : base(lowerBound, upperBound)
        { }
        
        public DateTimeOffsetRangeValidator(DateTimeOffset lowerBound, DateTimeOffset upperBound, bool negated)
            : base(lowerBound, upperBound, negated)
        { }
        
        public DateTimeOffsetRangeValidator(DateTimeOffset lowerBound, RangeBoundaryType lowerBoundType,
                                            DateTimeOffset upperBound, RangeBoundaryType upperBoundType)
            : base(lowerBound, lowerBoundType, upperBound, upperBoundType)
        { }
        
        public DateTimeOffsetRangeValidator(DateTimeOffset lowerBound, RangeBoundaryType lowerBoundType,
                                            DateTimeOffset upperBound, RangeBoundaryType upperBoundType, bool negated)
            : base(lowerBound, lowerBoundType, upperBound, upperBoundType, negated)
        { }
        
        public DateTimeOffsetRangeValidator(DateTimeOffset lowerBound, RangeBoundaryType lowerBoundType,
                                            DateTimeOffset upperBound, RangeBoundaryType upperBoundType,
            string messageTemplate)
            : base(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate)
        { }
        
        public DateTimeOffsetRangeValidator(DateTimeOffset lowerBound, RangeBoundaryType lowerBoundType,
                                            DateTimeOffset upperBound, RangeBoundaryType upperBoundType,
            string messageTemplate, bool negated)
            : base(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, negated)
        { }
    }
}
