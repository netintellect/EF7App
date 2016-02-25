using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
	/// <summary>
	/// Describes a <see cref="NotNullValidator"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Parameter,
		            AllowMultiple = true, Inherited = false)]
	public sealed class NotNullValidatorAttribute : ValueValidatorAttribute
	{
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new NotNullValidator(Negated);
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
