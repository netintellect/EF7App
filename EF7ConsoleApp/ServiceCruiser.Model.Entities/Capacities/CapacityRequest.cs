using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.Extensions;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Common.Utility;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Capacities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CapacityRequest : ValidatedEntity<CapacityRequest>
    {
        #region state
        private int _id;
		[JsonProperty, KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id;} 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
        private int _cycleId;
		[JsonProperty]
		public int CycleId 
		{ 
 			get {   return _cycleId;} 
 			set {   SetProperty(value,ref _cycleId, () => CycleId); } 
		}

        private int _maxBaseIncreasePercentage;
        [JsonProperty]
        public int MaxBaseIncreasePercentage
        {
            get {   return _maxBaseIncreasePercentage; }
            set {   SetProperty(value, ref _maxBaseIncreasePercentage, () => MaxBaseIncreasePercentage);    }
        }
        
        private int _maxBaseDecreasePercentage;
        [JsonProperty]
        public int MaxBaseDecreasePercentage
        {
            get {   return _maxBaseDecreasePercentage; }
            set {   SetProperty(value, ref _maxBaseDecreasePercentage, () => MaxBaseDecreasePercentage);    }
        }
        
        private CapacityVersionType _version;
        [JsonProperty]
        public CapacityVersionType Version
        {
            get {   return _version; }
            set
            {
                if (SetProperty(value, ref _version, () => Version))
                {
                    OnPropertyChanged(() => IsLocked);
                    OnPropertyChanged(() => DisplayVersion);
                }
            }
        }

        private DateTimeOffset _requestedAt;
        [JsonProperty]
        public DateTimeOffset RequestedAt
        {
            get {   return _requestedAt; }
            set {   SetProperty(value, ref _requestedAt, () => RequestedAt);    }
        }

        private int _userId;
        [JsonProperty]
        public int UserId
        {
            get {   return _userId; }
            set {   SetProperty(value, ref _userId, () => UserId);  }
        }

        private User _user;
        [JsonProperty, HandleOnNesting, Aggregation]
        public User User
        {
            get { return _user; }
            set 
            {
                _user = value;
                OnPropertyChanged(() => User);
            }
        }

        private int? _regionId;
        [RangeValidator(0, RangeBoundaryType.Inclusive, 1, RangeBoundaryType.Ignore,
            MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "CapacityRequestRegionRequired")]
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "CapacityRequestRegionId")]
        public int? RegionId
        {
            get { return _regionId; }
            set { SetProperty(value, ref _regionId, () => RegionId); }
        }

        private int _hoursRequested;
        [RangeValidator(1, RangeBoundaryType.Inclusive, 1, RangeBoundaryType.Ignore,
            MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "CapacityRequestHoursRequestedRequired")]
        [Display(ResourceType = typeof(Translations), Name = "CapacityRequestHoursRequested")]        
        [JsonProperty]
        public int HoursRequested
        {
            get { return _hoursRequested; }
            set { SetProperty(value, ref _hoursRequested, () => HoursRequested); }
        }

        private CapacityCycle _capacityCycle;
        [JsonProperty, Aggregation]
        public CapacityCycle CapacityCycle
        {
            get { return _capacityCycle; }
            set
            {
                _capacityCycle = value;
                OnPropertyChanged(() => CapacityCycle);
            }
        }
        
        private ObservableCollection<CapacityDistribution> _capacityDistributions = new ObservableCollection<CapacityDistribution>();
        [HandleOnNesting] [Aggregation(isComposite:true)]
        [ObjectCollectionValidator(typeof(CapacityDistribution))]
        [JsonProperty]
        public ICollection<CapacityDistribution> CapacityDistributions
        {
            get { return _capacityDistributions; }
            set { _capacityDistributions = value?.ToObservableCollection(); }
        }

        private ObservableCollection<ContractShift> _contractShifts = new ObservableCollection<ContractShift>();
        [HandleOnNesting] [Aggregation(isIndependent: true)]
        [ObjectCollectionValidator(typeof(ContractShift))]
        [JsonProperty]
        public ICollection<ContractShift> ContractShifts
        {
            get { return _contractShifts; }
            set { _contractShifts = value?.ToObservableCollection(); }
        }

        private ObservableCollection<ContractSubscription> _contractSubscriptions = new ObservableCollection<ContractSubscription>();
        [HandleOnNesting]
        [Aggregation(isIndependent: true)]
        [JsonProperty]
        public ICollection<ContractSubscription> ContractSubscriptions
        {
            get { return _contractSubscriptions; }
            set { _contractSubscriptions = value?.ToObservableCollection(); }
        }

        private Region _region;
        [HandleOnNesting, Aggregation]
        [JsonProperty]
        public Region Region
        {
            get { return _region; }
            set 
            {   
                _region = value;
                OnPropertyChanged(() => Region);
            }
        }
        
        public static ObservableCollection<CodeGroup> PossibleVersions => StaticFactory.Instance.GetCodeGroups(CodeGroupType.CapacityVersion);

        public bool IsLocked => (Version == CapacityVersionType.Final);

        private double _meanHoursPerTechnician;
        public double MeanHoursPerTechnician
        {
            get { return Math.Round(_meanHoursPerTechnician, 1); }
            set
            {
                _meanHoursPerTechnician = value;
                OnPropertyChanged(() => MeanHoursPerTechnician);
            }
        }

        private int _multiplictorTechnicians;
        public int MultiplictorTechnicians
        {
            get { return _multiplictorTechnicians; }
            set
            {
                _multiplictorTechnicians = value;
                OnPropertyChanged(() => MultiplictorTechnicians);
            }
        }

        private int _multiplicatorHours;
        public int MultiplicatorHours
        {
            get { return _multiplicatorHours; }
            set
            {
                _multiplicatorHours = value;
                OnPropertyChanged(() => MultiplicatorHours);
            }
        }


        private double _techniciansRequested;

        public double TechniciansRequested
        {
            get { return Math.Round(_techniciansRequested, 1); }
            set
            {
                _techniciansRequested = value;

                OnPropertyChanged(() => TechniciansRequested);
                OnPropertyChanged(() => Technicians);
                OnPropertyChanged(() => DisplayRequestedTechnicians);
            }

        }
        
        public string Technicians => Math.Round(TechniciansRequested, 1).ToString(CultureInfo.InvariantCulture);

        public string DisplayRequestedTechnicians => string.Format(Translations.CapacityRequestDisplayTechniciansRequested, Technicians);

        public string DisplayRequestedHours => string.Format(Translations.CapacityRequestDisplayHoursRequested, HoursRequested);

        public string DisplayCapacity =>
            $"{HoursRequested}h base capacity with {MaxBaseDecreasePercentage}% max decrease percentage and {MaxBaseIncreasePercentage}% max increase percentage"
            ;

        public string DisplayVersion
        {
            get
            {
                if (IsNew) return "-";

                return $" [{StaticFactory.Instance.GetValue(CodeGroupType.CapacityVersion, Version.ToString()) ?? "?"}]";
            }
        }

        public User CurrentUser { get; set; }
        #endregion
        
        #region behavior
       public void RefreshState()
        {
            OnPropertyChanged(() => HoursRequested);
        }

        public static CapacityRequest Create(int capacityCylceId)
        {
            var capacityRequest = new CapacityRequest
            {
                CycleId = capacityCylceId,
                RequestedAt = DateTime.Today
            };
            capacityRequest.User = capacityRequest.GetCurrentUser(RepositoryFinder);
            if (capacityRequest.User != null)
            {
                capacityRequest.UserId = capacityRequest.User.Id;
                capacityRequest.SetAuditInfo(capacityRequest.User.Login);
            }

            return capacityRequest;
        }

        public static CapacityRequest Copy(CapacityRequest request)
        {
            // shallow copy parent object
            var copyRequest = request.ShallowClone<CapacityRequest>();
            copyRequest.Id = 0;
            
            // shallow copy capacity distributions
            request.CapacityDistributions.ForEach(cd =>
            {
                var copyDistribution = cd.ShallowClone<CapacityDistribution>();
                copyDistribution.Id = 0;
                copyRequest.CapacityDistributions.Add(copyDistribution);
            });

            // shallow copy shifts
            request.ContractShifts.ForEach(cs =>
            {
                var copyShift = cs.ShallowClone<ContractShift>();
                copyShift.EndEdit();
                copyShift.IndependentState = IndependentState.Added;
                copyRequest.ContractShifts.Add(copyShift);
            });

            // shallow copy contract subscriptions
            request.ContractSubscriptions.ForEach(cs =>
            {
                var copyContract = cs.ShallowClone<ContractSubscription>();
                copyContract.EndEdit();
                copyContract.IndependentState = IndependentState.Added;
                copyRequest.ContractSubscriptions.Add((copyContract));
            });

            return copyRequest;
        }


        public void ReduceToOwnState()
        {
            ContractSubscriptions.Clear();
            CapacityDistributions.Clear();
            ContractShifts.Clear();
            CapacityCycle = null;
            Region = null;
        }

        public void SetScopedTotals(double hoursRequested, double techniciansRequested)
        {
            // bypass the state tracking, just want to display the totals 
            // passed in.
            _techniciansRequested = techniciansRequested;
            _hoursRequested = (int)hoursRequested;

            OnPropertyChanged(() => HoursRequested);
            OnPropertyChanged(() => TechniciansRequested);
        }
        #endregion
    }
}