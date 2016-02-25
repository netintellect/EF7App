using System;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Represents an <see cref="ObjectValidator"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
                    AllowMultiple = true, Inherited = false)]
    public sealed class ObjectValidatorAttribute : ValidatorAttribute
    {
        private readonly string _targetRuleset;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidatorAttribute"/> class.</para>
        /// </summary>
        public ObjectValidatorAttribute()
            : this(string.Empty)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidatorAttribute"/> class with
        /// a specific ruleset.</para>
        /// </summary>
        /// <param name="targetRuleset">The target ruleset.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
        /// <seealso cref="ObjectValidator(Type, string)"/>
        public ObjectValidatorAttribute(string targetRuleset)
        {
            if (targetRuleset == null)
            {
                throw new ArgumentNullException("targetRuleset");
            }

            this._targetRuleset = targetRuleset;
        }

        /// <summary>
        /// Creates the <see cref="Validator"/> described by the attribute object providing validator specific
        /// information.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This method must not be called on this class. Call 
        /// <see cref="ObjectValidatorAttribute.DoCreateValidator(Type, Type, MemberValueAccessBuilder, ValidatorFactory)"/>.</remarks>
        protected override Validator DoCreateValidator(Type targetType)
        {
            throw new NotImplementedException(Translations.ExceptionShouldNotCall);
        }

        /// <summary>
        /// Creates the <see cref="ObjectValidator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <param name="ownerType">The type of the object from which the value to validate is extracted.</param>
        /// <param name="memberValueAccessBuilder">The <see cref="MemberValueAccessBuilder"/> to use for validators that
        /// require access to properties.</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <returns>The created <see cref="Validator"/>.</returns>
        protected override Validator DoCreateValidator(
            Type targetType,
            Type ownerType,
            MemberValueAccessBuilder memberValueAccessBuilder,
            ValidatorFactory validatorFactory)
        {
            if (this.ValidateActualType)
            {
                return new ObjectValidator(validatorFactory, TargetRuleset);
            }
            else
            {
                return new ObjectValidator(targetType, validatorFactory, TargetRuleset);
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to validate based on the static type or the actual type.
        /// </summary>
        public bool ValidateActualType { get; set; }

        /// <summary>
        /// The target ruleset.
        /// </summary>
        public string TargetRuleset
        {
            get { return _targetRuleset; }
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
