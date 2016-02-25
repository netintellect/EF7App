using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    partial class PropertyComparisonValidatorAttribute
    {
        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="context">The context.</param>
        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext context)
        {
            return System.ComponentModel.DataAnnotations.ValidationResult.Success;
        }
    }
}
