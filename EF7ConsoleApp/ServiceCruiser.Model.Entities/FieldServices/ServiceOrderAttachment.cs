using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ServiceOrderAttachment: Attachment
    {
        #region state
        private int _serviceOrderId;
        [JsonProperty]
        public int ServiceOrderId
        {
            get { return _serviceOrderId; }
            set { SetProperty(value, ref _serviceOrderId, () => ServiceOrderId); }
        }

        #endregion

        #region behavior
        public static ServiceOrderAttachment Create(ServiceOrder serviceOrder, User currentUser,
                                                    UserRole currentUserRole)
        {
            if (serviceOrder == null) return null;
            if (currentUser == null) return null;
            if (currentUserRole == null) return null;

            return new ServiceOrderAttachment
            {
                ServiceOrderId = serviceOrder.Id,
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
