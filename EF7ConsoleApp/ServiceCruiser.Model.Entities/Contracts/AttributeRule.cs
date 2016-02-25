using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeRule : ValidatedEntity<AttributeRule>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private AttributeRuleType _ruleType;
        [JsonProperty]
        public AttributeRuleType RuleType
        {
            get { return _ruleType; }
            set { SetProperty(value, ref _ruleType, () => RuleType); }
        }
        private ObservableCollection<string> _operands = new ObservableCollection<string>();
        [JsonProperty]
        public ICollection<string> Operands
        {
            get { return _operands; }
            set { _operands = value != null ? value.ToObservableCollection() : null; }
        }

        [JsonProperty]
        public string OperandsSerialized
        {
            get { return JsonConvert.SerializeObject(Operands); }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                Operands = JsonConvert.DeserializeObject<ICollection<string>>(value);
            }
        }
        private ObservableCollection<string> _resultCodes = new ObservableCollection<string>();
        [JsonProperty]
        public ICollection<string> ResultCodes
        {
            get { return _resultCodes; }
            set { _resultCodes = value != null ? value.ToObservableCollection() : null; }
        }
        [JsonProperty]
        public string ResultCodesSerialized
        {
            get { return JsonConvert.SerializeObject(ResultCodes); }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                ResultCodes = JsonConvert.DeserializeObject<ICollection<string>>(value);
            }
        }
        private string _expression;
        [JsonProperty]
        public string Expression
        {
            get { return _expression; }
            set { SetProperty(value, ref _expression, () => Expression); }
        }

        private int _targetId;
        [JsonProperty]
        public int TargetId
        {
            get { return _targetId; }
            set { SetProperty(value, ref _targetId, () => TargetId); }
        }
        private AttributeGroup _target;
        [HandleOnNesting][Aggregation(isComposite: false)]
        [JsonProperty]
        public AttributeGroup Target
        {
            get { return _target; }
            set { SetProperty(value, ref _target, () => Target); }
        }

        private int _showAtId;
        [JsonProperty]
        public int ShowAtId
        {
            get { return _showAtId; }
            set { SetProperty(value, ref _showAtId, () => ShowAtId); }
        }
        private AttributeSpec _showAt;
        [JsonProperty]
        public AttributeSpec ShowAt
        {
            get { return _showAt; }
            set { SetProperty(value, ref _showAt, () => ShowAt); }
        }
        #endregion

        #region behavior

        private AttributeValueWrapBool FindValueWrap(string code, IAttributeContainer container, AttributeGroup rootGroup)
        {
            //First try to locate the ValueWrap in the container
            var valueWrap = container.Attributes.FirstOrDefault(a => a.Code == code) as AttributeValueWrapBool;
            if (valueWrap == null)
            {
                //Otherwise check the spec's assigend ValueWrap (through Initialize)
                var spec = rootGroup.FindAttributeSpec(code) as AttributeSpecBool;
                if (spec == null || spec.ValueWrap == null) throw new NotSupportedException(string.Format("Attribute rule contains operand {0} but has no Attribute with this code defined.", code));

                valueWrap = spec.ValueWrap as AttributeValueWrapBool;
            }
            return valueWrap;
        }

        public void ApplyRule(IAttributeContainer container, AttributeGroup rootGroup)
        {
            if (Target == null)
                throw new NotSupportedException("Attribute rule has no Target defined.");
            if (ShowAt == null)
                throw new NotSupportedException("Attribute rule has no ShowAt defined.");

            Target.IsVisible = true;
            switch (RuleType)
            {
                case AttributeRuleType.ShowOnAllTrueWithResultCodes:
                    Target.IsVisible = ValidateResultCode(container);
                    if (Target.IsVisible) goto case AttributeRuleType.ShowOnAllTrue;
                    break;
                case AttributeRuleType.ShowOnAllTrueWithoutResultCodes:
                    Target.IsVisible = string.IsNullOrEmpty(container.FixCode);
                    if (Target.IsVisible) goto case AttributeRuleType.ShowOnAllTrue;
                    break;
                case AttributeRuleType.ShowOnAllTrue:
                    foreach (var operand in Operands)
                    {
                        var attribute = FindValueWrap(operand, container, rootGroup);
                        if ((!attribute.Value.HasValue) || attribute.Value == false)
                        {                          
                            Target.IsVisible = false;
                            break;
                        }
                    }
                    break;
                case AttributeRuleType.ShowOnAllFalseWithResultCodes:
                    Target.IsVisible = string.IsNullOrEmpty(container.FixCode);
                    if (Target.IsVisible) goto case AttributeRuleType.ShowOnAllFalse;
                    break;
                case AttributeRuleType.ShowOnAllFalseWithoutResultCodes:
                    Target.IsVisible = ValidateResultCode(container);
                    if (Target.IsVisible) goto case AttributeRuleType.ShowOnAllFalse;
                    break;
                case AttributeRuleType.ShowOnAllFalse:
                    foreach (var operand in Operands)
                    {
                        var attribute = FindValueWrap(operand, container, rootGroup);
                        if (attribute.Value.HasValue && attribute.Value == true)
                        {
                            Target.IsVisible = false;
                            break;
                        }
                    }
                    break;
                case AttributeRuleType.ShowOnOneTrueWithResultCodes:
                    Target.IsVisible = ValidateResultCode(container);
                    if (Target.IsVisible) goto case AttributeRuleType.ShowOnOneTrue;
                    break;
                case AttributeRuleType.ShowOnOneTrueWithoutResultCodes:
                    Target.IsVisible = string.IsNullOrEmpty(container.FixCode);
                    if (Target.IsVisible) goto case AttributeRuleType.ShowOnOneTrue;
                    break;
                case AttributeRuleType.ShowOnOneTrue:
                    foreach (var operand in Operands)
                    {
                        var attribute = FindValueWrap(operand, container, rootGroup);
                        if (attribute.Value.HasValue && attribute.Value == true)
                            break;
                    }
                    Target.IsVisible = false;
                    break;
                case AttributeRuleType.ShowOnOneFalseWithResultCodes:
                    Target.IsVisible = ValidateResultCode(container);
                    if (Target.IsVisible) goto case AttributeRuleType.ShowOnOneFalse;
                    break;
                case AttributeRuleType.ShowOnOneFalseWithoutResultCodes:
                    Target.IsVisible = string.IsNullOrEmpty(container.FixCode);
                    if (Target.IsVisible) goto case AttributeRuleType.ShowOnOneFalse;
                    break;
                case AttributeRuleType.ShowOnOneFalse:
                    foreach (var operand in Operands)
                    {
                        var attribute = FindValueWrap(operand, container, rootGroup);
                        if (!attribute.Value.HasValue || attribute.Value == false)
                            break;
                    }
                    Target.IsVisible = false;
                    break;
                case AttributeRuleType.Expression:
                    Target.IsVisible = true; //TODO: implement Expression
                    break;
            }
            ShowAt.Refresh();
        }

        private bool ValidateResultCode(IAttributeContainer container)
        {
            if (ResultCodes == null || !ResultCodes.Any())
                throw new NotSupportedException("Attribute rule is of type that validates a resultcode, but the rule does not define any resultcodes");

            if (string.IsNullOrEmpty(container.FixCode))
                return false;

            return ResultCodes.Contains(container.FixCode);
        }

        #endregion
    }
}
