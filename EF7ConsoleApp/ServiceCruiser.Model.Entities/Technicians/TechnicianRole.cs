using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Capacities;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Entities.Technicians
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TechnicianRole : UserRole
    {
        #region state
        private ObservableCollection<Visit> _visits = new ObservableCollection<Visit>();
        [JsonProperty, Aggregation]
        public ICollection<Visit> Visits
        {
            get { return _visits; }
            set { _visits = value?.ToObservableCollection(); }
        }
        
        private ObservableCollection<Region> _regions = new ObservableCollection<Region>();
        [JsonProperty, Aggregation]
        public ICollection<Region> Regions
        {
            get { return _regions; }
            set { _regions = value?.ToObservableCollection(); }
        }

        private ObservableCollection<Assignment> _assignments = new ObservableCollection<Assignment>();
        [HandleOnNesting] [Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(Assignment))]
        [JsonProperty]
		public ICollection<Assignment> Assignments 
		{
			get {   return _assignments;} 
			set {   _assignments= value?.ToObservableCollection();} 
		}

        private int _technicianId;
        [JsonProperty]
        public int TechnicianId
        {
            get { return _technicianId; }
            set { SetProperty(value, ref _technicianId, () => Id); }
        }
        
        private Technician _technician;
        [JsonProperty, Aggregation]
        public Technician Technician
        {
            get {   return _technician; }
            set 
            {   _technician = value;
                OnPropertyChanged(() => Technician);
            }
        }

        private ObservableCollection<Skill> _skills = new ObservableCollection<Skill>();
        [JsonProperty, Aggregation]
        public ICollection<Skill> Skills
        {
            get {   return _skills; }
            set {   _skills = value?.ToObservableCollection();    }
        }

        [JsonProperty, Aggregation]
        public DseTechnicianInfo DseTechInfo { get; set; } 

        #endregion
    }
}
