using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.Data;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Technicians;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ValidationResult = ServiceCruiser.Model.Validations.Core.ValidationResult;

namespace ServiceCruiser.Model.Entities.FieldServices
{
	[JsonObject(MemberSerialization.OptIn)]
	[HasSelfValidation]
	public class Visit : ValidatedEntity<Visit>
	{
		#region state
		private int _id;
		[JsonProperty][KeyNew(true)]
        public int Id 
		{ 
			get {   return _id; } 
			set {   SetProperty(value,ref _id, () => Id);   } 
		}

		private DateTimeOffset? _kpiStart;
		[JsonProperty]
		public DateTimeOffset? KpiStart 
		{ 
			get {   return _kpiStart;   } 
			set {   SetProperty(value,ref _kpiStart, () => KpiStart);   } 
		}

		private DateTimeOffset _windowStart;
		[DateTimeOffsetRangeValidator("1900-01-01T00:00:00", RangeBoundaryType.Inclusive, "2014-01-01T00:00:01", RangeBoundaryType.Ignore,
									  MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "VisitWindowStartRequired")]
		[Display(ResourceType = typeof(Translations), Name = "VisitWindowStart")]        
		[JsonProperty]
		public DateTimeOffset  WindowStart 
		{ 
			get {   return _windowStart;    }
			set
			{
				if (SetProperty(value, ref _windowStart, () => WindowStart))
				{
					OnPropertyChanged(() => DisplayWindowPeriod);
				}
			} 
		}

		private DateTimeOffset _windowEnd;
		[ValidatorComposition(CompositionType.Or,
			MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "VisitWindowStartBeforeEnd")]
		[NotNullValidator(Negated = true,
			MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "VisitWindowStartBeforeEnd")]
		[PropertyComparisonValidator("WindowStart", ComparisonOperator.GreaterThanEqual,
			MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "VisitWindowStartBeforeEnd")]
		[Display(ResourceType = typeof(Translations), Name = "VisitWindowEnd")]

		[JsonProperty]
		public DateTimeOffset WindowEnd 
		{ 
			get {   return _windowEnd;  }
			set
			{
				if (SetProperty(value, ref _windowEnd, () => WindowEnd))
				{
					OnPropertyChanged(() => DisplayWindowPeriod);
				}
			} 
		}
				
		private DateTimeOffset? _plannedStart;
		[JsonProperty]
		public DateTimeOffset? PlannedStart 
		{ 
			get {   return _plannedStart;   }
			set
			{
				if (SetProperty(value, ref _plannedStart, () => PlannedStart))
				{
					OnPropertyChanged(() => DisplayPlannedPeriod);
					OnPropertyChanged(() => PlannedEnd);
					OnPropertyChanged(() => TechnicianPlannedWorkDay);
				}
			} 
		}

		private int? _extraTime;
		[JsonProperty]
		public int? ExtraTime
		{
			get { return _extraTime; }
			set { SetProperty(value, ref _extraTime, () => ExtraTime); }
		}
				
		public DateTimeOffset? PlannedEnd => PlannedStart?.AddMinutes(OnsiteTimeInitially ?? 0);

		/// <summary>
		/// Calculates initial work-order onsite time, i.e. when no tasks have been done yet.
		/// </summary>
		public int? OnsiteTimeInitially
		{
			get
			{
				if (WorkOrder == null) return null;
				if (!WorkOrder.TaskInfos.Any()) return null;

                return WorkOrder.TaskInfos.Where(wo => !wo.IsDeleted).Sum(t => t.TaskSpecification.OnsiteTime);
            }
		}

		/// <summary>
		/// Calculates work-order onsite time, taking into account finished tasks.
		/// </summary>
		public int? OnsiteTimeRemainingTasks
		{
			get
			{
				if (WorkOrder == null) return null;
				if (!WorkOrder.TaskInfos.Any()) return null;

				int minutes = 0;
				foreach (var taskInfo in WorkOrder.TaskInfos)
				{
                    if (taskInfo.IsDeleted) continue;
				    var task = Tasks.SingleOrDefault(t => t.TaskInfoId == taskInfo.Id);
					if (task == null || 
						(task.Status != TaskStatusType.Closed && task.Status != TaskStatusType.Aborted &&
						 task.ResultCode?.Outcome != ResultCodeOutcomeType.Done))
						minutes += taskInfo.TaskSpecification?.OnsiteTime ?? 0;
				}
				return minutes;
			}                
		}

