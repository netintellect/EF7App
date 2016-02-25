using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
	/// <summary>
	/// Represents a <see cref="TypeConversionValidator"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
		            AllowMultiple = true, Inherited = false)]
	public sealed class TypeConversionValidatorAttribute : ValueValidatorAttribute
	{
		private Type _targetType;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="TypeConversionValidatorAttribute"/>.</para>
		/// </summary>
		/// <param name="targetType">The supplied type used to determine if the string can be converted to it.</param>
		public TypeConversionValidatorAttribute(Type targetType)
		{
			ValidatorArgumentsValidatorHelper.ValidateTypeConversionValidator(targetType);

			_targetType = targetType;
		}

        /// <summary>
        /// The target type. 
        /// </summary>
	    public Type TargetType
	    {
	        get { return _targetType; }
	    }

	    /// <summary>
		/// Creates the <see cref="TypeConversionValidator"/> described by the attribute object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This operation must be overridden by subclasses.</remarks>
		/// <returns>The created <see cref="TypeConversionValidator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new TypeConversionValidator(TargetType, Negated);
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

