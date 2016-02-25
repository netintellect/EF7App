using System;
using System.Globalization;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    partial class PropertyComparisonValidatorAttribute
    {
        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the specified validation object on which the 
        /// <see cref="System.ComponentModel.DataAnnotations.ValidationAttribute "/> is declared.</param>
        /// <returns><see langword="true"/> if the specified value is valid; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="NotSupportedException">when invoked on an attribute with a non-null ruleset.</exception>
        public bool IsValid(object value)
        {
            if (!string.IsNullOrEmpty(this.Ruleset))
            {
                return true;
            }

            throw new NotSupportedException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Translations.ExceptionValidationAttributeNotSupported,
                    this.GetType().Name));
        }
    }
}
