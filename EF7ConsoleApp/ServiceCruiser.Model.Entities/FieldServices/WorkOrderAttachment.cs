using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkOrderAttachment: Attachment
    {
        #region state
        private int _workOrderId;
        [JsonProperty]
        public int WorkOrderId
        {
            get { return _workOrderId; }
            set { SetProperty(value, ref _workOrderId, () => WorkOrderId); }
        }

        #endregion

        #region behavior
        public static WorkOrderAttachment Create(WorkOrder workOrder, User currentUser,
                                                 UserRole currentUserRole)
        {
            if (workOrder == null) return null;
            if (currentUser == null) return null;
            if (currentUserRole == null) return null;

            return new WorkOrderAttachment
            {
                WorkOrderId = workOrder.Id,
                AttachedDate = DateTime.Now,
                AttachedById = currentUser.Id,
                AttachedBy = currentUser,
                AttachedByRoleId = currentUserRole.Id,
                AttachedByRole = currentUserRole
            };
        }
        #endregion
    }
}
