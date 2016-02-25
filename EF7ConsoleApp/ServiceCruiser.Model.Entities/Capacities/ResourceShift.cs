using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Technicians;

namespace ServiceCruiser.Model.Entities.Capacities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResourceShift : Shift
    {
        #region state
        private int _contractorId;
		[JsonProperty]
		public int ContractorId 
		{ 
 			get {   return _contractorId;   } 
 			set {   SetProperty(value,ref _contractorId, () => ContractorId);   } 
		}

		private int? _overtime;
		[JsonProperty]
		public int? Overtime 
		{ 
 			get {   return _overtime;   } 
 			set {   SetProperty(value,ref _overtime, () => Overtime);   } 
		}

		private int? _snaptime;
		[JsonProperty]
		public int? Snaptime 
		{ 
 			get {   return _snaptime;   } 
 			set {   SetProperty(value,ref _snaptime, () => Snaptime);   } 
		}
        
		private ObservableCollection<WorkDay> _workDays = new ObservableCollection<WorkDay>();
		[JsonProperty]
		public ICollection<WorkDay> WorkDays 
		{
			get{return _workDays;} 
			set{_workDays= value != null ? value.ToObservableCollection() : null;} 
		}
        
		private ContractorCompany _contractorCompany;
		[JsonProperty]
		public ContractorCompany ContractorCompany 
		{
			get {   return _contractorCompany;} 
			set 
            {   
                _contractorCompany= value;
				OnPropertyChanged(() => ContractorCompany);
			}
        }
        #endregion
    }
}
