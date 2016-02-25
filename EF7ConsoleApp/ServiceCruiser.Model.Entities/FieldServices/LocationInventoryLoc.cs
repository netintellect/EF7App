using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LocationInventoryLoc : InventoryLocation
    {
        #region behavior
        public static LocationInventoryLoc Create(string externalRef, User user)
        {
            if (string.IsNullOrEmpty(externalRef)) return null;

            var location = new LocationInventoryLoc
            {
                ExternalRef = externalRef
            };
            location.SetAuditInfo(user.Login);
            return location;
        }
        #endregion
    }
}
