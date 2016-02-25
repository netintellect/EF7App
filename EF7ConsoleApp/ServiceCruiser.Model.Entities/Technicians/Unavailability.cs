using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Technicians
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Unavailability : ValidatedEntity<Unavailability>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
		private DateTimeOffset _date;
		[JsonProperty]
		public DateTimeOffset Date 
		{ 
 			get {   return _date;} 
 			set {   SetProperty(value,ref _date, () => Date);   } 
		}
    			
        private TimeSpan _startTime;
		[JsonProperty]
		public TimeSpan StartTime 
		{ 
 			get {   return _startTime;  } 
 			set {   SetProperty(value,ref _startTime, () => StartTime); } 
		}
    			
        private TimeSpan? _endTime;
		[JsonProperty]
		public TimeSpan? EndTime 
		{ 
 			get {   return _endTime;    } 
 			set {   SetProperty(value,ref _endTime, () => EndTime); } 
		}
    			
		private bool _isWholeDay;
		[JsonProperty]
		public bool IsWholeDay 
		{ 
 			get {   return _isWholeDay;    } 
 			set {   SetProperty(value,ref _isWholeDay, () => IsWholeDay);   } 
		}

		private int _technicianId;
		[JsonProperty]
		public int TechnicianId 
		{ 
 			get {   return _technicianId;   } 
 			set {   SetProperty(value,ref _technicianId, () => TechnicianId);   } 
		}
    
		private Technician _technician;
		[JsonProperty]
		public Technician Technician 
		{
			get {   return _technician;    } 
			set 
            {   
				_technician= value;
				OnPropertyChanged(() => Technician);
			}
        }
        #endregion
    }
}
