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
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ValidationResult = ServiceCruiser.Model.Validations.Core.ValidationResult;
using ServiceCruiser.Model.Entities.Extensions;

namespace ServiceCruiser.Model.Entities.Capacities
{
    [JsonObject(MemberSerialization.OptIn)]
    [HasSelfValidation]
    public class CapacityCycle : ValidatedEntity<CapacityCycle>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private DateTimeOffset _executionStart;
        [DateTimeOffsetRangeValidator("2014-01-01T00:00:00", "2214-01-01T00:00:01",
                                      MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "CapacityCycleConfigurationValidFromRequired")]
        [Display(ResourceType = typeof(Translations), Name = "CapacityCycleExecutionStart")]
        [JsonProperty]
        public DateTimeOffset ExecutionStart
        {
            get {   return _executionStart; }
            set
            {
                if (SetProperty(value, ref _executionStart, () => ExecutionStart))
                {
                    SetPhaseTimes();

                    OnPropertyChanged(() => DisplayAgree,
                                      () => DisplayProvision,
                                      () => DisplayAppointmentBooking,
                                      () => DisplayExecution);
                    OnPropertyChanged(() => IsExecutionActive);
                }
            }
        }

        private DateTimeOffset _requestedAt;
        [JsonProperty]
        public DateTimeOffset RequestedAt
        {
            get { return _requestedAt; }
            set {   SetProperty(value, ref _requestedAt, () => RequestedAt);    }
        }

        private ObservableCollection<CapacityRequest> _capacityRequests = new ObservableCollection<CapacityRequest>();
        [HandleOnNesting] [Aggregation(isComposite:true)]
        [ObjectCollectionValidator(typeof(CapacityRequest))]
        [JsonProperty]
        public ICollection<CapacityRequest> CapacityRequests
        {
            get { return _capacityRequests; }
            set { _capacityRequests = value?.ToObservableCollection(); }
        }

        private int _contractModelId;
        [JsonProperty]
        public int ContractModelId
        {
            get {   return _contractModelId; }
            set {   SetProperty(value, ref _contractModelId, () => ContractModelId);    }
        }

        private ContractModel _contractModel;
        [HandleOnNesting, Aggregation]
        [JsonProperty]
        public ContractModel ContractModel
        {
            get { return _contractModel; }
            set 
            {
                _contractModel = value;
                OnPropertyChanged(() => ContractModel);
            }
        }

        private int _capacityCycleConfigurationId;
        [JsonProperty]
        public int CapacityCycleConfigurationId
        {
            get {   return _capacityCycleConfigurationId; }
            set {   SetProperty(value, ref _capacityCycleConfigurationId, () => CapacityCycleConfigurationId);  }
        }

        private CapacityCycleConfiguration _capacityCycleConfiguration;
        [JsonProperty, Aggregation]
        public CapacityCycleConfiguration CapacityCycleConfiguration
        {
            get { return _capacityCycleConfiguration; }
            set {   
                _capacityCycleConfiguration = value;
                
                SetPhaseTimes();

                OnPropertyChanged(() => DisplayAgree,
                                  () => DisplayProvision,
                                  () => DisplayAppointmentBooking,
                                  () => DisplayExecution);
                OnPropertyChanged(() => CapacityCycleConfiguration);
            }
        }
        
        private int _userId;
        [JsonProperty]
        public int UserId
        {
            get {   return _userId; }
            set {   SetProperty(value, ref _userId, () => UserId);  }
        }

        private User _user;
        [HandleOnNesting, Aggregation]
        [JsonProperty]
        public User User
        {
            get { return _user; }
            set 
            {
                _user = value;
                OnPropertyChanged(() => User);
            }
        }
       
        public CapacityVersionType Version
        {
            get
            {
                if (CapacityRequests.Any())
                {
                    if (CapacityRequests.All(cr => cr.Version == CapacityVersionType.Final))
                        return CapacityVersionType.Final;
                    if (CapacityRequests.All(cr => cr.Version == CapacityVersionType.Draft))
                        return CapacityVersionType.Draft;
                    if (CapacityRequests.All(cr => cr.Version == CapacityVersionType.Draft ||
                                                   cr.Version == CapacityVersionType.Final))
                        return CapacityVersionType.DraftFinal;
                    return CapacityVersionType.New;
                }
                return IsNew ? CapacityVersionType.New : CapacityVersionType.Draft;
            }
        }
        
        public bool IsLocked => (Version == CapacityVersionType.Final);

        public List<CapacityCycle> OtherCapacityCyles { get; set; }

        public ICollection<ContractorCompany> SelectedContractors { get; set; }