		public int? OnTravelTime
		{
			get
			{
				var contractModel = WorkOrder?.WorkSpecification?.ServiceSpecification?.ContractModel;
				if (contractModel == null) return null;

				var region = WorkOrder?.Location?.RegionFor(contractModel);
				if (region == null) return null;

				return WorkOrder.WorkSpecification.TravelTimes?.FirstOrDefault(tt => tt.WorkSpecificationId == WorkOrder.WorkSpecificationId &&
																					 tt.RegionId == region.Id)?.MeanTime;
			}
		}

		private DateTimeOffset? _effectiveStart;
		[JsonProperty]
		public DateTimeOffset? EffectiveStart 
		{ 
			get {   return _effectiveStart; } 
			set {   SetProperty(value,ref _effectiveStart, () => EffectiveStart);   } 
		}

		private DateTimeOffset? _effectiveEnd;
		[JsonProperty]
		public DateTimeOffset? EffectiveEnd 
		{ 
			get {   return _effectiveEnd;   } 
			set {   SetProperty(value,ref _effectiveEnd, () => EffectiveEnd);   } 
		}
	
		private VisitStatusType _status;
		[JsonProperty]
		public VisitStatusType Status 
		{ 
			get {   return _status; }
			set
			{
				if (SetProperty(value, ref _status, () => Status))
				{
					OnPropertyChanged(() => SuperState);
                    OnPropertyChanged(() => DisplayStatus);
				}
			} 
		}

		public VisitSuperStateType 
            SuperState
		{
			get
			{
				switch (Status)
				{
					case VisitStatusType.Unallocated:
					case VisitStatusType.Allocated:
						return VisitSuperStateType.Provisional;
					case VisitStatusType.Committed:
					case VisitStatusType.Accepted:
					case VisitStatusType.OnSite:
					case VisitStatusType.Travelling:
						return VisitSuperStateType.TechnicianOwned;
					default:
						return VisitSuperStateType.Undetermined;
				}
			}
		}

		private int _workOrderId;
		[JsonProperty]
		public int WorkOrderId 
		{ 
			get {   return _workOrderId;    } 
			set {   SetProperty(value,ref _workOrderId, () => WorkOrderId); } 
		}

		private int? _technicianId;
		[JsonProperty]
		public int? TechnicianId 
		{ 
			get {   return _technicianId;   } 
			set {   SetProperty(value,ref _technicianId, () => TechnicianId);   } 
		}

		private Technician _technician;
		[JsonProperty, Aggregation]
		public Technician Technician
		{
			get {   return _technician; }
			set 
			{   
				_technician = value;
				OnPropertyChanged(() => Technician);
				OnPropertyChanged(() => HasTechnicianAssigned);
			}
		}

