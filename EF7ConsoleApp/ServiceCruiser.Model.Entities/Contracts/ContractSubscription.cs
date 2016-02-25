using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Capacities;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Technicians;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ContractSubscription : ValidatedEntity<ContractSubscription>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id); }
        }

        private DateTimeOffset _validFrom;
        [JsonProperty]
        public DateTimeOffset ValidFrom
        {
            get {   return _validFrom; }
            set {   SetProperty(value, ref _validFrom, () => ValidFrom);    }
        }


        private DateTimeOffset? _validUntil;
        [JsonProperty]
        public DateTimeOffset? ValidUntil
        {
            get {   return _validUntil; }
            set {   SetProperty(value, ref _validUntil, () => ValidUntil);  }
        }

        private int _contractorId;
        [JsonProperty]
        public int ContractorId
        {
            get {   return _contractorId; }
            set {   SetProperty(value, ref _contractorId, () => ContractorId);  }
        }

        private ContractorCompany _contractor;
    	[HandleOnNesting, Aggregation]
        [JsonProperty]
        public ContractorCompany Contractor
        {
            get { return _contractor; }
            set 
            {   
                _contractor = value;
                OnPropertyChanged(() => Contractor);
            }
        }

        private int _contractModelId;
        [JsonProperty]
        public int ContractModelId
        {
            get {   return _contractModelId; }
            set {   SetProperty(value, ref _contractModelId, () => ContractModelId);    }
        }

        private ContractModel _contractModel;
        [Aggregation, JsonProperty]
        public ContractModel ContractModel
        {
            get { return _contractModel; }
            set 
            {   
                _contractModel = value;
                OnPropertyChanged(() => ContractModel);
            }
        }
        
        private ObservableCollection<ContractSubscription> _subContractSubscriptions = new ObservableCollection<ContractSubscription>();
        [JsonProperty]
        public ICollection<ContractSubscription> SubContractSubscriptions
        {
            get { return _subContractSubscriptions; }
            set { _subContractSubscriptions = value != null ? value.ToObservableCollection() : null; }
        }

        private int? _superContractSubscriptionId;
        [JsonProperty]
        public int? SuperContractSubscriptionId
        {
            get { return _superContractSubscriptionId; }
            set { SetProperty(value, ref _superContractSubscriptionId, () => SuperContractSubscriptionId); }
        }

        private ContractSubscription _superContractSubscription;
        [JsonProperty]
        public ContractSubscription SuperContractSubscription
        {
            get { return _superContractSubscription; }
            set {   
                _superContractSubscription = value;
                OnPropertyChanged(() => SuperContractSubscription);
            }
        }

        private ObservableCollection<CapacityRequest> _capacityRequests = new ObservableCollection<CapacityRequest>();
        [JsonProperty]
        public ICollection<CapacityRequest> CapacityRequests
        {
            get { return _capacityRequests; }
            set { _capacityRequests = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<CapacityDistribution> _capacityDistributions = new ObservableCollection<CapacityDistribution>();
        [JsonProperty]
        public ICollection<CapacityDistribution> CapacityDistributions
        {
            get { return _capacityDistributions; }
            set { _capacityDistributions = value != null ? value.ToObservableCollection() : null; }
        }
        
        [JsonProperty]
        public DseTechnicianInfo DseTechInfo { get; set; } 

        #endregion
    }
}
