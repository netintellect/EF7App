using System;

namespace ServiceCruiser.Model.Validations.Core.Instrumentation
{
    ///<summary>
    /// Describes the logical notifications raised by various validation classes to which providers will respond.
    ///</summary>
    public interface IValidationInstrumentationProvider
    {
        ///<summary>
        /// Notifies provider that a validation has succeeded.
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        void NotifyValidationSucceeded(Type typeBeingValidated);

        ///<summary>
        /// Notifies provider that validation has failed.
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        ///<param name="validationResult"></param>
        void NotifyValidationFailed(Type typeBeingValidated, ValidationResults validationResult);
        
        ///<summary>
        /// Notifies provider that a configuration based validation has been called.
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        void NotifyConfigurationCalled(Type typeBeingValidated);

        ///<summary>
        /// Notifies provider of a validation exception.
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        ///<param name="errorMessage"></param>
        ///<param name="exception"></param>
        void NotifyValidationException(Type typeBeingValidated, string errorMessage, Exception exception);
    }
}
