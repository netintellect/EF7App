using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Capacities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ContractShift : Shift
    {
        #region state

        private int? _contractModelId;
        [JsonProperty]
        public int? ContractModelId
        {
            get {   return _contractModelId; }
            set {   SetProperty(value, ref _contractModelId, () => ContractModelId);    }
        }

        private ContractModel _contractModel;
        [Aggregation(isComposite: false)]
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

        private ObservableCollection<CapacityDistribution> _capacityDistributions = new ObservableCollection<CapacityDistribution>();
        [JsonProperty]
        public ICollection<CapacityDistribution> CapacityDistributions
        {
            get { return _capacityDistributions; }
            set { _capacityDistributions = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<CapacityRequest> _capacityRequests = new ObservableCollection<CapacityRequest>();
        [JsonProperty]
        public ICollection<CapacityRequest> CapacityRequests
        {
            get { return _capacityRequests; }
            set { _capacityRequests = value != null ? value.ToObservableCollection() : null; }
        }
        
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(() => IsSelected);
            }
        } 
        #endregion
    }
}
