using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeValueWrapText : AttributeValueWrap
    {
        #region state
        [JsonProperty, IgnoreOnMap]
        public string Value
        {
            get { return RawValue; }
            set { RawValue = value; }
        }
        #endregion

        #region behavior

        public static AttributeValueWrapText Create(AttributeSpecText specification)
        {
            return new AttributeValueWrapText { Code = specification.Code, RawValue = specification.DefaultValue };
        }

        public static AttributeValueWrapText Create(IAttributeContainer container)
        {
            if (container == null) return null;

            var attributeValueWrapText = new AttributeValueWrapText();
            var containerType = container.GetType();

            if (containerType == typeof(Task))
                attributeValueWrapText.TaskId = container.Id;
            if (containerType == typeof(WorkOrder))
                attributeValueWrapText.WorkOrderId = container.Id;
            if (containerType == typeof(ServiceOrder))
                attributeValueWrapText.ServiceOrderId = container.Id;

            return attributeValueWrapText;
        }
        #endregion
    }
}
