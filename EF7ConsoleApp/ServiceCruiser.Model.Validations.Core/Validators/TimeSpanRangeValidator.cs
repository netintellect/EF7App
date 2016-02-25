using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    public class TimeSpanRangeValidator : RangeValidator<TimeSpan>
    {
        public TimeSpanRangeValidator(TimeSpan upperBound) : base(upperBound)
        { }
        
        public TimeSpanRangeValidator(TimeSpan upperBound, bool negated) : base(upperBound, negated)
        { }
        
        public TimeSpanRangeValidator(TimeSpan lowerBound, TimeSpan upperBound) : base(lowerBound, upperBound)
        { }
        
        public TimeSpanRangeValidator(TimeSpan lowerBound, TimeSpan upperBound, bool negated)
            : base(lowerBound, upperBound, negated)
        { }
        
        public TimeSpanRangeValidator(TimeSpan lowerBound, RangeBoundaryType lowerBoundType,
            TimeSpan upperBound, RangeBoundaryType upperBoundType)
            : base(lowerBound, lowerBoundType, upperBound, upperBoundType)
        { }
        
        public TimeSpanRangeValidator(TimeSpan lowerBound, RangeBoundaryType lowerBoundType,
            TimeSpan upperBound, RangeBoundaryType upperBoundType, bool negated)
            : base(lowerBound, lowerBoundType, upperBound, upperBoundType, negated)
        { }
        
        public TimeSpanRangeValidator(TimeSpan lowerBound, RangeBoundaryType lowerBoundType,
            TimeSpan upperBound, RangeBoundaryType upperBoundType,
            string messageTemplate)
            : base(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate)
        { }
        
        public TimeSpanRangeValidator(TimeSpan lowerBound, RangeBoundaryType lowerBoundType,
            TimeSpan upperBound, RangeBoundaryType upperBoundType,
            string messageTemplate, bool negated)
            : base(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, negated)
        { }
    }
}
