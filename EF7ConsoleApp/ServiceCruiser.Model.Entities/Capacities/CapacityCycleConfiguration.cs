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
using ServiceCruiser.Model.Entities.Extensions;
using ServiceCruiser.Model.Entities.Repositories;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ValidationResult = ServiceCruiser.Model.Validations.Core.ValidationResult;

namespace ServiceCruiser.Model.Entities.Capacities
{
    [JsonObject(MemberSerialization.OptIn)]
    [HasSelfValidation]
    public class CapacityCycleConfiguration : ValidatedEntity<CapacityCycleConfiguration>
    {
        #region state
        private int _id;
        [JsonProperty]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private string _name;
        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set { SetProperty(value, ref _name, () => Name);  }
        }

        private DateTimeOffset _validFrom;
        [DateTimeOffsetRangeValidator("1900-01-01T00:00:00", RangeBoundaryType.Inclusive, "2014-01-01T00:00:01", RangeBoundaryType.Ignore,
                                      MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "CapacityCycleConfigurationValidFromRequired")]
        [Display(ResourceType = typeof(Translations), Name = "CapacityCycleConfigurationValidFrom")]        
        [JsonProperty]
        public DateTimeOffset ValidFrom
        {
            get {   return _validFrom; }
            set
            {
                if (SetProperty(value, ref _validFrom, () => ValidFrom))
                {
                    OnPropertyChanged(() => DisplayConfiguration);
                }
            }
        }

        private DateTimeOffset? _validTo;
        [ValidatorComposition(CompositionType.Or,
                                MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "CapacityCycleConfigurationValidFromAfterTo")]
        [NotNullValidator(Negated = true, 
                            MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "CapacityCycleConfigurationValidFromAfterTo")]
        [PropertyComparisonValidator("ValidFrom", ComparisonOperator.GreaterThanEqual, 
                                      MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "CapacityCycleConfigurationValidFromAfterTo")]
        [Display(ResourceType = typeof(Translations), Name = "CapacityCycleConfigurationValidTo")]
        [JsonProperty]
        public DateTimeOffset? ValidTo
        {
            get {   return _validTo; }
            set
            {
                if (SetProperty(value, ref _validTo, () => ValidTo))
                {
                    OnPropertyChanged(() => DisplayConfiguration);
                }
            }
        }

        private TimeUnitsType _requestPeriodUnit;
        [JsonProperty]
        public TimeUnitsType RequestPeriodUnit
        {
            get {   return _requestPeriodUnit; }
            set {   SetProperty(value, ref _requestPeriodUnit, () => RequestPeriodUnit);    }
        }


        private int _requestPeriodLength;
        [JsonProperty]
        public int RequestPeriodLength
        {
            get {   return _requestPeriodLength; }
            set {   SetProperty(value, ref _requestPeriodLength, () => RequestPeriodLength);    }
        }

        private TimeUnitsType _executionPeriodUnit;
        [JsonProperty]
        public TimeUnitsType ExecutionPeriodUnit
        {
            get {   return _executionPeriodUnit; }
            set {   SetProperty(value, ref _executionPeriodUnit, () => ExecutionPeriodUnit);    }
        }


        private int _executionPeriodLength;
        [JsonProperty]
        public int ExecutionPeriodLength
        {
            get {   return _executionPeriodLength; }
            set {   SetProperty(value, ref _executionPeriodLength, () => ExecutionPeriodLength);    }
        }

        private TimeUnitsType _provisioningPeriodUnit;
        [JsonProperty]
        public TimeUnitsType ProvisioningPeriodUnit
        {
            get {   return _provisioningPeriodUnit; }
            set {   SetProperty(value, ref _provisioningPeriodUnit, () => ProvisioningPeriodUnit);  }
        }
        
        private int _provisioningPeriodLength;
        [JsonProperty]
        public int ProvisioningPeriodLength
        {
            get {   return _provisioningPeriodLength; }
            set {   SetProperty(value, ref _provisioningPeriodLength, () => ProvisioningPeriodLength);  }
        }


