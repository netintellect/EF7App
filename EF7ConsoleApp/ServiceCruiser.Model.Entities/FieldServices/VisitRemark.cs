using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VisitRemark : Remark
    {
        #region state
        private int _visitId;
		[JsonProperty]
		public int VisitId 
		{ 
 			get {   return _visitId;    } 
 			set {   SetProperty(value,ref _visitId, () => VisitId); } 
		}
        #endregion

        #region behavior

        public static VisitRemark Create(Visit visit, User currentUser, UserRole currentUserRole)
        {
            if (visit == null) return null;
            if (currentUser == null) return null;
            if (currentUserRole == null) return null;

            var remark = new VisitRemark
            {
                Type = RemarkType.Visit,
                EnteredDate = DateTime.Now,
                VisitId = visit.Id,
                UserId = currentUser.Id,
                User = currentUser,
                UserRoleId = currentUserRole.Id,
                UserRole = currentUserRole
            };
            remark.SetAuditInfo(currentUser.Login);
            return remark;
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
