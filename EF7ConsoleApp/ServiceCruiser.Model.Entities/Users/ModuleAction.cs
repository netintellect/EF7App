using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ModuleAction : ValidatedEntity<ModuleAction>
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
		public string Name 
		{ 
 			get {   return _name;   } 
 			set {   SetProperty(value,ref _name, () => Name);   } 
		}
    			

		private string _description;
		[JsonProperty]
		public string Description 
		{ 
 			get {   return _description;    } 
 			set {   SetProperty(value,ref _description, () => Description); } 
		}
    			
		private int _moduleId;
		[JsonProperty]
		public int ModuleId 
		{ 
 			get {   return _moduleId;   } 
 			set {   SetProperty(value,ref _moduleId, () => ModuleId);   } 
		}
    			
		private DateTimeOffset _validFrom;
		[JsonProperty]
		public DateTimeOffset ValidFrom 
		{ 
 			get {   return _validFrom;  } 
 			set {   SetProperty(value,ref _validFrom, () => ValidFrom); } 
		}
    			
		private DateTimeOffset? _validUntil;
		[JsonProperty]
		public DateTimeOffset? ValidUntil 
		{ 
 			get {   return _validUntil; } 
 			set {   SetProperty(value,ref _validUntil, () => ValidUntil);   } 
		}
        
		private Module _module;
		[JsonProperty]
		public Module Module 
		{
			get {   return _module; }
		    set
		    {
		        _module = value;
		        OnPropertyChanged(() => Module);
		    }
		}
        
		private ObservableCollection<Permission> _permissions = new ObservableCollection<Permission>();
		[JsonProperty]
		public ICollection<Permission> Permissions 
		{
			get {   return _permissions;    } 
			set {   _permissions= value != null ? value.ToObservableCollection() : null;    }
        }
        #endregion
    }
}