        private TimeUnitsType _appointmentBookingPeriodUnit;
        [JsonProperty]
        public TimeUnitsType AppointmentBookingPeriodUnit
        {
            get { return _appointmentBookingPeriodUnit; }
            set { SetProperty(value, ref _appointmentBookingPeriodUnit, () => AppointmentBookingPeriodUnit); }
        }

        private int _appointmentBookingPeriodLength;
        [JsonProperty]
        public int AppointmentBookingPeriodLength
        {
            get { return _appointmentBookingPeriodLength; }
            set { SetProperty(value, ref _appointmentBookingPeriodLength, () => AppointmentBookingPeriodLength); }
        }

        private string _remark;
        [JsonProperty]
        public string Remark
        {
            get {   return _remark; }
            set {   SetProperty(value, ref _remark, () => Remark);  }
        }

        private int _contractModelId;
        [JsonProperty]
        public int ContractModelId
        {
            get {   return _contractModelId; }
            set {   SetProperty(value, ref _contractModelId, () => ContractModelId);    }
        }

        private ContractModel _contractModel;
        [JsonProperty, Aggregation]
        public ContractModel ContractModel
        {
            get
            {
                if (_contractModel != null || !UseRepositoryFinder) return _contractModel;

                var repository = RepositoryFinder.GetRepository<IContractModelRepository>() as IContractModelRepository;
                return repository?.Filter(cm => cm.Id == ContractModelId).FirstOrDefault();
            }
            set
            {
                _contractModel = value;
                OnPropertyChanged(() => ContractModel);
            }
        }

        private int? _requestorId;
        [JsonProperty]
        public int? RequestorId
        {
            get {   return _requestorId; }
            set {   SetProperty(value, ref _requestorId, () => RequestorId);    }
        }

        private User _requestor;
        [JsonProperty, Aggregation]
        public User Requestor
        {
            get {   return _requestor; }
            set 
            {   
                _requestor = value;
                OnPropertyChanged(() => Requestor);
            }
        }

        private ObservableCollection<CapacityCycle> _capacityCycles = new ObservableCollection<CapacityCycle>();
        [JsonProperty, Aggregation]
        public ICollection<CapacityCycle> CapacityCycles
        {
            get { return _capacityCycles; }
            set { _capacityCycles = value?.ToObservableCollection(); }
        }
       
        private bool _areDatesLocked;
        public bool AreDatesLocked
        {
            get { return _areDatesLocked; }
            set
            {
                _areDatesLocked = value;
                OnPropertyChanged(() => AreDatesLocked);
            }
        }
        [JsonProperty] [IgnoreOnMap]
        public bool HasCapacityCycles { get; set; }
        [JsonProperty] [IgnoreOnMap]
        public bool HasActiveCapacityCycles { get; set; }
        
        #region request period

        private TimeUnit _requestPeriod;
        [IgnoreOnSave]
        public TimeUnit RequestPeriod
        {
            get { return _requestPeriod = _requestPeriod ?? new TimeUnit(RequestPeriodUnit, RequestPeriodLength); }
            set
            {
                _requestPeriod = value;
                if (_requestPeriod != null)
                {
                    RequestPeriodUnit = _requestPeriod.Unit;
                    RequestPeriodLength = _requestPeriod.Length;
                }
            }
        }

        public bool UseDayForRequest
        {
            get { return (RequestPeriodUnit == TimeUnitsType.Day); }
            set
            {
                if (value)
                {
                    RequestPeriodUnit = TimeUnitsType.Day;
                    RequestPeriodLength = minimumLength(TimeUnitsType.Day);
                    OnPropertyChanged(() => MinimumRequestLength,
                                      () => MaximumRequestLength);
                }
            }
        }

