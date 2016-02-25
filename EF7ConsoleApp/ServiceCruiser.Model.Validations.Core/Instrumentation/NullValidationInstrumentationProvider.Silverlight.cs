﻿using System;

namespace ServiceCruiser.Model.Validations.Core.Instrumentation
{
    /// <summary>
    /// Implementation of <see cref="IValidationInstrumentationProvider"/> that performs no instrumentation actions.
    /// </summary>
    public class NullValidationInstrumentationProvider : IValidationInstrumentationProvider
    {
        /// <summary>
        /// Notifies provider that a validation has succeeded.
        /// </summary>
        public void NotifyValidationSucceeded(Type typeBeingValidated)
        {
        }

        /// <summary>
        /// Notifies provider that validation has failed.
        /// </summary>
        public void NotifyValidationFailed(Type typeBeingValidated, ValidationResults validationResult)
        {
        }

        /// <summary>
        /// Notifies provider that a configuration based validation has been called.
        /// </summary>
        public void NotifyConfigurationCalled(Type typeBeingValidated)
        {
        }

        /// <summary>
        /// Notifies provider of a validation exception.
        /// </summary>
        public void NotifyValidationException(Type typeBeingValidated, string errorMessage, Exception exception)
        {
        }

        /// <summary>
        /// Notifies provider that configuration for validation has failed.
        /// </summary>
        public void NotifyConfigurationFailure(Exception configurationException)
        {
        }
    }
}
