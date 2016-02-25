using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VisitSignatureAttachment : Attachment
    {
        #region state
        private int _visitId;
        [JsonProperty]
        public int VisitId
        {
            get { return _visitId; }
            set { SetProperty(value, ref _visitId, () => VisitId); }
        }

        private ObservableCollection<Visit> _visits = new ObservableCollection<Visit>();
        [JsonProperty]
        public ICollection<Visit> Visits
        {
            get { return _visits; }
            set { _visits = value != null ? value.ToObservableCollection() : null; }
        }
        #endregion

        #region behavior
        public static VisitSignatureAttachment Create(Visit visit, User currentUser, UserRole currentUserRole)
        {
            if (visit == null) return null;
            if (visit.WorkOrder == null) return null;
            if (currentUser == null) return null;
            if (currentUserRole == null) return null;

            return new VisitSignatureAttachment
            {
                VisitId = visit.Id,
                ContractModelId = visit.WorkOrder.ContractModelId,
                Name = string.Format("Signature for Visit {0}", visit.Id),
                Type = AttachmentType.Png,
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
