using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.Data;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    [HasSelfValidation]
    public class Task : ValidatedEntity<Task>, IAttributeContainer
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private TaskStatusType _status;
        [JsonProperty]
        public TaskStatusType Status
        {
            get {   return _status; }
            set {   SetProperty(value, ref _status, () => Status);  }
        }

        private int _taskInfoId;
        [JsonProperty]
        public int TaskInfoId
        {
            get { return _taskInfoId; }
            set { SetProperty(value, ref _taskInfoId, () => TaskInfoId); }
        }

        private TaskInfo _taskInfo;
        [JsonProperty, Aggregation]
        public TaskInfo TaskInfo
        {
            get { return _taskInfo; }
            set
            {
                _taskInfo = value;
                OnPropertyChanged(() => TaskInfo);
            }
        }

        private int? _resultCodeId;
        [JsonProperty]
        public int? ResultCodeId
        {
            get {   return _resultCodeId; }
            set {   SetProperty(value, ref _resultCodeId, () => ResultCodeId);  }
        }

        private ResultCode _resultCode;
        [JsonProperty, Aggregation]
        public ResultCode ResultCode
        {
            get { return _resultCode; }
            set 
            {   
                _resultCode = value;
                OnPropertyChanged(() => ResultCode);
            }
        }
        
        private int _visitId;
        [JsonProperty]
        public int  VisitId
        {
            get {   return _visitId; }
            set {   SetProperty(value, ref _visitId, () => VisitId);    }
        }

        private Visit _visit;
        [JsonProperty, Aggregation]
        public Visit Visit
        {
            get { return _visit; }
            set 
            {   
                _visit = value;
                OnPropertyChanged(() => Visit);
            }
        }

        private ObservableCollection<AttributeValueWrap> _attributes = new ObservableCollection<AttributeValueWrap>();
        [HandleOnNesting] [Aggregation(isComposite: true)] [ObjectCollectionValidator(typeof(AttributeValueWrap))]
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public ICollection<AttributeValueWrap> Attributes
        {
            get { return _attributes; }
            set { _attributes = value?.ToObservableCollection(); }
        }

        public string FixCode => ResultCode?.Code;

        //public int? AttributeSpecId => TaskSpecification == null ? 0 : TaskSpecification.AttributeSpecId;
        public int? AttributeSpecId => TaskInfo?.AttributeSpecId;

        public static ObservableCollection<CodeGroup> PossibleStatuses => StaticFactory.Instance.GetCodeGroups(CodeGroupType.TaskStatus);
        #endregion

        #region behavior

        public string DisplayTaskSpecification => TaskInfo?.TaskSpecification == null ? "?" : $"{TaskInfo.TaskSpecification.Code ?? "?"} - {TaskInfo.TaskSpecification.Description ?? "?"}";

        public int DisplayOnsiteTime
        {
            get
            {
                if (TaskInfo?.TaskSpecification == null)
                    return 0;

                return TaskInfo.TaskSpecification.OnsiteTime ?? 0;
            }
        }

        public string DisplayStatus => StaticFactory.Instance.GetValue(CodeGroupType.TaskStatus, Status.ToString()) ?? "?";

        public string DisplayResultCode => ResultCode == null ? "?" : ResultCode.DisplayItem;

        public string DisplayResultStatus => ResultCode == null ? Translations.ResultCodeOutcomeNotSet : ResultCode.DisplayOutCome;

        public override void CancelEdit()
        {
            base.CancelEdit();
            var newValueWraps = Attributes.Where(a => a.IsNew).ToList();
            foreach (var attributeValueWrap in newValueWraps)
            {
                Attributes.Remove(attributeValueWrap);
            }
        }

        [SelfValidation]
        public void ValidateAttributes(ValidationResults results)
        {
            const string errorField = "Attributes";
            string errorMessage = "Some attributes have erros";
            if (TaskInfo.TaskSpecification.AttributeSpecs != null)
            {
                var isValid = TaskInfo.TaskSpecification.AttributeSpecs.IsValid;
                if (!isValid)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(errorMessage);

                    var specs = TaskInfo.TaskSpecification.AttributeSpecs.GetAllAttributeSpecs();
                    foreach (var attributeSpec in specs)
                    {
                        var validationerrors = attributeSpec.ValidationErrors.ToList();
                        if (validationerrors.Any())
                        {
                            foreach (var validationerror in validationerrors)
                            {
                                sb.AppendLine(validationerror.Message);
                            }
                        }
                    }
                    var validationResult = new ValidationResult(sb.ToString(), this, errorField, null, EntityValidator);
                    SetValidationError(validationResult);
                }
                else
                {
                    ClearValidationError(errorField);
                }
            }
        }
        
        public static Task Create(TaskInfo taskInfo, string login)
        {
            var task = new Task
            {
                Status = TaskStatusType.New,
                TaskInfoId =  taskInfo.Id,
                TaskInfo = taskInfo
            };

            task.SetAuditInfo(login);

            //Set the attribute information received from TaskInfo
            foreach (var attributeValueWrap in taskInfo.Attributes)
            {
                var valueWrap = AttributeValueWrap.Clone(attributeValueWrap);
                if (valueWrap != null)
                {
                    var clone = AttributeValueWrap.Clone(attributeValueWrap);
                    //remove the TaskInfoId, because the ValueWrap now belongs to the task
                    clone.TaskInfoId = null;
                    clone.SetAuditInfo(login);
                    task.Attributes.Add(clone);
                }
            }

            return task;
        }

        public void SetResultCode(ResultCode resultCode)
        {
            if (resultCode != null)
            {
                ResultCodeId = resultCode.Id;
                ResultCode = resultCode;
                Status = ResultCode.Outcome == ResultCodeOutcomeType.Abort ? TaskStatusType.Aborted : TaskStatusType.Closed;
            }
            else
            {
                ResultCodeId = null;
                ResultCode = null;
                Status = TaskStatusType.New;
            }
        }

        /// <summary>
        /// For Mobile app => set the LocalId for added entities (those that have their id set to 0)
        /// </summary>
        public override void ApplyLocalId()
        {
            if (Id == 0 && LocalId == Guid.Empty) LocalId = Guid.NewGuid();

            foreach (var attributeValueWrap in Attributes)
            {
                attributeValueWrap.ApplyLocalId();
            }
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => DisplayStatus,
                              () => DisplayTaskSpecification,
                              () => DisplayResultCode,
                              () => DisplayResultStatus);
            OnPropertyChanged(() => Attributes);
        }
        #endregion
    }
}