        public bool UseWeekForRequest
        {
            get { return (RequestPeriodUnit == TimeUnitsType.Week); }
            set
            {
                if (value)
                {
                    RequestPeriodUnit = TimeUnitsType.Week;
                    RequestPeriodLength = minimumLength(TimeUnitsType.Week);
                    OnPropertyChanged(() => MinimumRequestLength,
                                      () => MaximumRequestLength);
                }
            }
        }

        public bool UseMonthForRequest
        {
            get { return (RequestPeriodUnit == TimeUnitsType.Month); }
            set
            {
                if (value)
                {
                    RequestPeriodUnit = TimeUnitsType.Month;
                    RequestPeriodLength = minimumLength(TimeUnitsType.Month);
                    OnPropertyChanged(() => MinimumRequestLength,
                                      () => MaximumRequestLength);
                }
            }
        }

        public int MinimumRequestLength => minimumLength(RequestPeriodUnit);

        public int MaximumRequestLength => maximumLength(RequestPeriodUnit);

        #endregion

        #region provisioning period

        private TimeUnit _provisioningPeriod;
        public TimeUnit ProvisioningPeriod
        {
            get { return _provisioningPeriod = _provisioningPeriod ?? new TimeUnit(ProvisioningPeriodUnit, ProvisioningPeriodLength); }
            set
            {
                _provisioningPeriod = value;
                if (_provisioningPeriod != null)
                {
                    _provisioningPeriodUnit = _provisioningPeriod.Unit;
                    _provisioningPeriodLength = _provisioningPeriod.Length;
                }
            }
        }

        public bool UseDayForProvisioning
        {
            get { return (ProvisioningPeriodUnit == TimeUnitsType.Day); }
            set
            {
                if (value)
                {
                    ProvisioningPeriodUnit = TimeUnitsType.Day;
                    ProvisioningPeriodLength = minimumLength(TimeUnitsType.Day);
                    OnPropertyChanged(() => MinimumProvisioningLength,
                                      () => MaximumProvisioningLength);
                }
            }
        }

        public bool UseWeekForProvisioning
        {
            get { return (ProvisioningPeriodUnit == TimeUnitsType.Week); }
            set
            {
                if (value)
                {
                    ProvisioningPeriodUnit = TimeUnitsType.Week;
                    ProvisioningPeriodLength = minimumLength(TimeUnitsType.Week);
                    OnPropertyChanged(() => MinimumProvisioningLength,
                                      () => MaximumProvisioningLength);
                }
            }
        }

        public bool UseMonthForProvisioning
        {
            get { return (ProvisioningPeriodUnit == TimeUnitsType.Month); }
            set
            {
                if (value)
                {
                    ProvisioningPeriodUnit = TimeUnitsType.Month;
                    ProvisioningPeriodLength = minimumLength(TimeUnitsType.Month);
                    OnPropertyChanged(() => MinimumProvisioningLength,
                                      () => MaximumProvisioningLength);
                }
            }
        }

        public int MinimumProvisioningLength => minimumLength(ProvisioningPeriodUnit);

        public int MaximumProvisioningLength => maximumLength(ProvisioningPeriodUnit);

        #endregion

        #region execution period

        private TimeUnit _executionPeriod;
        public TimeUnit ExecutionPeriod
        {
            get { return _executionPeriod = _executionPeriod ?? new TimeUnit(ExecutionPeriodUnit, ExecutionPeriodLength); }
            set
            {
                _executionPeriod = value;
                if (_executionPeriod != null)
                {
                    ExecutionPeriodUnit = _executionPeriod.Unit;
                    ExecutionPeriodLength = _executionPeriod.Length;
                }
            }
        }

        public bool UseDayForExecution
        {
            get { return (ExecutionPeriodUnit == TimeUnitsType.Day); }
            set
            {
                if (value)
                {
                    ExecutionPeriodUnit = TimeUnitsType.Day;
                    ExecutionPeriodLength = minimumLength(TimeUnitsType.Day);
                    OnPropertyChanged(() => MinimumExecutionLength,
                                      () => MaximumExecutionLength);
                }
            }
        }

