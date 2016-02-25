using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
	/// <summary>
	/// Describes a <see cref="StringLengthValidator"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
		            AllowMultiple = true, Inherited = false)]
	public sealed class StringLengthValidatorAttribute : ValueValidatorAttribute
	{
		private readonly int _lowerBound;
		private readonly RangeBoundaryType _lowerBoundType;
		private readonly int _upperBound;
		private readonly RangeBoundaryType _upperBoundType;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="StringLengthValidatorAttribute"/> class with an upper bound constraint.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound.</param>
		public StringLengthValidatorAttribute(int upperBound)
			: this(0, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="StringLengthValidatorAttribute"/> class with lower and 
		/// upper bound constraints.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		public StringLengthValidatorAttribute(int lowerBound, int upperBound)
			: this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="StringLengthValidatorAttribute"/> class with fully specified
		/// bound constraints.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <seealso cref="RangeBoundaryType"/>
		public StringLengthValidatorAttribute(int lowerBound,
			RangeBoundaryType lowerBoundType,
			int upperBound,
			RangeBoundaryType upperBoundType)
		{
			_lowerBound = lowerBound;
			_lowerBoundType = lowerBoundType;
			_upperBound = upperBound;
			_upperBoundType = upperBoundType;
		}

        /// <summary>
        /// The lower bound.
        /// </summary>
	    public int LowerBound
	    {
	        get { return _lowerBound; }
	    }

        /// <summary>
        /// The indication of how to perform the lower bound check.
        /// </summary>
	    public RangeBoundaryType LowerBoundType
	    {
	        get { return _lowerBoundType; }
	    }

        /// <summary>
        /// The upper bound.
        /// </summary>
	    public int UpperBound
	    {
	        get { return _upperBound; }
	    }

        /// <summary>
        /// The indication of how to perform the upper bound check.
        /// </summary>
	    public RangeBoundaryType UpperBoundType
	    {
	        get { return _upperBoundType; }
	    }

	    /// <summary>
		/// Creates the <see cref="StringLengthValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="Validator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new StringLengthValidator(LowerBound,
				LowerBoundType,
				UpperBound,
				UpperBoundType,
				Negated);
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
