using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeValueWrapInt : AttributeValueWrap
    {
        #region state
        [JsonProperty, IgnoreOnMap]
        public int? Value
        {
            get
            {
                int extractedValue;
                if (int.TryParse(RawValue, out extractedValue))
                    return extractedValue;
                return null;
            }
            set { RawValue = value.ToString(); }
        }
        #endregion

        #region behavior

        public static AttributeValueWrapInt Create(AttributeSpecInt specification)
        {
            return new AttributeValueWrapInt { Code = specification.Code, RawValue = specification.DefaultValue };
        }
        public static AttributeValueWrapInt Create(IAttributeContainer container)
        {
            if (container == null) return null;

            var attributeValueWrapInt = new AttributeValueWrapInt();
            var containerType = container.GetType();

            if (containerType == typeof(Task))
                attributeValueWrapInt.TaskId = container.Id;
            if (containerType == typeof(WorkOrder))
                attributeValueWrapInt.WorkOrderId = container.Id;
            if (containerType == typeof(ServiceOrder))
                attributeValueWrapInt.ServiceOrderId = container.Id;

            return attributeValueWrapInt;
        }
        #endregion
    }
}
