using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeSpecText : AttributeSpec
    {
        #region state
        private bool? _isMultiLine;
        [JsonProperty]
        public bool? IsMultiLine
        {
            get { return _isMultiLine; }
            set { SetProperty(value, ref _isMultiLine, () => IsMultiLine); }
        }
        private string _regEx;
        [JsonProperty]
        public string RegEx
        {
            get { return _regEx; }
            set { SetProperty(value, ref _regEx, () => RegEx); }
        }

        #endregion

        #region behavior
        public AttributeSpecText()
        {
            _valueWrap = AttributeValueWrapText.Create(this);
        }

        public override void ValidateValueWrap(ValidationResults results)
        {
            var textValueWrap = (AttributeValueWrapText) ValueWrap;
            if (textValueWrap != null)
            {
                if (RequiresInput && string.IsNullOrEmpty(textValueWrap.Value))
                {
                    var validationResult = new ValidationResult(string.Format("{0} Requires input", Label), this, ErrorField, null, EntityValidator);
                    results.AddResult(validationResult);
                    SetValidationError(validationResult);
                }
                if (!string.IsNullOrEmpty(RegEx))
                {
                    var regEx = new Regex(RegEx);
                    var match = regEx.Match(textValueWrap.Value);
                    if (!match.Success)
                    {
                        var validationResult = new ValidationResult(string.Format("{0} is not in the correct format", Label), this, ErrorField, null, EntityValidator);
                        results.AddResult(validationResult);
                        SetValidationError(validationResult);
                    }
                }
            }
        }
        #endregion
    }
}
