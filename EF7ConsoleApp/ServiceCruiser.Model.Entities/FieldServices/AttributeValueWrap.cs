using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Converters;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), JsonConverter(typeof(CustomJsonCreateConverter<AttributeValueWrap>))]
    public abstract class AttributeValueWrap : ValidatedEntity<AttributeValueWrap>
    {
        #region state

        private int _id;
        [JsonProperty]
        [Key(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }

        private int? _serviceOrderId;
        [JsonProperty]
        public int? ServiceOrderId
        {
            get { return _serviceOrderId; }
            set { SetProperty(value, ref _serviceOrderId, () => ServiceOrderId); }
        }

        private int? _workOrderId;
        [JsonProperty]
        public int? WorkOrderId
        {
            get { return _workOrderId; }
            set { SetProperty(value, ref _workOrderId, () => WorkOrderId); }
        }

        private int? _taskInfoId;
        [JsonProperty]
        public int? TaskInfoId
        {
            get { return _taskInfoId; }
            set { SetProperty(value, ref _taskInfoId, () => TaskInfoId); }
        }

        private int? _taskId;
        [JsonProperty]
        public int? TaskId
        {
            get { return _taskId; }
            set { SetProperty(value, ref _taskId, () => TaskId); }
        }

        private string _code;
        [JsonProperty]
        public string Code
        {
            get { return _code; }
            set { SetProperty(value, ref _code, () => Code); }
        }
        private string _rawValue;
        [JsonProperty]
        public string RawValue
        {
            get { return _rawValue; }
            set { SetProperty(value, ref _rawValue, () => RawValue); }
        }
        #endregion

        #region behavior
        public static AttributeValueWrap Create(AttributeSpec specification, string value = null)
        {
            var specType = specification.GetType();
            if (specType == typeof(AttributeSpecBool))
                return new AttributeValueWrapBool { Code = specification.Code, RawValue = value ?? specification.DefaultValue };
            if (specType == typeof(AttributeSpecInt))
                return new AttributeValueWrapInt { Code = specification.Code, RawValue = value ?? specification.DefaultValue };
            if (specType == typeof(AttributeSpecSelect))
                return new AttributeValueWrapSelect { Code = specification.Code, RawValue = value ?? specification.DefaultValue };
            if (specType == typeof(AttributeSpecText))
                return new AttributeValueWrapText { Code = specification.Code, RawValue = value ?? specification.DefaultValue };
            if (specType == typeof(AttributeSpecDateTime))
                return new AttributeValueWrapDateTime { Code = specification.Code, RawValue = value ?? specification.DefaultValue };

            return null;
        }

        public static AttributeValueWrap Clone(AttributeValueWrap source, string value = null)
        {
            var sourceType = source.GetType();
            if (sourceType == typeof(AttributeValueWrapBool))
                return new AttributeValueWrapBool { ServiceOrderId = source.ServiceOrderId, WorkOrderId = source.WorkOrderId, TaskId = source.TaskId, TaskInfoId = source.TaskInfoId, Code = source.Code, RawValue = value ?? source.RawValue };

            if (sourceType == typeof(AttributeValueWrapInt))
                return new AttributeValueWrapInt { ServiceOrderId = source.ServiceOrderId, WorkOrderId = source.WorkOrderId, TaskId = source.TaskId, TaskInfoId = source.TaskInfoId, Code = source.Code, RawValue = value ?? source.RawValue };

            if (sourceType == typeof(AttributeValueWrapSelect))
                return new AttributeValueWrapSelect { ServiceOrderId = source.ServiceOrderId, WorkOrderId = source.WorkOrderId, TaskId = source.TaskId, TaskInfoId = source.TaskInfoId, Code = source.Code, RawValue = value ?? source.RawValue };

            if (sourceType == typeof(AttributeValueWrapText))
                return new AttributeValueWrapText { ServiceOrderId = source.ServiceOrderId, WorkOrderId = source.WorkOrderId, TaskId = source.TaskId, TaskInfoId = source.TaskInfoId, Code = source.Code, RawValue = value ?? source.RawValue };

            if (sourceType == typeof(AttributeValueWrapDateTime))
                return new AttributeValueWrapDateTime { ServiceOrderId = source.ServiceOrderId, WorkOrderId = source.WorkOrderId, TaskId = source.TaskId, TaskInfoId = source.TaskInfoId, Code = source.Code, RawValue = value ?? source.RawValue };

            return null;
        }

        #endregion
    }
}
