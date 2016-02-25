using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeValueWrapSelect : AttributeValueWrap
    {
        #region state
        [JsonProperty, IgnoreOnMap]
        public string Value
        {
            get { return RawValue == null ? null : (string)RawValue; }
            set { RawValue = value; }
        }
        #endregion

        #region behavior

        public static AttributeValueWrapSelect Create(AttributeSpecSelect specification)
        {
            return new AttributeValueWrapSelect { Code = specification.Code, RawValue = specification.DefaultValue };
        }
        public static AttributeValueWrapSelect Create(IAttributeContainer container)
        {
            if (container == null) return null;

            var attributeValueWrapSelect = new AttributeValueWrapSelect();
            var containerType = container.GetType();

            if (containerType == typeof(Task))
                attributeValueWrapSelect.TaskId = container.Id;
            if (containerType == typeof(WorkOrder))
                attributeValueWrapSelect.WorkOrderId = container.Id;
            if (containerType == typeof(ServiceOrder))
                attributeValueWrapSelect.ServiceOrderId = container.Id;

            return attributeValueWrapSelect;
        }
        #endregion
    }
}
