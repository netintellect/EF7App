using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.Repositories;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkOrder : ValidatedEntity<WorkOrder>, IAttributeContainer
    {
        #region state
        [JsonProperty, IgnoreOnMap]
        public int ContractModelId { get; set; }

        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
        
        private string _externalRef;
        //[StringLengthValidator(1, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Ignore,
        //                        MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "WorkOrderExternalRefRequired")]
        [Display(ResourceType = typeof(Translations), Name ="WorkOrderExternalRef")]
        [JsonProperty]
		public string ExternalRef 
		{ 
 			get {   return _externalRef;    }
            set
            {
                if (SetProperty(value, ref _externalRef, () => ExternalRef))
                {
                    OnPropertyChanged(() => DisplayWorkOrder);
                }
            } 
		}

        private int _priority;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkOrderPriority")]
		public int Priority 
		{ 
 			get {   return _priority;   } 
 			set {   SetProperty(value,ref _priority, () => Priority);   } 
		}

        private WorkStatusType _status;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkOrderStatus")]
		public WorkStatusType Status 
		{ 
 			get {   return _status; }
		    set
		    {
		        if (SetProperty(value, ref _status, () => Status))
		        {
                    OnPropertyChanged(() => DisplayStatus);
		        }
		    } 
		}

        private int _serviceOrderId;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkOrderServiceOrderId")]
		public int ServiceOrderId 
		{ 
 			get {   return _serviceOrderId; } 
 			set {   SetProperty(value,ref _serviceOrderId, () => ServiceOrderId);   } 
		}

        private ServiceOrder _serviceOrder;
        [JsonProperty, Aggregation]
        public ServiceOrder ServiceOrder
        {
            get { return _serviceOrder; }
            set
            {
                _serviceOrder = value;
                OnPropertyChanged(() => ServiceOrder);
            }
        }

        private int _workSpecificationId;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkOrderSpecificationId")]
		public int WorkSpecificationId 
		{ 
 			get {   return _workSpecificationId;    }
		    set
		    {
		        if (SetProperty(value, ref _workSpecificationId, () => WorkSpecificationId))
		        {
                    OnPropertyChanged(() => DisplayWorkOrder);
		        }
		    } 
		}
        
        private WorkSpecification _workSpecification;
        [JsonProperty, Aggregation] 
        [NotNullValidator(MessageTemplateResourceType = typeof(Translations),
                          MessageTemplateResourceName = "WorkOrderWorkSpecificationRequired")]
		public WorkSpecification WorkSpecification 
		{
            get
            {
                if (_workSpecification != null || !UseRepositoryFinder) return _workSpecification;

                var repository = RepositoryFinder.GetRepository<IWorkSpecificationRepository>() as IWorkSpecificationRepository;
                return repository?.Filter(ws => ws.Id == WorkSpecificationId).FirstOrDefault();
            } 
			set 
            {   
                _workSpecification= value;
                if (_workSpecification != null && IsNew)
                    _workSpecificationId = _workSpecification.Id;
                IsPropertyValid(() => WorkSpecification, value);
				OnPropertyChanged(() => WorkSpecification);
			} 
		}

        private ObservableCollection<WorkOrderRemark> _workOrderRemarks = new ObservableCollection<WorkOrderRemark>();
        [HandleOnNesting] [Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(Remark))]
        [ObjectCollectionValidator(typeof(WorkOrderRemark))]
        [ObjectCollectionValidator(typeof(BookingRemark))]
        [JsonProperty]
		public ICollection<WorkOrderRemark> WorkOrderRemarks 
		{
			get {    return _workOrderRemarks;   } 
			set {   _workOrderRemarks= value?.ToObservableCollection();   } 
		}
        
        private ObservableCollection<WorkOrderAttachment> _workOrderAttachments = new ObservableCollection<WorkOrderAttachment>();
        [HandleOnNesting, Aggregation(isComposite: true)] [ObjectCollectionValidator(typeof (WorkOrderAttachment))]
        [JsonProperty]
        public ICollection<WorkOrderAttachment> WorkOrderAttachments
        {
            get { return _workOrderAttachments; }
            set { _workOrderAttachments = value?.ToObservableCollection(); }
        }

        private ObservableCollection<Visit> _visits = new ObservableCollection<Visit>();

        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public ICollection<Visit> Visits 
        {
			get {   return _visits;} 
			set {   _visits= value?.ToObservableCollection();} 
		}

        public Visit ActiveVisit
        {
            get
            {
                return Visits?.OrderByDescending(v => v.AuditInfo.CreatedOn)
                              .FirstOrDefault(v => v.Status == VisitStatusType.Unallocated || v.Status == VisitStatusType.Allocated);
            }
        }

        private ObservableCollection<TaskInfo> _taskInfos = new ObservableCollection<TaskInfo>();

        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public ICollection<TaskInfo> TaskInfos
        {
            get { return _taskInfos; }
            set { _taskInfos = value?.ToObservableCollection(); }
        }
        
        private int? _locationId;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkOrderLocationId")]
		public int? LocationId 
		{ 
 			get {   return _locationId; } 
 			set {   SetProperty(value,ref _locationId, () => LocationId);   } 
		}

        private Location _location;
        [HandleOnNesting, Aggregation(isComposite: true)] [ObjectValidator]
		[JsonProperty]
		public Location Location 
		{
			get {   return _location;   } 
			set 
            {   
                _location= value;
				OnPropertyChanged(() => Location);
			} 
		}

        private ObservableCollection<WorkOrderHistory> _workOrderHistories = new ObservableCollection<WorkOrderHistory>();
        [JsonProperty]
        [HandleOnNesting, Aggregation(isComposite: true)]
        public ICollection<WorkOrderHistory> WorkOrderHistories
        {
            get { return _workOrderHistories; }
            set { _workOrderHistories = value?.ToObservableCollection(); }
        }

        private ObservableCollection<AttributeValueWrap> _attributes = new ObservableCollection<AttributeValueWrap>();
        [JsonProperty]
        [HandleOnNesting]
        [Aggregation(isComposite: true)]
        public ICollection<AttributeValueWrap> Attributes
        {
            get { return _attributes; }
            set { _attributes = value?.ToObservableCollection(); }
        }
        
        public string FixCode => null;

        public int? AttributeSpecId => WorkSpecification?.AttributeSpecId;

        private bool _isInEditMode;
        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                OnPropertyChanged(() => IsInEditMode);
                OnPropertyChanged(() => ExternalRef);
            }

        }

        public static ObservableCollection<CodeGroup> PossibleStatuses => StaticFactory.Instance.GetCodeGroups(CodeGroupType.WorkStatus);

        public ObservableCollection<Remark> Remarks
        {
            get
            {
                return WorkOrderRemarks.Cast<Remark>()
                                       .OrderByDescending(r => r.EnteredDate)
                                       .ToObservableCollection();
            }
        }
        
        public string DisplayStatus => StaticFactory.Instance.GetValue(CodeGroupType.WorkStatus, Status.ToString()) ?? "?";

        public string DisplaySpecification
        {
            get
            {
                var specification = WorkSpecification != null ? WorkSpecification.DisplaySpecification : "?";
                return $"{Id} - {specification}";
            }
        }

        public string DisplayWorkOrder
        {
            get
            {
                string display1 = string.Format(Translations.WorkOrderDisplayString1, !IsNew ? Id.ToString() :  "?",
                                                                                      DisplayStatus);
                string display2 = string.Empty;
                if (WorkSpecification != null)
                {
                    display2 = string.Format(Translations.WorkOrderDisplayString2, 
                                             !string.IsNullOrEmpty(WorkSpecification.Code) ? WorkSpecification.Code : "?");
                
                }
                return $"{display1}\n{display2}";
            }
        }
        public bool HasLocation => Location != null;

        public int TotalOnsiteTimeRemaining
        {
            get { return Visits.Sum(v => v.OnsiteTimeRemainingTasks ?? 0); }
        }

        public int TotalOnSiteTimeNeeded
        {
            get
            {
                // calculate the total time still needed for this work order,
                // look at the selected taks for the time needed, but take into
                // account there are maybe already been previous visits
                if (Visits.Any())
                    return TotalOnsiteTimeRemaining;

                return TaskInfos.Where(ti => !ti.IsDeleted)
                    .Sum(ti => ti.TaskSpecification.OnsiteTime ?? 0);
            }

        }

        public int NumberOfCalls
        {
            get
            {
                return WorkOrderRemarks.OfType<BookingRemark>().Count();

            }
        }

        public int NumberOfFailedCalls
        {
            get
            {
                return
                    WorkOrderRemarks.OfType<BookingRemark>()
                        .Count(br => br.ResultCode?.Outcome == ResultCodeOutcomeType.Redo);

            }
        }
        #endregion

        #region behavior
        /// <summary>
        /// Update the WorkOrder's Status based on the status from (last) visit
        /// </summary>
        /// <param name="visit">The visit that will trigger the Workorder's status change</param>
        /// <param name="currentUserRole">The user requesting the status change</param>
        public void UpdateStatus(UserRole currentUserRole, Visit visit = null, bool doForce = false)
        {
            if (currentUserRole == null)
                throw new ArgumentNullException(nameof(currentUserRole));

            #region check transition NotReady => VisitNeeded
            if (Status == WorkStatusType.NotReady)
            {
                var doStatusChange = false;
                // rule: check all data is complete
                doStatusChange = ((WorkSpecificationId > 0 || WorkSpecification != null) &&
                                  (LocationId > 0 || Location != null) &&
                                  (TaskInfos.Any()));

                // rule: No visit exists yet or last visit has status closed with redo.
                var closedVisit = Visits.OrderByDescending(v => v.WindowStart)
                                        .FirstOrDefault();
                var isLastVisitClosedWithRedo = doForce || (closedVisit?.ResultCode != null && 
                                                 closedVisit.Status == VisitStatusType.Closed && 
                                                 closedVisit.ResultCode.Outcome == ResultCodeOutcomeType.Redo);
                doStatusChange = doStatusChange &&
                                 (!Visits.Any() || isLastVisitClosedWithRedo); 
                if (doStatusChange)
                {
                    ChangeStatusTo(WorkStatusType.VisitNeeded, currentUserRole);
                }
            }
            #endregion

            #region check transition VisitNeeded => VisitOpen (or Aborted)
            if (Status == WorkStatusType.VisitNeeded)
            {
                // rule: at least one visit and last visit not in status closed
                var lastVisit = Visits.OrderByDescending(v => v.WindowStart)
                                      .FirstOrDefault(v => v.Status != VisitStatusType.Closed);

                // rule: there is an appointment booked
                if (lastVisit != null && lastVisit.IsAppointmentBooked)
                {

                    ChangeStatusTo(WorkStatusType.VisitOpen, currentUserRole);
                }
            }
            #endregion

            #region check transition VisitOpen => Closed (or Aborted)
            if (Status == WorkStatusType.VisitOpen)
            {
                // (OR) rule: at least one ans last visit is closed with outcome done
                var lastVisit = Visits.OrderByDescending(v => v.WindowStart)
                                      .FirstOrDefault(v => v.Status == VisitStatusType.Closed);
                if (lastVisit?.ResultCode != null && 
                    lastVisit.ResultCode.Outcome == ResultCodeOutcomeType.Done)
                {
                    ChangeStatusTo(WorkStatusType.Closed, currentUserRole);
                }
                else
                {
                    // (OR) rule: last remark booking with result code abort
                    var lastRemark = WorkOrderRemarks.OfType<BookingRemark>()
                                                     .OrderByDescending(br => br.EnteredDate)    
                                                     .FirstOrDefault();
                    if (lastRemark?.ResultCode != null && 
                        lastRemark.ResultCode.Outcome == ResultCodeOutcomeType.Abort)
                    {
                        ChangeStatusTo(WorkStatusType.Closed, currentUserRole);
                    }
                }
            }
            #endregion

            // TODO Alain still relevatnt?
            #region check if the supplied visit is the last one in workorder
            if (visit != null) 
            {
                var lastVisit = Visits.OrderByDescending(v => v.PlannedStart).First();
                if (visit.Id != lastVisit.Id) return;

                if (visit.Status == VisitStatusType.Closed)
                {
                    if (visit.ResultCode == null)
                        throw new NotSupportedException(Translations.WorkOrderResultCodeRequired);
                    switch (visit.ResultCode.Outcome)
                    {
                        case ResultCodeOutcomeType.Abort:
                            ChangeStatusTo(WorkStatusType.Aborted, currentUserRole);
                            break;
                        case ResultCodeOutcomeType.Redo:
                            ChangeStatusTo(WorkStatusType.VisitNeeded, currentUserRole);
                            break;
                        case ResultCodeOutcomeType.Done:
                            ChangeStatusTo(WorkStatusType.Closed, currentUserRole);
                            break;
                    }
                }
                if (visit.Status == VisitStatusType.Aborted)
                    ChangeStatusTo(WorkStatusType.Aborted, currentUserRole);
            }
            #endregion
        }

        public void UpdateStatusDueToAppointmentBooking(Visit bookedVisit, BookingRemark bookingRemark, UserRole userRole)
        {
            if (bookingRemark == null) return;

            // preconditions
            if (Status != WorkStatusType.VisitNeeded &&
                Status != WorkStatusType.VisitOpen) return;
            if (bookedVisit != null ||
                bookingRemark.ResultCode.Outcome != ResultCodeOutcomeType.Done)
            {
                WorkOrderRemarks.Add(bookingRemark);
            }
            
            // check when status VISIT_NEEDED
            if (Status == WorkStatusType.VisitNeeded)
            {
                if (bookingRemark.ResultCode.Outcome == ResultCodeOutcomeType.Done)
                {
                    if (bookedVisit != null)
                    {
                        var visit = Visits.FirstOrDefault(v => v.Id == bookedVisit.Id);
                        if (visit == null)
                        {
                            Visits.Add(bookedVisit);
                        }
                        else
                        {
                            visit.AppointmentWindowId = bookedVisit.AppointmentWindowId;
                            visit.AppointmentWindow = bookedVisit.AppointmentWindow;
                            visit.WindowStart = bookedVisit.WindowStart;
                            visit.WindowEnd = bookedVisit.WindowEnd;
                        }
                    }
                }
                if (bookingRemark.ResultCode.Outcome == ResultCodeOutcomeType.Abort)
                {
                    ChangeStatusTo(WorkStatusType.Closed, userRole);
                }
            }

            // check when status VISIT_OPEN
            if (Status == WorkStatusType.VisitOpen)
            {
                var visit = Visits.OrderByDescending(v => v.WindowStart)
                                  .FirstOrDefault();
                if (visit != null && visit.SuperState == VisitSuperStateType.Provisional)
                {
                    if (bookingRemark.ResultCode.Outcome == ResultCodeOutcomeType.Done &&
                        bookedVisit != null)
                    {
                        visit.AppointmentWindowId = bookedVisit.AppointmentWindowId;
                        visit.AppointmentWindow = bookedVisit.AppointmentWindow;
                        visit.WindowStart = bookedVisit.WindowStart;
                        visit.WindowEnd = bookedVisit.WindowEnd;
                    }
                    if (bookingRemark.ResultCode.Outcome == ResultCodeOutcomeType.Abort)
                    {
                        visit.ChangeStatusTo(VisitStatusType.Closed, userRole);
                        ChangeStatusTo(WorkStatusType.Closed, userRole);
                    }
                }
            }
        }

        /// <summary>
        /// Change the WorkOrder's status, and log an history record for the status change
        /// </summary>
        /// <param name="status">The new status for the WorkOrder</param>
        /// <param name="currentUserRole">The user requesting the status change</param>
        public void ChangeStatusTo(WorkStatusType status, UserRole currentUserRole)
        {
            if (currentUserRole == null) return;
            var fromState = Status;
            Status = status;
            var workOrderHistory = WorkOrderHistory.Create(this, fromState, currentUserRole);
            if (workOrderHistory != null) WorkOrderHistories.Add(workOrderHistory);
        }

        public static WorkOrder Create(WorkSpecification specification, User user)
        {
            if (specification == null) return null;
            
            var workOrder = new WorkOrder
            {
                Status = WorkStatusType.NotReady,
                WorkSpecificationId = specification.Id,
                WorkSpecification = specification
            };
            if (user != null)
                workOrder.SetAuditInfo(user.Login);

            return workOrder;
        }
        
        public static WorkOrder Create(ServiceOrder serviceOrder, User user)
        {
            if (serviceOrder == null) return null;
            
            var workOrder = new WorkOrder
            {
                Status = WorkStatusType.NotReady,
                ServiceOrderId = serviceOrder.Id,
                ServiceOrder = serviceOrder
            };
            if (user != null)
                workOrder.SetAuditInfo(user.Login);

            return workOrder;
        }

        /// <summary>
        /// For Mobile app => set the LocalId for added entities (those that have their id set to 0)
        /// </summary>
        public override void ApplyLocalId()
        {
            foreach (var workOrderHistory in WorkOrderHistories)
            {
                workOrderHistory.ApplyLocalId();
            }
            foreach (var workOrderRemark in WorkOrderRemarks)
            {
                workOrderRemark.ApplyLocalId();
            }
            foreach (var visit in Visits)
            {
                visit.ApplyLocalId();
            }
            foreach (var taskInfo in TaskInfos)
            {
                taskInfo.ApplyLocalId();
            }
        }

        /// <summary>
        /// Create a list of tasks for a visit. A workorder can have multiple visits
        /// Based on the WorkOrder.SelectedTasks a list is created with task attached to the actual visit it was executed
        /// </summary>
        /// <param name="visit">The visit for which the Tasks are requested</param>
        /// <returns></returns>
        public ICollection<Task> GetTasksForVisit(Visit visit)
        {
            // For a visit, all Tasks defined in WorkOrder.SelectedTasks have to be performed by a technician.
            // In redo scenario's, it is possible that multiple Visits are created.
            // Each Visit shows the same number of tasks (as defined by WorkOrder.SelectedTasks), with their corrsponding status

            if (visit == null) return null;

            var tasks = new List<Task>();

            foreach (var taskInfo in TaskInfos)
            {
                //Only the last task from a certain specification is shown
                var existingTask = Visits.SelectMany(v => v.Tasks).LastOrDefault(t => t.TaskInfo.TaskSpecificationId == taskInfo.TaskSpecificationId && !t.IsDeleted);
                if (existingTask == null)
                {
                    existingTask = Task.Create(taskInfo, null); //Task does not yet exist on visit => Create Virtual Task to show in list
                    existingTask.VisitId = visit.Id;
                    existingTask.Visit = visit;
                    existingTask.EndEdit();
                    existingTask.Delete();
                }
                else
                {
                    //Task => set to unmodified (Task boundary)
                    existingTask.EndEdit();
                }
                tasks.Add(existingTask);
            }
            return new ObservableCollection<Task>(tasks.OrderBy(t => t.TaskInfo.SeqNo));
        }

        /// <summary>
        /// Create a list of all visits preceding the supplied visit
        /// </summary>
        /// <param name="visit">The visit for which the history of visits is requested</param>
        /// <returns></returns>
        public ICollection<Visit> GetPastVisits(Visit visit)
        {
            if (visit == null) return null;
            var pastVisits = new ObservableCollection<Visit>();

            var list = Visits.OrderBy(v => v.PlannedStart);

            foreach (var pastVisit in list)
            {
                if (pastVisit.Id==visit.Id) break;
                pastVisits.Add(pastVisit);
            }
            return pastVisits;
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => HasLocation);
            OnPropertyChanged(() => ExternalRef,
                              () => DisplayStatus);
            OnPropertyChanged(() => WorkOrderHistories);
            OnPropertyChanged(() => Visits);
            OnPropertyChanged(() => WorkOrderRemarks);
            OnPropertyChanged(() => TaskInfos);
            OnPropertyChanged(() => TotalOnSiteTimeNeeded);
        }

        public override void CancelEdit()
        {
            base.CancelEdit();

            //Check if there are any WorkOrderRemarks added
            var remarksToRemove = WorkOrderRemarks.Where(vr => vr.IsNew).ToList();
            if (remarksToRemove.Any())
                foreach (var visitRemark in remarksToRemove)
                {
                    WorkOrderRemarks.Remove(visitRemark);
                }
        }

        #endregion
        
    }
}