        private DateTimeOffset _requestStart;
        public DateTimeOffset RequestStart
        {
            get { return _requestStart; }
            set
            {
                _requestStart = value;
                OnPropertyChanged(() => RequestStart);
                OnPropertyChanged(() => IsRequestActive);
            }
        }
        
        private DateTimeOffset _requestEnd;
        public DateTimeOffset RequestEnd
        {
            get { return _requestEnd; }
            set
            {
                _requestEnd = value;
                OnPropertyChanged(() => RequestEnd);
                OnPropertyChanged(() => IsRequestActive);
            }
        }

        public bool IsRequestActive => (RequestStart <= DateTime.Today &&
                                        DateTime.Today <= RequestEnd);

        private DateTimeOffset _provisioningStart;
        public DateTimeOffset ProvisioningStart
        {
            get { return _provisioningStart; }
            set
            {
                _provisioningStart = value;
                OnPropertyChanged(() => ProvisioningStart);
                OnPropertyChanged(() => IsProvisioningActive);
            }
        }

        private DateTimeOffset _provisioningEnd;
        public DateTimeOffset ProvisioningEnd
        {
            get { return _provisioningEnd; }
            set
            {
                _provisioningEnd = value;
                OnPropertyChanged(() => ProvisioningEnd);
                OnPropertyChanged(() => IsProvisioningActive);
            }
        }

        private DateTimeOffset _appointmentBookingStart;
        public DateTimeOffset AppointmentBookingStart
        {
            get { return _appointmentBookingStart; }
            set
            {
                _appointmentBookingStart = value;
                OnPropertyChanged(() => AppointmentBookingStart);
            }
        }

        public DateTimeOffset AppointmentBookingEnd => ExecutionEnd;

        public bool IsProvisioningActive => (ProvisioningStart <= DateTime.Today &&
                                             DateTime.Today <= ProvisioningEnd);

        private DateTimeOffset _executionEnd;
        public DateTimeOffset ExecutionEnd
        {
            get { return _executionEnd; }
            set
            {
                _executionEnd = value;
                OnPropertyChanged(() => ExecutionEnd);
                OnPropertyChanged(() => AppointmentBookingEnd);
                OnPropertyChanged(() => IsExecutionActive);
            }
        }

        public bool IsExecutionActive => (ExecutionStart <= DateTime.Today &&
                                          DateTime.Today <= ExecutionEnd);

        private double _meanHoursTechnicians;
        public double MeanHoursTechnicians
        {
            set
            {
                _meanHoursTechnicians = value;
                OnPropertyChanged(() => MeanHoursTechnicians);
            }
            get { return _meanHoursTechnicians; }
        }

        public string DisplayVersion
        {
            get
            {
                if (IsNew) return "-";

                return $" [{StaticFactory.Instance.GetValue(CodeGroupType.CapacityVersion, Version.ToString()) ?? "?"}]";
            }
        }

        public string DisplayAgree => $"{(RequestStart.Date != DateTime.MinValue ? RequestStart.ToString("d") : "?")} - {(RequestEnd.Date != DateTime.MinValue ? RequestEnd.ToString("d") : "?")}";

        public string DisplayProvision => $"{(ProvisioningStart.Date != DateTime.MinValue ? ProvisioningStart.ToString("d") : "?")} - {(ProvisioningEnd.Date != DateTime.MinValue ? ProvisioningEnd.ToString("d") : "?")}";

        public string DisplayExecution => $"{(ExecutionStart.Date != DateTime.MinValue ? ExecutionStart.ToString("d") : "?")} - {(ExecutionEnd.Date != DateTime.MinValue ? ExecutionEnd.ToString("d") : "?")}";

        public string DisplayAppointmentBooking => $"{(AppointmentBookingStart.Date != DateTime.MinValue ? AppointmentBookingStart.ToString("d") : "?")} - {(AppointmentBookingEnd.Date != DateTime.MinValue ? AppointmentBookingEnd.ToString("d") : "?")}";

        public string DisplayRequestorAndTime
        {
            get
            {
                if (User == null) return string.Empty;
                return $"Requested by {User.DisplayName} at {RequestedAt.ToString("d")}";
            }
        }

        public string DisplayRegion
        {
            get
            {
                if (CapacityRequests == null || !CapacityRequests.Any()) return string.Empty;

                return CapacityRequests.FirstOrDefault()?.Region.Name;
            }    
        }
        
        private bool _canPickPossibleConfiguration;
        public bool CanPickPossibleConfiguration
        {
            get
            {
                if (!IsNew) return false;
                return _canPickPossibleConfiguration;
            }
            set
            {
                _canPickPossibleConfiguration = value;
                OnPropertyChanged(() => CanPickPossibleConfiguration);
            }
        }

        #endregion

        #region behavior

