using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Users;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ServiceOrderHistory : ValidatedEntity<ServiceOrderHistory>
    {
        #region state
		private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}

		private int _serviceOrderId;
		[JsonProperty]
		public int ServiceOrderId 
		{ 
 			get {   return _serviceOrderId; } 
 			set {   SetProperty(value,ref _serviceOrderId, () => ServiceOrderId);   } 
		}

		private ServiceStatusType _fromStatus;
		[JsonProperty]
		public ServiceStatusType FromStatus 
		{ 
 			get {   return _fromStatus; } 
 			set {   SetProperty(value,ref _fromStatus, () => FromStatus);   } 
		}

		private ServiceStatusType _toStatus;
		[JsonProperty]
		public ServiceStatusType ToStatus 
		{ 
 			get {   return _toStatus;   } 
 			set {   SetProperty(value,ref _toStatus, () => ToStatus);   } 
		}

		private string _reason;
		[JsonProperty]
		public string Reason 
		{ 
 			get {   return _reason; } 
 			set {   SetProperty(value,ref _reason, () => Reason);   } 
		}
    		

		private int _userRoleId;
		[JsonProperty]
		public int UserRoleId 
		{ 
 			get {   return _userRoleId; } 
 			set {   SetProperty(value,ref _userRoleId, () => UserRoleId);   } 
		}
    
        private ServiceOrder _serviceOrder;
		[JsonProperty, Aggregation]
		public ServiceOrder ServiceOrder 
		{
			get {   return _serviceOrder;   } 
			set 
            {   
                _serviceOrder= value;
				OnPropertyChanged(() => ServiceOrder);
			} 
		}
        
		private UserRole _userRole;
		[JsonProperty, Aggregation]
		public UserRole UserRole 
		{
			get {   return _userRole;   } 
			set 
            {
				_userRole= value;
				OnPropertyChanged(() => UserRole);
            }
        }
        #endregion

        #region behavior
        public static ServiceOrderHistory Create(ServiceOrder serviceOrder, ServiceStatusType fromStatus, UserRole currentUserRole)
        {
            if (serviceOrder == null) return null;
            if (currentUserRole == null) return null;

            return new ServiceOrderHistory
            {
                ServiceOrderId = serviceOrder.Id,
                ServiceOrder = serviceOrder,
                UserRoleId = currentUserRole.Id,
                UserRole = currentUserRole,
                FromStatus = fromStatus,
                ToStatus = serviceOrder.Status
            };
        }
        #endregion
    }
}
