using System;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Users;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ServiceOrderRemark : Remark
    {
        #region state
        private int _serviceOrderId;
        [JsonProperty]
        public int ServiceOrderId
        {
            get {   return _serviceOrderId; }
            set { SetProperty(value, ref _serviceOrderId, () => ServiceOrderId); }
        }

        #endregion

        #region behavior
        public static ServiceOrderRemark Create(ServiceOrder serviceOrder, User currentUser,
                                                UserRole currentUserRole)
        {
            if (serviceOrder == null) return null;
            if (currentUser == null) return null;
            if (currentUserRole == null) return null;
            
            return new ServiceOrderRemark
            {
                Type = RemarkType.Service,
                EnteredDate = DateTime.Now,
                UserId = currentUser.Id,
                User = currentUser,
                UserRoleId = currentUserRole.Id,
                UserRole = currentUserRole
            };
        }

        public static ServiceOrderRemark Create(User currentUser)
        {
            if (currentUser == null) return null;

            var userRole = currentUser.UserRoles.FirstOrDefault();
            if (userRole == null) return null;

            return new ServiceOrderRemark
            {
                EnteredDate = DateTime.Now,
                UserId = currentUser.Id,
                User = currentUser,
                UserRoleId = userRole.Id,
                UserRole = userRole
            };
        }
        #endregion
    }
}
