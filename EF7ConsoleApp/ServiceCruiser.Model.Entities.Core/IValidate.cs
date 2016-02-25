using System.Collections.Generic;
using ServiceCruiser.Model.Validations.Core;

namespace ServiceCruiser.Model.Entities.Core
{
    public interface IValidate
    {
        void Invalidate();
        bool HasGraphValidationErrors();
        void AddValidationErrors(IEnumerable<ValidationResult> validationResults);
        void SetValidationError(ValidationResult validationResult);
        void ClearValidationErrors();
        void ClearValidationError(string propertyName);
    }
}