        [SelfValidation]
        public void CheckCapacityCycles(ValidationResults results)
        {
            if (ContractModel?.AreOverlappingsAllowed ?? false) return;

            if (OtherCapacityCyles == null ||
                !OtherCapacityCyles.Any()) return;
            
            bool hasOverlap = false;
            foreach(var capacityCycle in OtherCapacityCyles)
            {
                if (Equals(capacityCycle)) continue;
                hasOverlap = (capacityCycle.ExecutionStart <= ExecutionStart &&
                              capacityCycle.ExecutionEnd >= ExecutionStart) ||
                             (capacityCycle.ExecutionStart <= ExecutionEnd &&
                              capacityCycle.ExecutionEnd >= ExecutionEnd);
                if (hasOverlap) break;
            }
            if (hasOverlap)
            {
                var errorMessage = string.Format(Translations.CapacityCycleExecutionOverlap, ExecutionStart.ToString("d"));
                var validationResult = new ValidationResult(errorMessage, this, "ExecutionStart", null, EntityValidator);
            
                results.AddResult(validationResult);
                SetValidationError(validationResult);
            }
            ClearValidationErrors();
        }

        [SelfValidation]
        public void CheckActiveConfiguration(ValidationResults results)
        {
            if (CapacityCycleConfiguration == null)
            {
                var errorMessage = string.Format(Translations.CapacityCycleNoActiveConfiguration, ExecutionStart.ToString("d"));

                var validationResult = new ValidationResult(errorMessage, this, "ExecutionStart", null, EntityValidator);

                results.AddResult(validationResult);
                SetValidationError(validationResult);
            }
            ClearValidationErrors();
        }

        public static CapacityCycle Create(ContractModel contractModel)
        {
            if (contractModel == null) return null;

            var capacityCycle = new CapacityCycle
            {
                RequestedAt = DateTime.Now,
                ContractModelId = contractModel.Id,
                ContractModel = contractModel,
            };
            capacityCycle.User = capacityCycle.GetCurrentUser(RepositoryFinder);
            if (capacityCycle.User != null)
            {
                capacityCycle.UserId = capacityCycle.User.Id;
                capacityCycle.SetAuditInfo(capacityCycle.User.Login);
            }
            return capacityCycle;
        }
        
        public void SetPhaseTimes()
        {
            if (CapacityCycleConfiguration == null ||
                ExecutionStart.Date == DateTime.MinValue)
            {
                InitPeriods();
                return;
            } 

            // execution period - inclusive enddate
            ExecutionEnd = CalculatePhaseTimes(CapacityCycleConfiguration.ExecutionPeriod, ExecutionStart).AddDays(-1);

            // provisioning period
            ProvisioningEnd = ExecutionStart.Date.AddDays(-1);
            ProvisioningStart = CalculatePhaseTimes(CapacityCycleConfiguration.ProvisioningPeriod, ProvisioningEnd, false).AddDays(1);

            // appointment booking period
            AppointmentBookingStart = CalculatePhaseTimes(CapacityCycleConfiguration.AppointmentBookingPeriod, ExecutionEnd, false).AddDays(1);

            // request period
            RequestEnd = ProvisioningStart.AddDays(-1);
            RequestStart = CalculatePhaseTimes(CapacityCycleConfiguration.RequestPeriod, RequestEnd, false).AddDays(1);
        }

        private DateTimeOffset CalculatePhaseTimes(TimeUnit timeUnit, DateTimeOffset? startTime = null, bool doForward = true)
        {
            int length = timeUnit.Length;
            if (!doForward) length *= -1;

            switch (timeUnit.Unit)
            {
            case TimeUnitsType.Day:
                return startTime?.AddDays(length) ?? ExecutionStart.AddDays(length);
            case TimeUnitsType.Week:
                return startTime?.AddDays(length * 7) ?? ExecutionStart.AddDays(length * 7);
            case TimeUnitsType.Month:
                return startTime?.AddMonths(length) ?? ExecutionStart.AddMonths(length);
            }
            return DateTime.Now;
        }
        
        public void RefreshState()
        {
            OnPropertyChanged(() => DisplayVersion);
            OnPropertyChanged(() => Version);
        }
        public void Enhance(CapacityRequest capacityRequest)
        {
            if (capacityRequest == null) return;

            var oriCapacityRequest = CapacityRequests.FirstOrDefault(cr => cr.Id == capacityRequest.Id);
            if (oriCapacityRequest != null)
                CapacityRequests.Remove(oriCapacityRequest);
            CapacityRequests.Add(capacityRequest);

            RefreshState();
        }

        private void InitPeriods()
        {
            ExecutionEnd = ProvisioningEnd = ProvisioningStart = AppointmentBookingStart = RequestEnd = RequestStart = DateTimeOffset.MinValue; 
        }
        #endregion
    }
}
