using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeValueWrapBool : AttributeValueWrap
    {
        #region state
        [JsonProperty, IgnoreOnMap]
        public bool? Value
        {
            get
            {
                bool extractedValue;
                if (bool.TryParse(RawValue, out extractedValue))
                    return extractedValue;
                return null;
            }
            set { RawValue = value.ToString(); }
        }
        #endregion

        #region behavior

        public static AttributeValueWrapBool Create(AttributeSpecBool specification)
        {
            return new AttributeValueWrapBool {Code = specification.Code, RawValue = specification.DefaultValue};
        }
        public static AttributeValueWrapBool Create(IAttributeContainer container)
        {
            if (container == null) return null;

            var attributeValueWrapBool = new AttributeValueWrapBool();
            var containerType = container.GetType();

            if (containerType == typeof(Task))
                attributeValueWrapBool.TaskId = container.Id;
            if (containerType == typeof(WorkOrder))
                attributeValueWrapBool.WorkOrderId = container.Id;
            if (containerType == typeof(ServiceOrder))
                attributeValueWrapBool.ServiceOrderId = container.Id;

            return attributeValueWrapBool;
        }
        #endregion
    }
}
