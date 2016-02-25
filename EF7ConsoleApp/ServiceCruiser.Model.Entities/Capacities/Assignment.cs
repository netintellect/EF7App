using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod;
using ServiceCruiser.Model.Entities.Extensions;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Technicians;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Capacities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Assignment : ValidatedEntity<Assignment>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private TimeSpan _from;
        [TimeSpanRangeValidator("00:00:00", RangeBoundaryType.Inclusive, "00:00:01", RangeBoundaryType.Ignore,
                                MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "AssignmentFromRequired")]
        [Display(ResourceType = typeof(Translations), Name = "AssignmentFrom")]        
        [JsonProperty]
        public TimeSpan From
        {
            get {   return _from; }
            set
            {

                if (_originalFrom == null) _originalFrom = value;

                if (SetProperty(value, ref _from, () => From))
                {
                    OnPropertyChanged(() => ContractShiftOverlap);
                    OnPropertyChanged(() => ResourceShiftOverlap);
                    OnPropertyChanged(() => Duration);
                    OnPropertyChanged(() => ContractShiftOverlap);
                    OnPropertyChanged(() => ResourceShiftOverlap);
                    OnPropertyChanged(() => DisplayFrom);
                }
            }
        }

        private TimeSpan _until;
        [TimeSpanRangeValidator("00:00:00", RangeBoundaryType.Inclusive, "00:00:01", RangeBoundaryType.Ignore,
                                MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "AssignmentUntilRequired")]
        [PropertyComparisonValidator("From", ComparisonOperator.GreaterThanEqual,
                                MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "AssignmentFromAfterTo")]
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "AssignmentUntil")]
        public TimeSpan Until
        {
            get {   return _until; }
            set
            {
                if (_originalUntil == null) _originalUntil = value;

                if (SetProperty(value, ref _until, () => Until))
                {
                    OnPropertyChanged(() => ContractShiftOverlap);
                    OnPropertyChanged(() => ResourceShiftOverlap);
                    OnPropertyChanged(() => Duration);
                    OnPropertyChanged(() => ContractShiftOverlap);
                    OnPropertyChanged(() => ResourceShiftOverlap);
                    OnPropertyChanged(() => DisplayUntil);
                }
            }
        }

        private int _workDayId;
        [JsonProperty]
        public int WorkDayId
        {
            get {   return _workDayId; }
            set {   SetProperty(value, ref _workDayId, () => WorkDayId);    }
        }

        private WorkDay _workDay;
        [Aggregation(isComposite: false)]
        [JsonProperty]
        public WorkDay WorkDay
        {
            get { return _workDay; }
            set 
            {
                _workDay = value;
                OnPropertyChanged(() => WorkDay);
            }
        }

        private int _technicianRoleId;
        [JsonProperty]
        public int TechnicianRoleId
        {
            get {   return _technicianRoleId; }
            set {   SetProperty(value, ref _technicianRoleId, () => TechnicianRoleId);  }
        }

        private TechnicianRole _technicianRole;
        [Aggregation(isComposite: false)]
        [JsonProperty]
        public TechnicianRole TechnicianRole
        {
            get { return _technicianRole; }
            set
            {
                _technicianRole = value;
                OnPropertyChanged(() => TechnicianRole);
            }
        }

        private int? _capacityDistributionId;
        [JsonProperty]
        public int? CapacityDistributionId
        {
            get {   return _capacityDistributionId; }
            set {   SetProperty(value, ref _capacityDistributionId, () => CapacityDistributionId);  }
        }

        private CapacityDistribution _capacityDistribution;
        [JsonProperty, Aggregation]
        public CapacityDistribution CapacityDistribution
        {
            get { return _capacityDistribution; }
            set {
                _capacityDistribution = value;
                OnPropertyChanged(() => CapacityDistribution);
            }
        }

        [JsonProperty] [IgnoreOnMap]
        public string ContractModelName { get; set; }
        
        private TimeSpan? _originalFrom;
        private TimeSpan? _originalUntil;
        private ContractShift _contractShift;
        public ContractShift ContractShift
        {
            get { return _contractShift; }
            set
            {
                _contractShift = value;
                SetAssignmentRange();
                OnPropertyChanged(() => ContractShift);
                OnPropertyChanged(() => ContractShiftOverlap);
                OnPropertyChanged(() => ResourceShiftOverlap);
            }
        }
        
        public ITimeRange AssignmentRange
        {
            get { return new TimeRange(From, Until); }
        }

        public double Duration
        {
            get { return  AssignmentRange.Duration.TotalMinutes; }
        }

        public ITimeRange WorkingHoursRange
        {
            get
            {
                if (WorkDay == null) return null;

                // check if the most used break intersects with the
                // assignment, if so subtract
                ITimeRange intersectionRange = AssignmentRange.GetIntersection(WorkDay.MostOftenBreakRange);

                var assignmentRange = new TimeRange(AssignmentRange);
                if (intersectionRange != null)
                    assignmentRange.Duration = assignmentRange.Duration - intersectionRange.Duration;

                return assignmentRange;
            }
        }

        public double WorkingHoursDuration
        {
            get
            {
                if (WorkingHoursRange == null) return 0d;
                
                return WorkingHoursRange.Duration.TotalMinutes;
            }
        }

        public string DisplayFrom
        {
            get {   return string.Format("{0}:{1}", From.Hours.ToString("00"), From.Minutes.ToString("00")); }
        }

        public string DisplayUntil
        {
            get { return string.Format("{0}:{1}", Until.Hours.ToString("00"), Until.Minutes.ToString("00"));  }
        }

        public string DisplayAssignment
        {
            get
            {
                string name = "?";
                if (TechnicianRole != null &&
                    TechnicianRole.Technician != null)
                    name = TechnicianRole.Technician.DisplayName;
                else if (WorkDay != null &&
                         WorkDay.Technician != null)
                    name = WorkDay.Technician.DisplayName;

                return string.Format("assignment of {0}", name);
            }
        }
        private TimeSpan _rangeFrom = new TimeSpan(0,0,0);
        public TimeSpan RangeFrom
        {
            get { return _rangeFrom; }
            set
            {
                _rangeFrom = value;
                OnPropertyChanged(() => RangeFrom);
            }
        }

        private TimeSpan _rangeUntil = new TimeSpan(24, 0, 0);
        public TimeSpan RangeUntil
        {
            get { return _rangeUntil; }
            set
            {
                _rangeUntil = value;
                OnPropertyChanged(() => RangeUntil);
            }
        }

        public double ContractShiftOverlap
        {
            get
            {
                if (ContractShift == null) return 0;
                if (WorkDay == null) return 0;
                
                var assignmentRange = new TimeRange(From, Until);
                var contractWorkDayRange = ContractShift.WorkDayRange;
                var overlapRange = assignmentRange.GetIntersection(contractWorkDayRange);
                
                if (Math.Abs(contractWorkDayRange.Duration.TotalMinutes) > double.Epsilon)
                    return (assignmentRange.Duration.TotalMinutes / contractWorkDayRange.Duration.TotalMinutes) * 100;
                //return (overlapRange.Duration.TotalMinutes / contractWorkDayRange.Duration.TotalMinutes) * 100;

                return 0;
            }
        }

        public double ResourceShiftOverlap
        {
            get
            {
                if (WorkDay == null) return 0;
                if (WorkDay.ResourceShift == null) return 0;

                var assignmentRange = new TimeRange(From, Until);
                var workDayRange = WorkDay.ResourceShift.WorkDayRange;
                var overlapRange = assignmentRange.GetIntersection(workDayRange);

                if (Math.Abs(workDayRange.Duration.TotalMinutes) > double.Epsilon)
                    return (assignmentRange.Duration.TotalMinutes / workDayRange.Duration.TotalMinutes) * 100;
                //return (overlapRange.Duration.TotalMinutes / workDayRange.Duration.TotalMinutes)*100;

                return 0;
            }
        }

        #endregion

        #region behavior

        internal static Assignment Create(TechnicianRole technicianRole, 
                                          WorkDay workDay)
        {
            if (technicianRole == null) return null;
            if (workDay == null) return null;

            var assignment = new Assignment
            {
                WorkDayId = workDay.Id,
                WorkDay = workDay,
                TechnicianRole = technicianRole,
                TechnicianRoleId = technicianRole.Id
            };
            var user = assignment.GetCurrentUser(RepositoryFinder);
            if (user != null) assignment.SetAuditInfo(user.Login);

            return assignment;
        }

        private void SetAssignmentRange()
        {
            if (ContractShift == null) return;
            if (WorkDay == null ||
                WorkDay.ResourceShift == null) return;

            ITimeRange timeRange = ContractShift.WorkDayRange.GetIntersection(WorkDay.ResourceShift.WorkDayRange);
            if (timeRange == null) return;

            RangeFrom = new TimeSpan(timeRange.Start.Hour, timeRange.Start.Minute, 0);
            RangeUntil = new TimeSpan(timeRange.End.Hour, timeRange.End.Minute, 0);
        }

        public double GetProvisioningDuration(ContractShift contractWorkDay)
        {
            if (contractWorkDay == null) throw new ArgumentNullException("contractWorkDay");

            var contractWorkingHoursRange = contractWorkDay.WorkingHoursRange;
            if (contractWorkingHoursRange == null) return 0d;

            var workingHoursRange = WorkingHoursRange;
            if (workingHoursRange == null) return 0d;

            return contractWorkingHoursRange.GetIntersection(workingHoursRange).Duration.TotalMinutes;
        }
        
        public override void EndEdit()
        {
            if (_originalFrom.HasValue)
            {
                From = _originalFrom.Value;
                _originalFrom = null;
            }
            if (_originalUntil.HasValue)
            {
                Until = _originalUntil.Value;
                _originalUntil = null;
            }
            IsDeleted = false;
            base.EndEdit();
        }

        public override void SetUnmodified(AuditEntity baseEntity)
        { 
            _originalFrom = null;
            _originalUntil = null;

            base.SetUnmodified(baseEntity);
        }

        #endregion
    }
}
