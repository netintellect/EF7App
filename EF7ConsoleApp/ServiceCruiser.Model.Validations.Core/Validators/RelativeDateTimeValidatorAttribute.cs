//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
	/// <summary>
	/// Represents a <see cref="RelativeDateTimeValidatorAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
		            AllowMultiple = true, Inherited = false)]
	public sealed class RelativeDateTimeValidatorAttribute : ValueValidatorAttribute
	{
		private readonly int _lowerBound;
		private readonly DateTimeUnit _lowerUnit;
		private readonly RangeBoundaryType _lowerBoundType;
		private readonly int _upperBound;
		private readonly DateTimeUnit _upperUnit;
		private readonly RangeBoundaryType _upperBoundType;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidatorAttribute"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		public RelativeDateTimeValidatorAttribute(int upperBound, DateTimeUnit upperUnit)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidatorAttribute"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		public RelativeDateTimeValidatorAttribute(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidatorAttribute"/>.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerUnit">The lower bound unit of time.</param>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		public RelativeDateTimeValidatorAttribute(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit)
			: this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidatorAttribute"/>.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerUnit">The lower bound unit of time.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		public RelativeDateTimeValidatorAttribute(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
			int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
		{
			ValidatorArgumentsValidatorHelper.ValidateRelativeDatimeValidator(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType);

			_lowerBound = lowerBound;
			_lowerUnit = lowerUnit;
			_lowerBoundType = lowerBoundType;
			_upperBound = upperBound;
			_upperUnit = upperUnit;
			_upperBoundType = upperBoundType;
		}

        /// <summary>
        /// The lower bound
        /// </summary>
	    public int LowerBound
	    {
	        get { return _lowerBound; }
	    }


        /// <summary>
        /// The lower bound unit of time.
        /// </summary>
	    public DateTimeUnit LowerUnit
	    {
	        get { return _lowerUnit; }
	    }

        /// <summary>
        /// The indication of how to perform the lower bound check.
        /// </summary>
	    public RangeBoundaryType LowerBoundType
	    {
	        get { return _lowerBoundType; }
	    }

        /// <summary>
        /// The upper bound
        /// </summary>
	    public int UpperBound
	    {
	        get { return _upperBound; }
	    }

        /// <summary>
        /// The upper bound unit of time.
        /// </summary>
	    public DateTimeUnit UpperUnit
	    {
	        get { return _upperUnit; }
	    }

        /// <summary>
        /// The indication of how to perform the upper bound check.
        /// </summary>
	    public RangeBoundaryType UpperBoundType
	    {
	        get { return _upperBoundType; }
	    }

	    /// <summary>
		/// Creates the <see cref="RelativeDateTimeValidator"/> described by the attribute object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This operation must be overridden by subclasses.</remarks>
		/// <returns>The created <see cref="RelativeDateTimeValidator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new RelativeDateTimeValidator(LowerBound, LowerUnit, LowerBoundType, UpperBound, UpperUnit, UpperBoundType, Negated);
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

