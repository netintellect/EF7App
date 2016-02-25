using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeSpecSelect : AttributeSpec
    {
        #region state
        private bool? _isMultiSelectable;
        [JsonProperty]
        public bool? IsMultiSelectable
        {
            get { return _isMultiSelectable; }
            set { SetProperty(value, ref _isMultiSelectable, () => IsMultiSelectable); }
        }
        private string _elements;
        [JsonProperty]
        public string Elements //simicolum-separated list of selectable values
        {
            get { return _elements; }
            set { SetProperty(value, ref _elements, () => Elements); }
        }

        public ICollection<string> Items
        {
            get { return new Collection<string>(Elements.Split(';')); }
        }

        #endregion

        #region behavior
        public AttributeSpecSelect()
        {
            _valueWrap = AttributeValueWrapSelect.Create(this);
        }

        public override void ValidateValueWrap(ValidationResults results)
        {
            var selectValueWrap = (AttributeValueWrapSelect)ValueWrap;
            if (selectValueWrap != null)
            {
                if (RequiresInput && selectValueWrap.Value == null)
                {
                    var validationResult = new ValidationResult(string.Format("{0} has no value", Label), this, ErrorField, null, EntityValidator);
                    results.AddResult(validationResult);
                    SetValidationError(validationResult);
                }
                if (selectValueWrap.Value != null && !Items.Contains(selectValueWrap.Value))
                {
                    var validationResult = new ValidationResult(string.Format("{0} has an incorrect value", Label), this, ErrorField, null, EntityValidator);
                    results.AddResult(validationResult);
                    SetValidationError(validationResult);
                }
            }
        }
        #endregion
    }
}
