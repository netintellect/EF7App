using System;
using ServiceCruiser.Model.Validations.Core.Instrumentation;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    public sealed class GenericValidatorWrapper<T> : Validator<T>
    {
        private readonly IValidationInstrumentationProvider _instrumentationProvider;

        public GenericValidatorWrapper(Validator wrappedValidator, 
                                       IValidationInstrumentationProvider instrumentationProvider) : base(null, null)
        {
            WrappedValidator = wrappedValidator;
            _instrumentationProvider = instrumentationProvider;
        }

        protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            Type typeBeingValidated = typeof(T);

            _instrumentationProvider.NotifyConfigurationCalled(typeBeingValidated);

            try
            {
                WrappedValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);

                if (validationResults.IsValid)
                {
                    _instrumentationProvider.NotifyValidationSucceeded(typeBeingValidated);
                }
                else
                {
                    _instrumentationProvider.NotifyValidationFailed(typeBeingValidated, validationResults);
                }
            }
            catch (Exception ex)
            {
                _instrumentationProvider.NotifyValidationException(typeBeingValidated, ex.Message, ex);
                throw;
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return null; }
        }

        ///<summary>
        /// Returns the validator wrapped by <see cref="GenericValidatorWrapper{T}"/>
        ///</summary>
        public Validator WrappedValidator { get; private set; }

    }
}
