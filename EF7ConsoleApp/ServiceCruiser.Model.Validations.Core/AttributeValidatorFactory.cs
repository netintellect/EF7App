using System;

using ServiceCruiser.Model.Validations.Core.Instrumentation;

namespace ServiceCruiser.Model.Validations.Core
{
    ///<summary>
    /// A CompositeValidatorFactory that produces validators based on reflection.
    ///</summary>
    public class AttributeValidatorFactory : ValidatorFactory
    {
        ///<summary>
        /// Initializes an AttributeValidatorFactory
        ///</summary>
        /// <param name="instrumentationProvider">The <see cref="IValidationInstrumentationProvider"/> 
        /// to provide to validators for instrumentation purposes.</param>
        public AttributeValidatorFactory(IValidationInstrumentationProvider instrumentationProvider)
            : base(instrumentationProvider)
        { }

        /// <summary>
        /// Creates the validator for the specified target and ruleset.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/>to validate.</param>
        /// <param name="ruleset">The ruleset to use when validating</param>
        /// <param name="mainValidatorFactory">Factory to use when building nested validators.</param>
        /// <returns>A <see cref="Validator"/></returns>
        protected internal override Validator InnerCreateValidator(
            Type targetType, 
            string ruleset, 
            ValidatorFactory mainValidatorFactory)
        {
            var builder =
                new MetadataValidatorBuilder(MemberAccessValidatorBuilderFactory.Default, mainValidatorFactory);

            return builder.CreateValidator(targetType, ruleset);
        }
    }
}
