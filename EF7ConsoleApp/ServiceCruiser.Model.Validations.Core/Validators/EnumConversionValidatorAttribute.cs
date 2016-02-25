using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
	/// <summary>
	/// Represents a <see cref="EnumConversionValidator"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
		            AllowMultiple = true, Inherited = false)]
	public sealed class EnumConversionValidatorAttribute : ValueValidatorAttribute
	{
		private readonly Type _enumType;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="EnumConversionValidatorAttribute"/> </para>
		/// </summary>
		/// <param name="enumType">The type of enum that should be validated</param>
		public EnumConversionValidatorAttribute(Type enumType)
		{
			ValidatorArgumentsValidatorHelper.ValidateEnumConversionValidator(enumType);

			_enumType = enumType;
		}

        /// <summary>
        /// The type of enum that should be validated. 
        /// </summary>
	    public Type EnumType
	    {
	        get { return _enumType; }
	    }

	    /// <summary>
		/// Creates the <see cref="EnumConversionValidator"/> described by the attribute object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This operation must be overridden by subclasses.</remarks>
		/// <returns>The created <see cref="EnumConversionValidator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new EnumConversionValidator(EnumType, Negated);
		}


        private readonly Guid typeId = Guid.NewGuid();

        /// <summary>
        /// Gets a unique identifier for this attribute.
        /// </summary>
        public object TypeId
        {
            get
            {
                return this.typeId;
            }
        }

    }
}
