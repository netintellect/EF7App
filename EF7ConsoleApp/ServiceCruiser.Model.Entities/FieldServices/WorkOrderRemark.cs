using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkOrderRemark : Remark
    {
        #region state
        private int _workOrderId;
        [JsonProperty]
        public int WorkOrderId
        {
            get {   return _workOrderId; }
            set {   SetProperty(value, ref _workOrderId, () => WorkOrderId); }
        }

        public PlanningOverrideType PlanningOverrideType { get; set; }
        #endregion

        #region behavior
        
        public static WorkOrderRemark Create(WorkOrder workOrder, User currentUser, UserRole currentUserRole)
        {
            if (workOrder == null) return null;
            if (currentUser == null) return null;
            if (currentUserRole == null) return null;

            return new WorkOrderRemark
            {
                Type = RemarkType.Work,
                EnteredDate = DateTime.Now,
                UserId = currentUser.Id,
                User = currentUser,
                UserRoleId = currentUserRole.Id,
                UserRole = currentUserRole
            };
            
        }
        
        /// <summary>
        /// For Mobile app => set the LocalId for added entities (those that have their id set to 0)
        /// </summary>
        public override void ApplyLocalId()
        {
            if (Id == 0 && LocalId == Guid.Empty) LocalId = Guid.NewGuid();
        }

        #endregion
    }
}
