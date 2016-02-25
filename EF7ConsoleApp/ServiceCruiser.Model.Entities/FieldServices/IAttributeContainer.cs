using System.Collections.Generic;
using ServiceCruiser.Model.Validations.Core;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    public interface IAttributeContainer
    {
        int Id { get; set; }
        ICollection<AttributeValueWrap> Attributes { get; set; }
        int? AttributeSpecId { get; }
        string FixCode { get; }
        void SetValidationError(ValidationResult validationResult);
        void ClearValidationError(string propertyName);
    }
}
