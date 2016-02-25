using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    [HasSelfValidation]
    public abstract class AttributeSpec : ValidatedEntity<AttributeSpec>
    {
        protected const string ErrorField = "Attributes";

        #region state
        private int _id;
        [JsonProperty, KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private string _code;
        [JsonProperty]
        public string Code
        {
            get { return _code; }
            set { SetProperty(value, ref _code, () => Code); }
        }
        private string _label;
        [JsonProperty]
        public string Label
        {
            get { return _label; }
            set { SetProperty(value, ref _label, () => Label); }
        }
        private string _description;
        [JsonProperty]
        public string Description
        {
            get { return _description; }
            set { SetProperty(value, ref _description, () => Description); }
        }
        private string _defaultValue;
        [JsonProperty]
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { SetProperty(value, ref _defaultValue, () => DefaultValue); }
        }
        private bool _requiresInput;
        [JsonProperty]
        public bool RequiresInput
        {
            get { return _requiresInput; }
            set { SetProperty(value, ref _requiresInput, () => RequiresInput); }
        }
        private bool _isAttributeReadOnly;
        [JsonProperty]
        public bool IsAttributeReadOnly
        {
            get { return _isAttributeReadOnly; }
            set { SetProperty(value, ref _isAttributeReadOnly, () => IsAttributeReadOnly); }
        }
        private bool _isHidden;
        [JsonProperty]
        public bool IsHidden
        {
            get { return _isHidden; }
            set { SetProperty(value, ref _isHidden, () => IsHidden); }
        }

        private int _groupId;
        [JsonProperty]
        public int GroupId
        {
            get { return _groupId; }
            set { SetProperty(value, ref _groupId, () => GroupId); }
        }
        private AttributeGroup _group;
        [JsonProperty]
        public AttributeGroup Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged(() => Group);
            }
        }

        private ObservableCollection<AttributeRule> _subGroupRules = new ObservableCollection<AttributeRule>();
        [HandleOnNesting][Aggregation(isComposite: true)][ObjectCollectionValidator(typeof(AttributeRule))]
        [JsonProperty]
        public ICollection<AttributeRule> SubGroupRules
        {
            get { return _subGroupRules; }
            set { _subGroupRules = value != null ? value.ToObservableCollection() : null; }
        }

        public ICollection<AttributeRule> VisibleSubGroupRules
        {
            get
            {
                return _subGroupRules != null ? _subGroupRules.Where(r => r.Target.IsVisible).ToObservableCollection() : null;
            }
        }

        public bool IsEditable
        {
            get { return !IsAttributeReadOnly; }
        }

        protected AttributeValueWrap _valueWrap;
        [HandleOnNesting][Aggregation(isComposite: false)]
        public AttributeValueWrap ValueWrap
        {
            get { return _valueWrap; }
            set { _valueWrap = value; }
        }

        #endregion

        #region behavior
        [SelfValidation]
        public abstract void ValidateValueWrap(ValidationResults results);

        public void Refresh()
        {
            OnPropertyChanged(() => VisibleSubGroupRules);
        }

        #endregion
    }
}