        public bool UseWeekForExecution
        {
            get { return (ExecutionPeriodUnit == TimeUnitsType.Week); }
            set
            {
                if (value)
                {
                    ExecutionPeriodUnit = TimeUnitsType.Week;
                    ExecutionPeriodLength = minimumLength(TimeUnitsType.Week);
                    OnPropertyChanged(() => MinimumExecutionLength,
                                      () => MaximumExecutionLength);
                }
            }
        }

        public bool UseMonthForExecution
        {
            get { return (ExecutionPeriodUnit == TimeUnitsType.Month); }
            set
            {
                if (value)
                {
                    ExecutionPeriodUnit = TimeUnitsType.Month;
                    ExecutionPeriodLength = minimumLength(TimeUnitsType.Month);
                    OnPropertyChanged(() => MinimumExecutionLength,
                                      () => MaximumExecutionLength);
                }
            }
        }

        public int MinimumExecutionLength => minimumLength(ExecutionPeriodUnit);

        public int MaximumExecutionLength => maximumLength(ExecutionPeriodUnit);

        #endregion


        #region appointment booking period

        private TimeUnit _appointmentBookingPeriod;
        public TimeUnit AppointmentBookingPeriod
        {
            get { return _appointmentBookingPeriod = _appointmentBookingPeriod ?? new TimeUnit(AppointmentBookingPeriodUnit, AppointmentBookingPeriodLength); }
            set
            {
                _appointmentBookingPeriod = value;
                if (_appointmentBookingPeriod != null)
                {
                    AppointmentBookingPeriodUnit = _appointmentBookingPeriod.Unit;
                    AppointmentBookingPeriodLength = _appointmentBookingPeriod.Length;
                }
            }
        }

        public bool UseDayForAppointmentBooking
        {
            get { return (AppointmentBookingPeriodUnit == TimeUnitsType.Day); }
            set
            {
                if (value)
                {
                    AppointmentBookingPeriodUnit = TimeUnitsType.Day;
                    AppointmentBookingPeriodLength = minimumLength(TimeUnitsType.Day);
                    OnPropertyChanged(() => MinimumAppointmentBookingLength,
                                      () => MaximumAppointmentBookingLength);
                }
            }
        }

        public bool UseWeekForAppointmentBooking
        {
            get { return (AppointmentBookingPeriodUnit == TimeUnitsType.Week); }
            set
            {
                if (value)
                {
                    AppointmentBookingPeriodUnit = TimeUnitsType.Week;
                    AppointmentBookingPeriodLength = minimumLength(TimeUnitsType.Week);
                    OnPropertyChanged(() => MinimumAppointmentBookingLength,
                                      () => MaximumAppointmentBookingLength);
                }
            }
        }

        public bool UseMonthForAppointmentBooking
        {
            get { return (AppointmentBookingPeriodUnit == TimeUnitsType.Month); }
            set
            {
                if (value)
                {
                    AppointmentBookingPeriodUnit = TimeUnitsType.Month;
                    AppointmentBookingPeriodLength = minimumLength(TimeUnitsType.Month);
                    OnPropertyChanged(() => MinimumAppointmentBookingLength,
                                      () => MaximumAppointmentBookingLength);
                }
            }
        }

        public int MinimumAppointmentBookingLength => minimumLength(AppointmentBookingPeriodUnit);

        public int MaximumAppointmentBookingLength => maximumLength(AppointmentBookingPeriodUnit);

        #endregion
        
        public string DisplayConfiguration =>
            $"Config valid from {(ValidFrom != DateTimeOffset.MinValue ? ValidFrom.ToString("dd/MM/yyyy") : "?")} to {(ValidTo != null ? ((DateTimeOffset) ValidTo).ToString("dd/MM/yyyy") : "?")}";

        public string DisplayRequestor => $"Requested by {(Requestor != null ? Requestor.DisplayName : "?")}";

        #endregion

        #region behavior

