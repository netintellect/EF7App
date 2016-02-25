using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeSpecDateTime : AttributeSpec
    {
        #region state
        private DateTimeOffset? _minDateTime;
        [JsonProperty]
        public DateTimeOffset? MinDateTime
        {
            get { return _minDateTime; }
            set { SetProperty(value, ref _minDateTime, () => MinDateTime); }
        }
        private DateTimeOffset? _maxDateTime;
        [JsonProperty]
        public DateTimeOffset? MaxDateTime
        {
            get { return _maxDateTime; }
            set { SetProperty(value, ref _maxDateTime, () => MaxDateTime); }
        }
        public DateTimeOffset DisplayMinDateTime { get { return MinDateTime ?? DateTimeOffset.MinValue; } }
        public DateTimeOffset DisplayMaxDateTime { get { return MaxDateTime ?? DateTimeOffset.MaxValue; } }
        #endregion

        #region behavior

        public AttributeSpecDateTime()
        {
            _valueWrap = AttributeValueWrapDateTime.Create(this);
        }
        public override void ValidateValueWrap(ValidationResults results)
        {
            var dateTimeValueWrap = (AttributeValueWrapDateTime) ValueWrap;
            if (dateTimeValueWrap != null)
            {
                if (RequiresInput && dateTimeValueWrap.Value == null)
                {
                    var validationResult = new ValidationResult(string.Format("{0} has no value", Label), this, ErrorField, null, EntityValidator);
                    results.AddResult(validationResult);
                    SetValidationError(validationResult);
                }
                if (dateTimeValueWrap.Value < DisplayMinDateTime || dateTimeValueWrap.Value > DisplayMaxDateTime)
                {
                    var validationResult = new ValidationResult(string.Format("{0} must be between {1} and {2}", Label, DisplayMinDateTime, DisplayMaxDateTime), this, ErrorField, null, EntityValidator);
                    results.AddResult(validationResult);
                    SetValidationError(validationResult);
                }
            }
        }
        #endregion
    }
}
