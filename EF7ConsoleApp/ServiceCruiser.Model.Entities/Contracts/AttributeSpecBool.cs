using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeSpecBool : AttributeSpec
    {
        #region behavior
        public AttributeSpecBool()
        {
            _valueWrap = AttributeValueWrapBool.Create(this);
        }

        public override void ValidateValueWrap(ValidationResults results)
        {
            var boolValueWrap = (AttributeValueWrapBool)ValueWrap;
            if (boolValueWrap != null)
            {
                if (RequiresInput && boolValueWrap.Value == null)
                {
                    var validationResult = new ValidationResult(string.Format("{0} is not checked or unchecked", Label), this, ErrorField, null, EntityValidator);
                    results.AddResult(validationResult);
                    SetValidationError(validationResult);
                }
            }
        }
        #endregion
    }
}
