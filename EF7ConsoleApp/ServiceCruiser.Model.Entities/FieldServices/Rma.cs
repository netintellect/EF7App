using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Rma : InventoryAction
    {
        #region state
        private string _rmaNumber;
        [JsonProperty]
        public string RmaNumber
        {
            get { return _rmaNumber; }
            set { SetProperty(value, ref _rmaNumber, () => RmaNumber); }
        }
        private int _visitId;
        [JsonProperty]
        public int VisitId
        {
            get { return _visitId; }
            set { SetProperty(value, ref _visitId, () => VisitId); }
        }
        private Visit _visit;
        [JsonProperty, Aggregation(isComposite: false)]
        public Visit Visit
        {
            get { return _visit; }
            set
            {
                _visit = value;
                OnPropertyChanged(() => Visit);
            }
        }
        private int _rmaReasonId;
        [JsonProperty]
        public int RmaReasonId
        {
            get { return _rmaReasonId; }
            set { SetProperty(value, ref _rmaReasonId, () => RmaReasonId); }
        }
        private RmaReason _rmaReason;
        [JsonProperty, Aggregation(isComposite: false)]
        public RmaReason RmaReason
        {
            get { return _rmaReason; }
            set
            {
                _rmaReason = value;
                OnPropertyChanged(() => RmaReason);
            }
        }

        public string DisplayDescription
        {
            get
            {
                return Part == null ? string.Empty : string.Format("{0} ({1}) x {2}", Amount, Part.DisplayUnit, Part.ShortDescription);
            }
        }
        #endregion

        #region behavior
        public static Rma Create(Visit visit, ITrackedPart part, UserRole currentUserRole, string login)
        {
            if (visit == null) return null;
            if (part == null) return null;
            if (currentUserRole == null) return null;

            var rma = new Rma
            {
                VisitId = visit.Id,
                Visit = visit,
                PartId = part.Id,
                Part = (Part)part,
                UserRoleId = currentUserRole.Id,
                User = currentUserRole
            };
            rma.SetAuditInfo(login);
            return rma;
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
