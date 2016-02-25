using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Permission : ValidatedEntity<Permission>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
        private bool _canExecute;
		[JsonProperty]
		public bool CanExecute 
		{ 
 			get {   return _canExecute; } 
 			set {   SetProperty(value,ref _canExecute, () => CanExecute);   } 
		}
    			
		private bool _canRead;
		[JsonProperty]
		public bool CanRead 
		{ 
 			get {   return _canRead;    } 
 			set
            {
			    SetProperty(value,ref _canRead, () => CanRead);
			} 
		}

		private bool _canCreate;
		[JsonProperty]
		public bool CanCreate 
		{ 
 			get {   return _canCreate;  } 
 			set {   SetProperty(value,ref _canCreate, () => CanCreate); } 
		}
    			

		private bool _canUpdate;
		[JsonProperty]
		public bool CanUpdate 
		{ 
 			get {   return _canUpdate;  } 
 			set {   SetProperty(value,ref _canUpdate, () => CanUpdate); } 
		}
    			
		private bool _canDelete;
		[JsonProperty]
		public bool CanDelete 
		{ 
 			get {   return _canDelete;  } 
 			set {   SetProperty(value,ref _canDelete, () => CanDelete); } 
		}
    			
		private int _actionId;
		[JsonProperty]
		public int ActionId 
		{ 
 			get {   return _actionId;   } 
 			set {   SetProperty(value,ref _actionId, () => ActionId);   } 
		}
    			
        private int _userRoleId;
		[JsonProperty]
		public int UserRoleId 
		{ 
 			get {   return _userRoleId; } 
 			set {   SetProperty(value,ref _userRoleId, () => UserRoleId);   } 
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
 			get {   return _validUntil;    } 
 			set {   SetProperty(value,ref _validUntil, () => ValidUntil);   } 
		}
    
		private ModuleAction _action;
		[JsonProperty]
		public ModuleAction Action 
		{
			get {   return _action; }
		    set
		    {
		        _action = value;
		        OnPropertyChanged(() => Action);
		    }
		}
        
		private UserRole _userRole;
		[JsonProperty]
		public UserRole UserRole 
		{
			get {   return _userRole;   } 
			set {
				_userRole= value;
				OnPropertyChanged(() => UserRole);
			}
        }

        public override string ToString()
        {
            var permissions = $"[{Id}]: CanExecute = {CanExecute} - CanRead = {CanRead} - CanCreate = {CanCreate} - " +
                              $"CanDelete = {CanDelete} - CanUpdate = {CanUpdate}";
            if (UserRole != null && UserRole.User != null)
                permissions = permissions + $" => assigned to roleType [{UserRole.Id}] {UserRole.RoleType} for [{UserRole.User.Id}] {UserRole.User.DisplayName}";
            return permissions;
        }

        #endregion
    }
}
