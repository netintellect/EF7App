using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Repositories;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    [HasSelfValidation]
    public class TaskInfo : ValidatedEntity<TaskInfo>, IAttributeContainer
    {
        #region state
        private int _id;
        [JsonProperty, KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }

        private int _seqNo;
        [JsonProperty]
        public int SeqNo
        {
            get { return _seqNo; }
            set { SetProperty(value, ref _seqNo, () => SeqNo); }
        }

        private int _taskSpecificationId;
        [JsonProperty]
        public int TaskSpecificationId
        {
            get { return _taskSpecificationId; }
            set { SetProperty(value, ref _taskSpecificationId, () => TaskSpecificationId); }
        }

        private TaskSpecification _taskSpecification;
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: false)]
        public TaskSpecification TaskSpecification
        {
            get
            {
                if (_taskSpecification != null || !UseRepositoryFinder) return _taskSpecification;
                var repository = RepositoryFinder.GetRepository<ITaskSpecificationRepository>() as ITaskSpecificationRepository;
                return repository?.Filter(ts => ts.Id == TaskSpecificationId).FirstOrDefault();
            }
            set
            {
                _taskSpecification = value;
                OnPropertyChanged(() => TaskSpecification);
            }
        }

        private ObservableCollection<AttributeValueWrap> _attributes = new ObservableCollection<AttributeValueWrap>();
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto), HandleOnNesting, Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(AttributeValueWrap))]
        public ICollection<AttributeValueWrap> Attributes
        {
            get { return _attributes; }
            set { _attributes = value?.ToObservableCollection(); }
        }

        public int?     AttributeSpecId => TaskSpecification == null ? 0 : TaskSpecification.AttributeSpecId;

        public string FixCode => null;

        private int? _workOrderId;
        [JsonProperty]
        public int? WorkOrderId
        {
            get { return _workOrderId; }
            set { SetProperty(value, ref _workOrderId, () => WorkOrderId); }
        }

        private WorkOrder _workOrder;
        [JsonProperty]
        public WorkOrder WorkOrder
        {
            get { return _workOrder; }
            set
            {
                _workOrder = value;
                OnPropertyChanged(() => WorkOrder);
            }
        }

        private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(Task))]
        public ICollection<Task> Tasks
        {
            get { return _tasks; }
            set { _tasks = value?.ToObservableCollection(); }
        }
        #endregion

        #region behavior

        public override void ApplyLocalId()
        {
            if (Id == 0 && LocalId == Guid.Empty) LocalId = Guid.NewGuid();

            foreach (var task in Tasks)
            {
                task.ApplyLocalId();
            }
        }

        public static TaskInfo Create(TaskSpecification taskSpecification, User user)
        {
            var taskInfo = new TaskInfo
            {
                TaskSpecificationId = taskSpecification.Id,
                TaskSpecification = taskSpecification
            };
            if (user != null)
                taskInfo.SetAuditInfo(user.Login);
            return taskInfo;
        }

        public bool HasTasks()
        {
            return Tasks != null && Tasks.Any();
        }

        public void AddOrUpdateTask(Task task, string login)
        {
            if (task.IsNew)
            {
                task.UndoDelete(); //We have a virtual task => Remove IsDeleted flag
                task.SetAuditInfo(login);
                foreach (var attributeValueWrap in task.Attributes.Where(a => a.IsNew))
                {
                    attributeValueWrap.SetAuditInfo(login);
                }
                Tasks.Add(task);
            }
            else
            {
                var existingTask = Tasks.FirstOrDefault(t => t.Id == task.Id);
                if (existingTask != null)
                {
                    existingTask.SetResultCode(task.ResultCode);
                    existingTask.SetAuditInfo(login);
                }
            }
        }
        #endregion
    }
}
