using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeValueWrapDateTime : AttributeValueWrap
    {
        #region state
        [JsonProperty, IgnoreOnMap]
        public DateTimeOffset? Value
        {
            get
            {
                DateTimeOffset extractedValue;
                if (DateTimeOffset.TryParse(RawValue, out extractedValue))
                    return extractedValue;
                return null;
            }
            set { RawValue = value.ToString(); }
        }
        #endregion

        #region behavior
        public static AttributeValueWrapDateTime Create(AttributeSpecDateTime specification)
        {
            return new AttributeValueWrapDateTime { Code = specification.Code, RawValue = specification.DefaultValue };
        }
        public static AttributeValueWrapDateTime Create(IAttributeContainer container)
        {
            if (container == null) return null;

            var attributeValueWrapDateTime = new AttributeValueWrapDateTime();
            var containerType = container.GetType();

            if (containerType == typeof(Task))
                attributeValueWrapDateTime.TaskId = container.Id;
            if (containerType == typeof(WorkOrder))
                attributeValueWrapDateTime.WorkOrderId = container.Id;
            if (containerType == typeof(ServiceOrder))
                attributeValueWrapDateTime.ServiceOrderId = container.Id;

            return attributeValueWrapDateTime;
            
        }
        #endregion
    }
}
