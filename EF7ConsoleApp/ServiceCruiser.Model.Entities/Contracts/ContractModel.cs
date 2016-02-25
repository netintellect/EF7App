using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Capacities;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Repositories;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Technicians;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Common.Utility;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ValidationResult = ServiceCruiser.Model.Validations.Core.ValidationResult;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    [HasSelfValidation]
    public class ContractModel : ValidatedEntity<ContractModel>
    {
        #region state
        // temporary solution until this piece of information is persisted
        public bool AreOverlappingsAllowed { get; } = true;

        private int _id;
		[JsonProperty, KeyNew(true)]
		public int Id 
		{ 
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
		}

        private string _name;
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Ignore,
                                MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ContractModelNameRequired")]
        [Display(ResourceType = typeof(Translations), Name = "ContractModelName")]
        [JsonProperty]
		public string Name 
		{ 
            get { return _name; }
            set { SetProperty(value, ref _name, () => Name); }
		}
    			
        private bool _canOverrideOrderTimes;
		[JsonProperty]
		public bool CanOverrideOrderTimes 
		{ 
            get { return _canOverrideOrderTimes; }
            set { SetProperty(value, ref _canOverrideOrderTimes, () => CanOverrideOrderTimes); }
		}

		private bool _isCustomerMandatory;
		[JsonProperty]
		public bool IsCustomerMandatory 
		{ 
            get { return _isCustomerMandatory; }
            set { SetProperty(value, ref _isCustomerMandatory, () => IsCustomerMandatory); }
		}

        [JsonProperty]
        public int DseNrOfDaysToPlan { get; set; }

        private string _country;
        [JsonProperty]
        public string Country
        {
            get { return _country; }
            set { SetProperty(value, ref _country, () => Country); }
        }

        private int _numberOfNextVisitsOnMobile;
        [JsonProperty]
        public int NumberOfNextVisitsOnMobile
        {
            get { return _numberOfNextVisitsOnMobile; }
            set { SetProperty(value, ref _numberOfNextVisitsOnMobile, () => NumberOfNextVisitsOnMobile); }
        }

        private string _timeZoneId;
        [Display(ResourceType = typeof(Translations), Name = "ContractModelTimeZoneId")]
        [JsonProperty]
        public string TimeZoneId
        {
            get { return _timeZoneId; }
            set { SetProperty(value, ref _timeZoneId, () => TimeZoneId); }
        }

        private ObservableCollection<CapacityCycleConfiguration> _capacityCycleConfigurations = new ObservableCollection<CapacityCycleConfiguration>();
        [HandleOnNesting]
        [Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(CapacityCycleConfiguration))]
        [JsonProperty]
		public ICollection<CapacityCycleConfiguration> CapacityCycleConfigurations 
		{
            get { return _capacityCycleConfigurations; }
            set { _capacityCycleConfigurations = value?.ToObservableCollection(); }
		}
        
        private ObservableCollection<ServiceSpecification> _serviceSpecifications = new ObservableCollection<ServiceSpecification>();
        [JsonProperty, HandleOnNesting, Aggregation]
        [ObjectCollectionValidator(typeof(ServiceSpecification))]
        public ICollection<ServiceSpecification> ServiceSpecifications 
		{
            get
            {
                if (_serviceSpecifications.Any() || !UseRepositoryFinder) return _serviceSpecifications;

                var repository = RepositoryFinder.GetRepository<IServiceSpecificationRepository>() as IServiceSpecificationRepository;
                return repository?.Filter(ss => ss.ContractModelId == Id);
            }
            set
            {
                _serviceSpecifications = value?.ToObservableCollection();
            }
		}
            
        private ObservableCollection<AppointmentWindow> _appointmentWindows = new ObservableCollection<AppointmentWindow>();
        [JsonProperty, HandleOnNesting, Aggregation(true)]
        [ObjectCollectionValidator(typeof(ServiceSpecification))]
        public ICollection<AppointmentWindow> AppointmentWindows
        {
            get { return _appointmentWindows; }
            set { _appointmentWindows = value?.ToObservableCollection(); }
        }
            
        private int _contractingId;
		[JsonProperty]
		public int ContractingId 
		{ 
            get { return _contractingId; }
            set { SetProperty(value, ref _contractingId, () => ContractingId); }
		}

        private ContractingCompany _contractingCompany;
        [HandleOnNesting, Aggregation(), ObjectValidator()]
        [JsonProperty]
        public ContractingCompany ContractingCompany
        {
            get { return _contractingCompany; }
            set 
            {   
                _contractingCompany = value;
                OnPropertyChanged(() => ContractingCompany);
            }
        }

        private ObservableCollection<ContractSubscription> _contractSubscriptions = new ObservableCollection<ContractSubscription>();
        [HandleOnNesting]
        [Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(ContractSubscription))]
        [JsonProperty]
        public ICollection<ContractSubscription> ContractSubscriptions
        {
            get { return _contractSubscriptions; }
            set { _contractSubscriptions = value?.ToObservableCollection(); }
        }

        private ObservableCollection<ContractShift> _contractShifts = new ObservableCollection<ContractShift>();
        [HandleOnNesting]
        [Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(ContractShift))]
        [JsonProperty]
        public ICollection<ContractShift> ContractShifts
        {
            get { return _contractShifts; }
            set { _contractShifts = value?.ToObservableCollection(); }
        }

        private ObservableCollection<Region> _regions = new ObservableCollection<Region>();
        [HandleOnNesting]
        [Aggregation(isIndependent: true)]
        [JsonProperty]
        public ICollection<Region> Regions
        {
            get { return _regions; }
            set { _regions = value?.ToObservableCollection(); }
        }

        private ObservableCollection<CapacityCycle> _capacityCycles = new ObservableCollection<CapacityCycle>();
        [JsonProperty]
        public ICollection<CapacityCycle> CapacityCycles
        {
            get { return _capacityCycles; }
            set { _capacityCycles = value?.ToObservableCollection(); }
        }
        
        private ObservableCollection<UserRole> _userRoles = new ObservableCollection<UserRole>();
        [JsonProperty]
        public ICollection<UserRole> UserRoles
        {
            get { return _userRoles; }
            set { _userRoles = value?.ToObservableCollection(); }
        }

        private ObservableCollection<ResultCode> _resultCodes = new ObservableCollection<ResultCode>();
        [JsonProperty]
        public ICollection<ResultCode> ResultCodes
        {
            get { return _resultCodes; }
            set { _resultCodes = value?.ToObservableCollection(); }
        }

        private ObservableCollection<RmaReason> _rmaReasons = new ObservableCollection<RmaReason>();
        [JsonProperty]
        public ICollection<RmaReason> RmaReasons
        {
            get { return _rmaReasons; }
            set { _rmaReasons = value?.ToObservableCollection(); }
        }

        private ObservableCollection<SkillDefinition> _skillDefinitions = new ObservableCollection<SkillDefinition>();
        [JsonProperty]
        public ICollection<SkillDefinition> SkillDefinitions
        {
            get { return _skillDefinitions; }
            set { _skillDefinitions = value?.ToObservableCollection(); }
        }

        private ObservableCollection<Skill> _skills = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> Skills
        {
            get { return _skills; }
            set
            {
                _skills = value;
                OnPropertyChanged(() => Skills);
            }
        }

        public ObservableCollection<ContractSubscription> ActiveContracts
        {
            get 
            { 
                return ContractSubscriptions.Where(cs => cs.ValidFrom <= DateTime.Today &&
                                                        (cs.ValidUntil == null || cs.ValidUntil >= DateTime.Today))
                                            .ToObservableCollection(); 
            }
        }
        #endregion

        #region behavior
        public CapacityCycleConfiguration NewConfiguration(User currentUser)
        {
            var configuration = CapacityCycleConfiguration.Create(this);

            if (CapacityCycleConfigurations != null &&
                CapacityCycleConfigurations.Any())
            {
                // first set the previous period to ended
                CapacityCycleConfiguration previousConfiguration = CapacityCycleConfigurations.FirstOrDefault(m => m.ValidTo == null);
                if (previousConfiguration != null)
                {
                    previousConfiguration.ValidTo = DateTime.Today.AddDays(-1);
                    previousConfiguration.AreDatesLocked = true;

                    configuration.ValidFrom = DateTime.Today;
                }
                else
                {
                    previousConfiguration = CapacityCycleConfigurations.OrderByDescending(m => m.ValidTo)
                                                                       .FirstOrDefault();
                    if (previousConfiguration != null)
                    {
                        previousConfiguration.AreDatesLocked = true;
                        configuration.ValidFrom = previousConfiguration.ValidTo.Value.AddDays(1);
                    }
                    else
                    {
                        configuration.ValidFrom = DateTime.Today;
                    }
                }
            }
            else
            {
                configuration.ValidFrom = DateTime.Today;
            }
            return configuration;
        }

        [SelfValidation]
        public void CheckConfigurations(ValidationResults results)
        {
            if (AreOverlappingsAllowed) return;

            const string errorField = "CapacityCycleConfigurations";
            string errorMessage = Translations.ContractModelConfigurationsInvalid;

            var configurations = CapacityCycleConfigurations.OrderBy(c => c.ValidFrom);

            configurations.ForEach(c => c.ClearValidationError(errorField));

            bool overlaps = false;
            foreach (var configuration in configurations)
            {
                var currentPeriod = new TimeRange(configuration.ValidFrom.LocalDateTime,
                                                  configuration.ValidTo.HasValue ? configuration.ValidTo.GetValueOrDefault().LocalDateTime : DateTime.MaxValue);
                overlaps = configurations.Any(c => !c.Equals(configuration) &&
                                                   currentPeriod.OverlapsWith(new TimeRange(c.ValidFrom.LocalDateTime, 
                                                                                            c.ValidTo.HasValue ? c.ValidTo.GetValueOrDefault().LocalDateTime : DateTime.MaxValue)));
                if (overlaps) break;
            }

            if (overlaps)
            {
                var validationResult = new ValidationResult(errorMessage, this, errorField, null, EntityValidator);

                results.AddResult(validationResult);
                SetValidationError(validationResult);
            }
        }
        #endregion
    }
}
