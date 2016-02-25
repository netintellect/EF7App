using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeSpecInt : AttributeSpec
    {
        #region state
        private int? _minValue;
        [JsonProperty]
        public int? MinValue
        {
            get { return _minValue; }
            set { SetProperty(value, ref _minValue, () => MinValue); }
        }
        private int? _maxValue;
        [JsonProperty]
        public int? MaxValue
        {
            get { return _maxValue; }
            set { SetProperty(value, ref _maxValue, () => MaxValue); }
        }

        public int DisplayMinValue { get { return MinValue ?? int.MinValue; } }
        public int DisplayMaxValue { get { return MaxValue ?? int.MaxValue; } }

        #endregion

        #region behavior
        public AttributeSpecInt()
        {
            _valueWrap = AttributeValueWrapInt.Create(this);
        }

        public override void ValidateValueWrap(ValidationResults results)
        {
            var intValueWrap = (AttributeValueWrapInt)ValueWrap;
            if (intValueWrap != null)
            {
                if (RequiresInput && intValueWrap.Value == null)
                {
                    var validationResult = new ValidationResult(string.Format("{0} has no value", Label), this, ErrorField, null, EntityValidator);
                    results.AddResult(validationResult);
                    SetValidationError(validationResult);
                }
                if (intValueWrap.Value < DisplayMinValue || intValueWrap.Value > DisplayMaxValue)
                {
                    var validationResult = new ValidationResult(string.Format("{0} must be between {1} and {2}", Label, DisplayMinValue, DisplayMaxValue), this, ErrorField, null, EntityValidator);
                    results.AddResult(validationResult);
                    SetValidationError(validationResult);
                }
            }
        }
        #endregion
    }
}
