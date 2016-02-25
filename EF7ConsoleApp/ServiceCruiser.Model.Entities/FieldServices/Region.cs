using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Capacities;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Technicians;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Region : ValidatedEntity<Region>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
		private string _name;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="RegionName")]
		public string Name 
		{ 
 			get {   return _name;   } 
 			set {   SetProperty(value,ref _name, () => Name);   } 
		}
        
        private int _contractModelId;
		[JsonProperty]
		public int ContractModelId 
		{ 
 			get {   return _contractModelId;    } 
 			set {   SetProperty(value,ref _contractModelId, () => ContractModelId); } 
		}
    
        
		private ObservableCollection<CapacityRequest> _capacityRequests = new ObservableCollection<CapacityRequest>();
		[JsonProperty]
		public ICollection<CapacityRequest> CapacityRequests 
		{
			get {   return _capacityRequests;   } 
			set {   _capacityRequests= value != null ? value.ToObservableCollection() : null;   } 
		}
        
		private ContractModel _contractModel;
		[JsonProperty]
		public ContractModel ContractModel 
		{
			get {   return _contractModel;  } 
			set 
            {   
                _contractModel= value;
				OnPropertyChanged(() => ContractModel);
            } 
		}

        private ObservableCollection<ZipCode> _zipCodes;
        [JsonProperty, Aggregation(isIndependent: true)]
        public ICollection<ZipCode> ZipCodes
        {
            get { return _zipCodes; }
            set { _zipCodes = value != null ? value.ToObservableCollection() : null; }
        }

		private ObservableCollection<TechnicianRole> _technicianRoles = new ObservableCollection<TechnicianRole>();
		[JsonProperty]
		public ICollection<TechnicianRole> TechnicianRoles 
		{
			get {   return _technicianRoles;    } 
			set {   _technicianRoles= value != null ? value.ToObservableCollection() : null;    }
        }

        private ObservableCollection<TravelTime> _travelTimes = new ObservableCollection<TravelTime>();
        [JsonProperty, HandleOnNesting, Aggregation(true)]
        public ICollection<TravelTime> TravelTimes
        {
            get { return _travelTimes; }
            set { _travelTimes = value != null ? value.ToObservableCollection() : null; }
        }
        #endregion
    }
}
