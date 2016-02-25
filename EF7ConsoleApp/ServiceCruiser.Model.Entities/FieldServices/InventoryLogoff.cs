using System;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InventoryLogoff : InventoryAction
    {
        #region behavior
        public static InventoryLogoff Create(ITrackedPart part, User user)
        {
            var inventoryLogoff = new InventoryLogoff
            {
                Time = DateTimeOffset.Now,
                UserRoleId = user.UserRoles.First(ur => ur.RoleType == RoleType.Technician).Id,
                PartId = part.Id,
                Part = (Part)part
            };
            inventoryLogoff.SetAuditInfo(user.Login);
            return inventoryLogoff;
        }
        #endregion
    }
}