        [SelfValidation]
        public void CheckExecutionLength(ValidationResults results)
        {
            const string errorField = "ExecutionPeriodLength";

            ClearValidationError(errorField);
            
            if (!IsPropertyChanged(() => ExecutionPeriodUnit) &&
                !IsPropertyChanged(() => ExecutionPeriodLength)) return;
            if (ValidTo == null) return;

            var executionDate = ValidFrom;
            executionDate = GetCalculatedDate(executionDate, ExecutionPeriodUnit, ExecutionPeriodLength);
            if (executionDate > ValidTo)
            {
                var validationResult = new ValidationResult(Translations.CapacityCycleConfigurationExecutionPeriodInvalid,
                                                            this, errorField, null, EntityValidator);

                results.AddResult(validationResult);
                SetValidationError(validationResult);   
            }

        }

        [SelfValidation]
        public void CheckAppointmentBookingPeriod(ValidationResults results)
        {
            const string errorField = "AppointmentBookingPeriodLength";

            ClearValidationError(errorField);

            if (!IsPropertyChanged(() => ProvisioningPeriodUnit) &&
                !IsPropertyChanged(() => ProvisioningPeriodLength) &&
                !IsPropertyChanged(() => AppointmentBookingPeriodUnit) &&
                !IsPropertyChanged(() => AppointmentBookingPeriodLength)) return;

            var date = DateTime.Today;
            var provisioningDate = GetCalculatedDate(date, ProvisioningPeriodUnit, ProvisioningPeriodLength);
            provisioningDate = GetCalculatedDate(provisioningDate, ExecutionPeriodUnit, ExecutionPeriodLength);
            var appointmentDate = GetCalculatedDate(date, AppointmentBookingPeriodUnit, AppointmentBookingPeriodLength);
            
            if (appointmentDate > provisioningDate)
            {
                var validationResult = new ValidationResult(Translations.CapacityCycleConfigurationAppointmentBookingPeriodInvalid,
                                                            this, errorField, null, EntityValidator);

                results.AddResult(validationResult);
                SetValidationError(validationResult);   
            }
        }

        public void LockConfiguration(IEnumerable<CapacityCycleConfiguration> configurations)
        {
            if (configurations == null) return;
            if (configurations.Count() == 1) return;
            
            DateTimeOffset? maxDate = DateTimeOffset.MaxValue;
            if (configurations.All(c => c.ValidTo != null))
                maxDate = configurations.Max(c => c.ValidTo);

            AreDatesLocked = (ValidTo != null && ValidTo != maxDate);
        }

        public static CapacityCycleConfiguration Create(ContractModel contractModel)
        {
            if (contractModel == null) return null;

            var configuration = new CapacityCycleConfiguration
            {
                ContractModelId = contractModel.Id,
                
            };
            var user = configuration.GetCurrentUser(RepositoryFinder);
            if (user == null) return configuration;

            configuration.RequestorId = user.Id;
            configuration.Requestor = user;
            configuration.SetAuditInfo(user.Login);

            return configuration;
        }

        private DateTimeOffset GetCalculatedDate(DateTimeOffset date, TimeUnitsType timeUnitsType, int timeLength)
        {
            switch (timeUnitsType)
            {
                case TimeUnitsType.Day:
                    return date.AddDays(timeLength);
                case TimeUnitsType.Week:
                    return date.AddDays(timeLength*7);
                case TimeUnitsType.Month:
                    return date.AddMonths(timeLength);
            }
            return date;
        }

        private int maximumLength(TimeUnitsType periodUnit)
        {
            switch (periodUnit)
            {
                case TimeUnitsType.Day:
                    return 365;
                case TimeUnitsType.Week:
                    return 52;
                case TimeUnitsType.Month:
                    return 12;
            }
            return 0;
        }

        private int minimumLength(TimeUnitsType periodUnit)
        {
            switch (periodUnit)
            {
                case TimeUnitsType.Day:
                case TimeUnitsType.Week:
                case TimeUnitsType.Month:
                    return 1;
            }
            return 0;
        }

        #endregion

    }
}
