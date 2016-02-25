using System;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Internal implementation for range checking validators.
    /// </summary>
    /// <typeparam name="T">The type of value being checked for ranges.</typeparam>
    public class RangeChecker<T> where T : IComparable
    {
        private readonly T _lowerBound;
        private readonly RangeBoundaryType _lowerBoundType;
        private readonly T _upperBound;
        private readonly RangeBoundaryType _upperBoundType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lowerBound"></param>
        /// <param name="lowerBoundType"></param>
        /// <param name="upperBound"></param>
        /// <param name="upperBoundType"></param>
        public RangeChecker(T lowerBound, RangeBoundaryType lowerBoundType,
            T upperBound, RangeBoundaryType upperBoundType)
        {
            if (upperBound == null && upperBoundType != RangeBoundaryType.Ignore)
                throw new ArgumentException(Translations.ExceptionUpperBoundNull);
            if (lowerBound == null && lowerBoundType != RangeBoundaryType.Ignore)
                throw new ArgumentException(Translations.ExceptionLowerBoundNull);

            if (lowerBoundType != RangeBoundaryType.Ignore
                && upperBoundType != RangeBoundaryType.Ignore
                && upperBound.CompareTo(lowerBound) < 0)
                throw new ArgumentException(Translations.ExceptionUpperBoundLowerThanLowerBound);

            _lowerBound = lowerBound;
            _lowerBoundType = lowerBoundType;
            _upperBound = upperBound;
            _upperBoundType = upperBoundType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsInRange(T target)
        {
            if (_lowerBoundType > RangeBoundaryType.Ignore)
            {
                int lowerBoundComparison = _lowerBound.CompareTo(target);
                if (lowerBoundComparison > 0)
                    return false;
                if (_lowerBoundType == RangeBoundaryType.Exclusive && lowerBoundComparison == 0)
                    return false;
            }
            if (_upperBoundType > RangeBoundaryType.Ignore)
            {
                int upperBoundComparison = _upperBound.CompareTo(target);
                if (upperBoundComparison < 0)
                    return false;
                if (_upperBoundType == RangeBoundaryType.Exclusive && upperBoundComparison == 0)
                    return false;
            }

            return true;
        }

        #region test only properties

        /// <summary>
        /// 
        /// </summary>
        public T LowerBound
        {
            get { return _lowerBound; }
        }

        /// <summary>
        /// 
        /// </summary>
        public T UpperBound
        {
            get { return _upperBound; }
        }

        /// <summary>
        /// 
        /// </summary>
        public RangeBoundaryType LowerBoundType
        {
            get { return _lowerBoundType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public RangeBoundaryType UpperBoundType
        {
            get { return _upperBoundType; }
        }

        #endregion
    }
}
