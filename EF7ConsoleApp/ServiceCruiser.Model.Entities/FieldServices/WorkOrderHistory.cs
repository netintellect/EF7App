using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Users;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkOrderHistory : ValidatedEntity<WorkOrderHistory>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}

		private int _workOrderId;
		[JsonProperty]
		public int WorkOrderId 
		{ 
 			get {   return _workOrderId;    } 
 			set {   SetProperty(value,ref _workOrderId, () => WorkOrderId); } 
		}
    			
        private WorkStatusType _fromStatus;
		[JsonProperty]
		public WorkStatusType FromStatus 
		{ 
 			get {   return _fromStatus; } 
 			set {   SetProperty(value,ref _fromStatus, () => FromStatus);   } 
		}

		private WorkStatusType _toStatus;
		[JsonProperty]
		public WorkStatusType ToStatus 
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
        
		private WorkOrder _workOrder;
		[JsonProperty, Aggregation]
		public WorkOrder WorkOrder 
		{
			get {   return _workOrder;  } 
			set
            {   
                _workOrder= value;
				OnPropertyChanged(() => WorkOrder);
			}
        }
        #endregion

        #region behavior

        public static WorkOrderHistory Create(WorkOrder workOrder, 
                                              WorkStatusType fromState, UserRole currentUserRole)
        {
            if (workOrder == null) return null;
            if (currentUserRole == null) return null;
            var workOrderHistory = new WorkOrderHistory
            {
                WorkOrderId = workOrder.Id,
                WorkOrder = workOrder,
                UserRoleId = currentUserRole.Id,
                UserRole = currentUserRole,
                FromStatus = fromState,
                ToStatus = workOrder.Status
            };
            workOrderHistory.SetAuditInfo(currentUserRole.User.Login);
            return workOrderHistory;
        }
        /// <summary>
        /// For Mobile app => set the LocalId for added entities (those that have their id set to 0)
        /// </summary>
        public override void ApplyLocalId()
        {
            if (Id == 0 && LocalId != Guid.Empty) LocalId = new Guid();
        }

        #endregion
    }
}
