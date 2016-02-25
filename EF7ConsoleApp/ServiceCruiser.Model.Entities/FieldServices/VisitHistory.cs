using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Users;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VisitHistory : ValidatedEntity<VisitHistory>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
		private int _visitId;
		[JsonProperty]
		public int VisitId 
		{ 
 			get {   return _visitId;    } 
 			set {   SetProperty(value,ref _visitId, () => VisitId); } 
		}

        private Visit _visit;
        [JsonProperty, Aggregation]
        public Visit Visit
        {
            get { return _visit; }
            set
            {
                _visit = value;
                OnPropertyChanged(() => Visit);
            }
        }
    			
		private VisitStatusType _fromStatus;
		[JsonProperty]
		public VisitStatusType FromStatus 
		{ 
 			get {   return _fromStatus; } 
 			set {   SetProperty(value,ref _fromStatus, () => FromStatus);  } 
		}
    			
		private VisitStatusType _toStatus;
		[JsonProperty]
		public VisitStatusType ToStatus 
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
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged(() => UserRole);
            }
        }

		private bool _undone;
		[JsonProperty]
		public bool Undone 
		{ 
 			get {   return _undone; } 
 			set {   SetProperty(value,ref _undone, () => Undone);   } 
		}
        #endregion

        #region behavior

        /// <summary>
        /// For Mobile app => set the LocalId for added entities (those that have their id set to 0)
        /// </summary>
        public override void ApplyLocalId()
        {
            if (Id == 0 && LocalId == Guid.Empty) LocalId = Guid.NewGuid();
        }

        public static VisitHistory Create(Visit visit,
                                          VisitStatusType fromStatus,
                                           UserRole userRole)
        {
            if (visit == null) return null;
            if (userRole == null) return null;

            var visitHistory = new VisitHistory
            {
                VisitId = visit.Id,
                Visit = visit,
                UserRoleId = userRole.Id,
                UserRole = userRole,
                FromStatus = fromStatus,
                ToStatus = visit.Status
            };
            visitHistory.SetAuditInfo(userRole.User.Login);
            return visitHistory;
        }

        #endregion
    }
}
