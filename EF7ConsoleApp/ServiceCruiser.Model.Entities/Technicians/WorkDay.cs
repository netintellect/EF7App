using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Capacities;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod;
using ServiceCruiser.Model.Entities.Resources;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Technicians
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkDay :  ValidatedEntity<WorkDay>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		[Display(ResourceType = typeof(Translations), Name ="WorkDayId")]
		public int Id 
		{ 
 			get{    return _id; } 
 			set{    SetProperty(value,ref _id, () => Id);   } 
		}

        private DateTimeOffset _date;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkDayDate")]
		public DateTimeOffset Date 
		{ 
 			get {   return _date;} 
 			set {   SetProperty(value,ref _date, () => Date);   } 
		}

        private int _technicianId;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkDayTechnicianId")]
		public int TechnicianId 
		{ 
 			get {   return _technicianId;   } 
 			set {   SetProperty(value,ref _technicianId, () => TechnicianId);   } 
		}

        private Technician _technician;

        [JsonProperty]
        public Technician Technician
        {
            get { return _technician; }
            set
            {
                _technician = value;
                OnPropertyChanged(() => Technician);
            }
        }

        private int _resourceShiftId;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkDayShiftId")]
		public int ResourceShiftId 
		{ 
 			get {   return _resourceShiftId;    } 
 			set {   SetProperty(value,ref _resourceShiftId, () => ResourceShiftId); } 
		}

        private ResourceShift _resourceShift;    
        [HandleOnNesting] [Aggregation(isComposite: false)]
		[JsonProperty]
		public ResourceShift ResourceShift 
		{
			get {   return _resourceShift;  } 
			set 
            {   
                _resourceShift= value;
				OnPropertyChanged(() => ResourceShift);
			} 
		}
        
        private int? _technicianAddressId;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="WorkDayTechnicianAddressId")]
		public int? TechnicianAddressId 
		{ 
 			get {   return _technicianAddressId;   } 
 			set {   SetProperty(value, ref _technicianAddressId, () => TechnicianAddressId);   } 
		}

        private TechnicianAddress _technicianAddress;
        [HandleOnNesting] [Aggregation]
        [JsonProperty]
		public TechnicianAddress TechnicianAddress 
		{
			get {   return _technicianAddress;   } 
			set 
            {
                _technicianAddress= value;
				OnPropertyChanged(() => TechnicianAddress);
			} 
		}

        private ObservableCollection<Assignment> _assignments = new ObservableCollection<Assignment>();
        [JsonProperty]
        public ICollection<Assignment> Assignments
        {
            get { return _assignments; }
            set { _assignments = value != null ? value.ToObservableCollection() : null; }
        }
        
        public ITimeRange WorkDayRange
        {
            get { return ResourceShift == null ? null : ResourceShift.WorkDayRange; }
        }

        public ITimeRange BreakWinddowRange
        {
            get { return ResourceShift == null ? null : ResourceShift.BreakWindowRange; }
        }

        public ITimeRange MostOftenBreakRange 
        { 
            get { return ResourceShift == null ? null : ResourceShift.MostOftenBreakRange; }
        }

        public ITimeRange BreakDurationRange
        {
            get { return ResourceShift == null ? null : ResourceShift.BreakDurationRange; }
        }

        public double WorkingHoursDuration
        {
            get { return ResourceShift == null ? 0d : ResourceShift.WorkingHoursDuration; }
        }
        #endregion
    }
}
