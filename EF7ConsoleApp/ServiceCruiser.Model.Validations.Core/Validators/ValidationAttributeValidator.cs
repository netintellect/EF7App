﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Validator wrapping a collection of <see cref="ValidationAttribute"/>.
    /// </summary>
    public class ValidationAttributeValidator : Validator
    {
        private readonly IEnumerable<ValidationAttribute> _validationAttributes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationAttributes"></param>
        public ValidationAttributeValidator(params ValidationAttribute[] validationAttributes)
            : this((IEnumerable<ValidationAttribute>)validationAttributes)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationAttributeValidator"/> with a 
        /// <see cref="ValidationAttribute"/>.
        /// </summary>
        /// <param name="validationAttributes">The validation attributes to wrap.</param>
        public ValidationAttributeValidator(IEnumerable<ValidationAttribute> validationAttributes)
            : base(null, null)
        {
            if (validationAttributes == null) throw new ArgumentNullException("validationAttributes");

            _validationAttributes =
                validationAttributes.Select(
                    va =>
                    {
                        if (va == null)
                        {
                            throw new ArgumentException(Translations.ExceptionContainsNullElements, "validationAttributes");
                        }
                        return va;
                    }).ToList();
        }

        /// <summary>
        /// Validates by forwarding the validation request to the wrapped <see cref="ValidationAttribute"/>.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
        /// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
        /// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            foreach (var validationAttribute in _validationAttributes)
            {
                var context = new ValidationContext(currentTarget, null) { MemberName = key };
                try
                {
                    var result = validationAttribute.GetValidationResult(objectToValidate, context);
                    if (result != null && !string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        LogValidationResult(validationResults, result.ErrorMessage, currentTarget, key);
                    }
                }
                catch (Exception e)
                {
                    string message =
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Translations.ValidationAttributeFailed,
                            validationAttribute.GetType().Name,
                            e.Message);
                    LogValidationResult(validationResults, message, currentTarget, key);
                }
            }
        }

        /// <summary>
        /// Gets the message template to use when logging results no message is supplied.
        /// </summary>
        /// <remarks>
        /// Not used by this validator.
        /// </remarks>
        protected override string DefaultMessageTemplate
        {
            get { return null; }
        }
    }
}