		private int? _resultCodeId;
		[JsonProperty]
		public int? ResultCodeId 
		{ 
			get {   return _resultCodeId;   } 
			set {   SetProperty(value,ref _resultCodeId, () => ResultCodeId);   } 
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

		private PlanningOverrideType _overrideType;
		[JsonProperty]
		public PlanningOverrideType OverrideType 
		{ 
			get {   return _overrideType;   }
			set
			{
				if (SetProperty(value, ref _overrideType, () => OverrideType))
				{
					OnPropertyChanged(() => DisplayPlanningOverride);
				}
			} 
		}
				
		private string _signatureName;
		[JsonProperty]
		public string SignatureName 
		{ 
			get {   return _signatureName;  } 
			set {   SetProperty(value,ref _signatureName, () => SignatureName); } 
		}

		private int? _signatureId;
		[JsonProperty]
		public int? SignatureId
		{
			get { return _signatureId; }
			set { SetProperty(value, ref _signatureId, () => SignatureId); }
		}

		private VisitSignatureAttachment _signature;
		[JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
		public VisitSignatureAttachment Signature
		{
			get { return _signature; }
			set
			{
				_signature = value;
				OnPropertyChanged(() => Signature);
			}
		}

		private ObservableCollection<VisitRemark> _visitRemarks = new ObservableCollection<VisitRemark>();
		[HandleOnNesting] [Aggregation(isComposite: true)]
		[ObjectCollectionValidator(typeof(Remark))]
		[ObjectCollectionValidator(typeof(VisitRemark))]
		[JsonProperty]
		public ICollection<VisitRemark> VisitRemarks
		{
			get { return _visitRemarks; }
			set { _visitRemarks = value?.ToObservableCollection(); }
		}

		private ObservableCollection<VisitHistory> _visitHistories = new ObservableCollection<VisitHistory>();
		[JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
		[ObjectCollectionValidator(typeof(VisitHistory))]
		public ICollection<VisitHistory> VisitHistories
		{
			get { return _visitHistories; }
			set { _visitHistories = value?.ToObservableCollection(); }
		}

		private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();
		//[JsonProperty, Aggregation]
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public ICollection<Task> Tasks
		{
			get { return _tasks; }
			set { _tasks = value?.ToObservableCollection(); }
		}

		private ObservableCollection<Rma> _rmas = new ObservableCollection<Rma>();
		[JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
		public ICollection<Rma> Rmas
		{
			get { return _rmas; }
			set { _rmas = value?.ToObservableCollection(); }
		}

		private WorkOrder _workOrder;
		[JsonProperty, Aggregation]
		public WorkOrder WorkOrder 
		{
			get {   return _workOrder;  } 
			set {   
				_workOrder= value;
				OnPropertyChanged(() => WorkOrder);
			} 
		}

		private int? _technicianRoleId;
		[JsonProperty]
		public int? TechnicianRoleId
		{
			get { return _technicianRoleId; }
			set { SetProperty(value, ref _technicianRoleId, () => TechnicianRoleId); }
		}

		private TechnicianRole _technicianRole;
		[HandleOnNesting, Aggregation]
		[JsonProperty]
		public TechnicianRole TechnicianRole 
		{
			get {   return _technicianRole;   } 
			set
			{   
				_technicianRole= value;
				OnPropertyChanged(() => TechnicianRole);
			} 
		}

		private int? _appointmentWindowId;
		[JsonProperty]
		public int? AppointmentWindowId
		{
			get { return _appointmentWindowId; }
			set { SetProperty(value, ref _appointmentWindowId, () => AppointmentWindowId); }
		}

		private AppointmentWindow _appointmentWindow;
		[JsonProperty, Aggregation]
		public AppointmentWindow AppointmentWindow
		{
			get { return _appointmentWindow; }
			set
			{
				_appointmentWindow = value;
				UpdateAppointmentWindow(_appointmentWindow);
				OnPropertyChanged(() => AppointmentWindow);
			}
		}

		private void UpdateAppointmentWindow(AppointmentWindow appointmentWindow)
		{
			if (_appointmentWindow == null) return;
			if (OverrideType.HasFlag(PlanningOverrideType.Window)) return;
			
            // take the predefined start and end date of the appointment window
			// only the date portion will vary
			var startTimeSpan = new TimeSpan(AppointmentWindow.WindowStart.Hours,
											 AppointmentWindow.WindowStart.Minutes,
											 AppointmentWindow.WindowStart.Seconds);
		    WindowStart = new DateTimeOffset(new DateTime(WindowStart.Year, WindowStart.Month, WindowStart.Day), WindowStart.Offset).Add(startTimeSpan);

			var endTimeSpan = new TimeSpan(AppointmentWindow.WindowEnd.Hours,
										   AppointmentWindow.WindowEnd.Minutes,
										   AppointmentWindow.WindowEnd.Seconds);
			WindowEnd = new DateTimeOffset(new DateTime(WindowEnd.Year, WindowEnd.Month, WindowEnd.Day), WindowEnd.Offset).Add(endTimeSpan);
        }

		private ObservableCollection<DseSla> _dseSlas = new ObservableCollection<DseSla>();
		[JsonProperty, HandleOnNesting, Aggregation(isComposite:true)]
		public ICollection<DseSla> DseSlas
		{
			get { return _dseSlas; }
			set { _dseSlas = value?.ToObservableCollection(); }
		}

		private ObservableCollection<LogOff> _logOffItems = new ObservableCollection<LogOff>();
		[JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
		public ICollection<LogOff> LogOffItems
		{
			get { return _logOffItems; }
			set { _logOffItems = value?.ToObservableCollection(); }
		}
		
		private ExtraOrder _extraOrder;
		[JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
		public ExtraOrder ExtraOrder
		{
			get { return _extraOrder; }
			set
			{
				_extraOrder = value;
				OnPropertyChanged(() => ExtraOrder);
			}
		}

        [HandleOnNesting, Aggregation]
		public User User { get; set; }
        public UserRole CurrentUserRole { get; set; }

	    private bool _isCommitForced;
        [JsonProperty, IgnoreOnMap]
	    public bool IsCommitForced
	    {
	        get { return _isCommitForced;}
            set
            {
                _isCommitForced = value;
                if (_isCommitForced)
                {
                    if (Status == VisitStatusType.Committed) return;
                    ChangeStatusTo(VisitStatusType.Committed, CurrentUserRole);
                }
                else
                {
                    if (IsPropertyChanged("Status"))
                    {
                        VisitStatusType originalStatus;
                        var result = Enum.TryParse(GetOriginalValue<Visit, VisitStatusType>(wo => wo.Status).ToString(),
                                                   out originalStatus);
                        if (result)
                        {
                            Status = originalStatus;
                            ChangedPropertyNames.Remove("Status");
                        }

                    }
                }
                OnPropertyChanged(() => IsCommitForced);
            }
	    }

		public static ObservableCollection<CodeGroup> PossibleStatuses => StaticFactory.Instance.GetCodeGroups(CodeGroupType.VisitStatus);

	    public string DisplayStatus => StaticFactory.Instance.GetValue(CodeGroupType.VisitStatus, Status.ToString()) ?? "?";

        public string DisplayWindowPeriod
        {
            get
            {
                var fmt = CultureInfo.CurrentCulture.DateTimeFormat;
                return $"{(WindowStart != DateTimeOffset.MinValue ? WindowStart.LocalDateTime.ToString("g", fmt) : "?")} - " +
                       $"{(WindowEnd != DateTimeOffset.MinValue ? WindowEnd.LocalDateTime.ToString("t", fmt) : "?")}";
            }
        }

        public string DisplayWindowTime => $"{(WindowStart != DateTimeOffset.MinValue ? $"{WindowStart.LocalDateTime:HH:mm}" : "?")} - {(WindowEnd != DateTimeOffset.MinValue ? $"{WindowEnd.LocalDateTime:HH:mm}" : "?")}";

        public string DisplayPlannedPeriod
		{
			get
			{
                var fmt = CultureInfo.CurrentCulture.DateTimeFormat;
                return $"{PlannedStart?.LocalDateTime.ToString("g", fmt) ?? "?"} - " +
                       $"{PlannedEnd?.LocalDateTime.ToString("t", fmt) ?? "?"}";
			}
		}

        public string DisplayPlannedTime => $"{(PlannedStart.HasValue ? $"{PlannedStart.Value.LocalDateTime:HH:mm}" : "?")} - {(PlannedEnd.HasValue ? $"{ PlannedEnd.Value.LocalDateTime:HH:mm}" : "?")}";

        public string DisplayEffectivePeriod
		{
			get
			{
                var fmt = CultureInfo.CurrentCulture.DateTimeFormat;
                return $"{EffectiveStart?.LocalDateTime.ToString("g", fmt) ?? "?"} - " +
                       $"{EffectiveEnd?.LocalDateTime.ToString("t", fmt) ?? "?"}";
			}
		}

        public string DisplayEffectiveTime => $"{(EffectiveStart.HasValue ? $"{EffectiveStart.Value.LocalDateTime:HH:mm}" : "?")} - {(EffectiveEnd.HasValue ? $"{ EffectiveEnd.Value.LocalDateTime:HH:mm}" : "?")}";

		public string DisplayVisitLine1
		{
			get
			{
				var display1 = string.Format(Translations.VisitDisplayString1, !IsNew ? Id.ToString() : "?",
																			   DisplayStatus,
																			   DisplayPlannedPeriod);
				
				return display1;
			}
		}
		
		public string DisplayVisitLine2
		{
			get
			{
				var display2 = string.Format(Translations.VisitDisplayString2, Technician != null ? Technician.DisplayName : "?",
																			   EffectiveStart?.ToString("d") ?? "?",
																			   EffectiveEnd?.ToString("d") ?? "?");
				return display2;
			}
		}

		public string DisplayWorkSpecification => WorkOrder?.WorkSpecification != null ? $"{WorkOrder.WorkSpecification.Code} - {WorkOrder.WorkSpecification.Description}" : "?";

	    public string DisplayAudit
	    {
	        get
	        {
	            return string.Format(Translations.DisplayAuditMessage,
	                !string.IsNullOrEmpty(AuditInfo.CreatedBy) ? AuditInfo.CreatedBy : "?",
	                AuditInfo.CreatedOn != null ? AuditInfo.CreatedOn.Value.ToString("d") : "?",
	                AuditInfo.CreatedOn != null ? AuditInfo.CreatedOn.Value.ToString("t") : "");
	        }
	    }

        public ObservableCollection<Remark> Remarks
		{
			get
			{
				var remarks = new ObservableCollection<Remark>();
				if (VisitRemarks == null || !VisitRemarks.Any()) return remarks;
				
				return VisitRemarks.Cast<Remark>()
								   .ToObservableCollection();
			}
		}

		public ObservableCollection<Task> AllTasks
		{
			get
			{
				if (WorkOrder == null) return Tasks.ToObservableCollection();

				return WorkOrder.GetTasksForVisit(this)
								.ToObservableCollection();
			}
		}

		public WorkDay TechnicianPlannedWorkDay
		{
			get
			{
				if (Technician == null ||
					!Technician.WorkDays.Any()) return null;
				if (PlannedStart == null) return null;

				return Technician.WorkDays.FirstOrDefault(wd => wd.Date.Date == PlannedStart.Value.Date);
			}
		}

		public bool HasTechnicianAssigned => Technician != null;

		public string DisplayPlanningOverride
		{
			get
			{
				var overrides = new List<string>();
				
				if (OverrideType.HasFlag(PlanningOverrideType.Window))
				{
					var codeValue = StaticFactory.Instance.GetValue(CodeGroupType.PlanningOverrideType, PlanningOverrideType.Window.ToString());
					overrides.Add(codeValue ?? "?");
				}
				if (OverrideType.HasFlag(PlanningOverrideType.Start))
				{
					var codeValue = StaticFactory.Instance.GetValue(CodeGroupType.PlanningOverrideType, PlanningOverrideType.Start.ToString());
					overrides.Add(codeValue ?? "?");
				}
				if (OverrideType.HasFlag(PlanningOverrideType.Technician))
				{
					var codeValue = StaticFactory.Instance.GetValue(CodeGroupType.PlanningOverrideType, PlanningOverrideType.Technician.ToString());
					overrides.Add(codeValue ?? "?");
				}

				if (!overrides.Any())
					return StaticFactory.Instance.GetCode(CodeGroupType.PlanningOverrideType, PlanningOverrideType.None.ToString()) ?? "?";

				return overrides.Aggregate((a, b) => a + ", " + b);
			}
		}
		#endregion

		#region behavior
		[SelfValidation]
		public void CheckResultCodeOnEndedStatus(ValidationResults results)
		{
			const string errorField = "ResultCode";
			string errorMessage = Translations.VisitNoResultCodeOnEndedStatus;
			if (Status == VisitStatusType.Closed && ResultCodeId == null)
			{
				var validationResult = new ValidationResult(errorMessage, this, errorField, null, EntityValidator);
				results.AddResult(validationResult);
				SetValidationError(validationResult);
			} 
		}

        [SelfValidation]
        public void CheckPlannedStartOnCommit(ValidationResults results)
        {
            const string errorField = "PlannedStart";
            string errorMessage = Translations.CheckPlannedStartOnCommit;
            if (Status == VisitStatusType.Committed && PlannedStart == null)
            {
                var validationResult = new ValidationResult(errorMessage, this, errorField, null, EntityValidator);
                results.AddResult(validationResult);
                SetValidationError(validationResult);
            }
        }
        
        public bool IsAppointmentBooked
	    {
	        get
	        {
                if (AppointmentWindow != null) return true;
	            return OverrideType != PlanningOverrideType.None &&
	                   WindowStart != DateTimeOffset.MinValue &&
	                   WindowEnd != DateTimeOffset.MinValue;
	        }
	    }

		public static Visit Create(WorkOrder workOrder, User user, UserRole userRole = null)
		{
			if (workOrder == null) return null;

			var visit = new Visit
			{
				WorkOrderId = workOrder.Id,
				WorkOrder = workOrder
			};
            visit.ChangeStatusTo(VisitStatusType.Unallocated, userRole);
            if (user != null)
				visit.SetAuditInfo(user.Login);

			return visit;
		}

		public bool HasTasks()
		{
			return Tasks != null && Tasks.Any(t => !t.IsDeleted);
		}

        public void AddOrUpdateTask(Task task, string login)
        {
            //Visit.Tasks & TaskInfo.Tasks have to be in sync, otherwise
            //Insert/Update will fail with Foreign Key Null
            TaskInfo taskInfo = null;
            if (WorkOrder?.TaskInfos != null)
                taskInfo = WorkOrder.TaskInfos.FirstOrDefault(ti => ti.Id == task.TaskInfoId);

            if (task.IsNew)
            {
                task.UndoDelete(); //We have a virtual task => Remove IsDeleted flag
                if (!string.IsNullOrEmpty(login )) task.SetAuditInfo(login);

                task.SetAuditInfo(login);
                foreach (var attributeValueWrap in task.Attributes.Where(a => a.IsNew))
                {
                    attributeValueWrap.SetAuditInfo(login);
                }
                Tasks.Add(task);
                taskInfo?.Tasks.Add(task);
            }
            else
            {
                var existingTask = Tasks.FirstOrDefault(t => t.Id == task.Id);
                if (existingTask != null)
                {
                    existingTask.SetResultCode(task.ResultCode);
                    existingTask.SetAuditInfo(login);
                }
                var existingTaskInfoTask = taskInfo?.Tasks.FirstOrDefault(t => t.Id == task.Id);
                if (existingTaskInfoTask != null)
                {
                    existingTaskInfoTask.SetResultCode(task.ResultCode);
                    existingTaskInfoTask.SetAuditInfo(login);
                }
            }
        }

        public Task ReopenTask(Task task, string login)
        {
            if (Tasks.Any(t => t.Id == task.Id)) //Only Task of previous Visit can be reopened
                return null;
            if (task?.ResultCode == null || task.ResultCode.Outcome != ResultCodeOutcomeType.Done) 
				return null;

            //When the task is re-opened, the Result of the Visit is unclear
            ResultCode = null;
            ResultCodeId = null;

            var newTask = Task.Create(task.TaskInfo, login);
			return newTask;
		}

		public Task ContinueTask(Task task, string login)
		{
            if (Tasks.Any(t => t.Id == task.Id)) //Only Task of previous Visit can be continued
                return null;
            if (task?.ResultCode == null || task.ResultCode.Outcome != ResultCodeOutcomeType.Redo)
				return null;

            //When the task is continued, the Result of the Visit is unclear
            ResultCode = null;
            ResultCodeId = null;

            var newTask = Task.Create(task.TaskInfo, login);
			return newTask;         
		}

		public void DeleteLogOff(LogOff logOffItem)
		{
			if (logOffItem == null) return;

            //When Offline => logOffItem will contain LocalId (means IsNew == false)
            //so, we explicitly check the 0 on Id 
            if (logOffItem.IsNew || logOffItem.Id == 0)
				LogOffItems.Remove(logOffItem);
			else
				logOffItem.Delete();

			if (ExtraOrder != null)
			{
				var extraOrderItems = LogOffItems.OfType<ExtraOrderLogoff>().Count(i => !i.IsDeleted);
				if (extraOrderItems == 0)
				{
					if (ExtraOrder.IsNew)
						ExtraOrder = null;
					else
						ExtraOrder.Delete();
				}
			}
			RecalculateExtraOrderTotal();
		}

        public void DeleteTask(Task task)
        {
            if (task == null) return;

            //TODO: This has to be fixed on StoreItem (save IsDeleted)
            //When Offline => task will contain LocalId (means IsNew == false)
            //so, we explicitly check the 0 on Id 
            if (task.IsNew || task.Id == 0) 
                Tasks.Remove(task);
            else
                task.Delete();
        }

        public void DeleteRma(Rma rmaItem)
		{
			if (rmaItem == null) return;

            //When Offline => rma will contain LocalId (means IsNew == false)
            //so, we explicitly check the 0 on Id 
            if (rmaItem.IsNew || rmaItem.Id == 0)
				Rmas.Remove(rmaItem);
			else
				rmaItem.Delete();
		}

		public void RecalculateExtraOrderTotal()
		{
			if (ExtraOrder != null)
				ExtraOrder.TotalPrice = LogOffItems.OfType<ExtraOrderLogoff>().Sum(l => l.TotalPrice);
		}

		public void RefreshState()
		{
			OnPropertyChanged(() => WindowStart,
							  () => WindowEnd);
			OnPropertyChanged(() => PlannedStart,
							  () => PlannedEnd,
							  () => EffectiveStart,
							  () => EffectiveEnd);
			OnPropertyChanged(() => Technician);
			OnPropertyChanged(() => DisplayStatus,
							  () => DisplayVisitLine1,
							  () => DisplayVisitLine2);
			OnPropertyChanged(() => EffectiveStart,
							  () => EffectiveEnd);
			OnPropertyChanged(() => VisitHistories);
			OnPropertyChanged(() => VisitRemarks);

			OnPropertyChanged(() => LogOffItems);
			OnPropertyChanged(() => Rmas);
		}
		
		public override void CancelEdit()
		{
			base.CancelEdit();

			//Check if there are any VisitHistories added
			var historiesToRemove = VisitHistories.Where(vh => vh.IsNew).ToList();
			if (historiesToRemove.Any())
				foreach (var visitHistory in historiesToRemove)
				{
					VisitHistories.Remove(visitHistory);
				}

			//Check if there are any VisitRemarks added
			var remarksToRemove = VisitRemarks.Where(vr => vr.IsNew).ToList();
			if (remarksToRemove.Any())
				foreach (var visitRemark in remarksToRemove)
				{
					VisitRemarks.Remove(visitRemark);
				}
		}

		/// <summary>
		/// For Mobile app => set the LocalId for added entities (those that have their id set to 0)
		/// </summary>
		public override void ApplyLocalId()
		{
			if (Id == 0 && LocalId == Guid.Empty) LocalId = Guid.NewGuid();
			foreach (var visitRemark in VisitRemarks)
			{
				visitRemark.ApplyLocalId();
			}

		    Signature?.ApplyLocalId();

		    foreach (var visitHistory in VisitHistories)
			{
				visitHistory.ApplyLocalId();
			}
			foreach (var task in Tasks)
			{
				task.ApplyLocalId();
			}
		    foreach (var logOffItem in LogOffItems)
		    {
		        logOffItem.ApplyLocalId();
		    }
		    foreach (var rma in Rmas)
		    {
		        rma.ApplyLocalId();
		    }
		}

	    public void UpdateStatus(UserRole userRole)
	    {
	        if (userRole == null) 
                throw new ArgumentNullException(nameof(userRole));

            #region check transition Unallocated => Allocated
            if (Status == VisitStatusType.Unallocated)
	        {
	            if (PlannedStart != null && Technician != null)    
                    ChangeStatusTo(VisitStatusType.Allocated, userRole);
	        }
            #endregion

            #region check transition Allocated => Committed
            if (Status == VisitStatusType.Allocated)
	        {
	            if (OverrideType.HasFlag(PlanningOverrideType.Technician) &&
                    OverrideType.HasFlag(PlanningOverrideType.Technician))    
                    ChangeStatusTo(VisitStatusType.Committed, userRole);
	        }
            #endregion
        }

        public void ChangeStatusTo(VisitStatusType status, UserRole currentUserRole)
        {
            var fromState = Status;
            Status = status;
            if (currentUserRole == null) return;
            var visitHistory = VisitHistory.Create(this, fromState, currentUserRole);
            if (visitHistory != null) VisitHistories.Add(visitHistory);
        }
        #endregion
    }
}