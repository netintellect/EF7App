using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Capacities;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Data;
using ServiceCruiser.Model.Entities.Technicians;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ContractorCompany : Company
    {
        #region state

        private ObservableCollection<ContractSubscription> _contractSubscriptions;
		[JsonProperty]
		public ICollection<ContractSubscription> ContractSubscriptions 
		{
			get{return _contractSubscriptions;} 
			set{_contractSubscriptions= value != null ? value.ToObservableCollection() : null;} 
		}
        
		private ObservableCollection<ResourceShift> _resourceShifts = new ObservableCollection<ResourceShift>();
		[JsonProperty]
		public ICollection<ResourceShift> ResourceShifts 
		{
			get{return _resourceShifts;} 
			set{_resourceShifts= value != null ? value.ToObservableCollection() : null;}
        }
        #endregion
    }
}
