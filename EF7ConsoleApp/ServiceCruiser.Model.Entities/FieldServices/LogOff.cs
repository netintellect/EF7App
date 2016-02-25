using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), DeletePriority(51)]
    public class LogOff : ValidatedEntity<LogOff>
    {
        #region state
        private int _id;
        [JsonProperty, Key(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private int _partId;
        [JsonProperty]
        public int PartId
        {
            get { return _partId; }
            set { SetProperty(value, ref _partId, () => PartId); }
        }
        private Part _part;
        [HandleOnNesting, Aggregation(isComposite: false)]
        [JsonProperty]
        public Part Part
        {
            get { return _part; }
            set
            {
                _part = value;
                OnPropertyChanged(() => Part);
            }
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
        private double _amount;
        [JsonProperty]
        public double Amount
        {
            get { return _amount; }
            set { SetProperty(value, ref _amount, () => Amount); }
        }

        private int? _inventoryLogoffId;
        [JsonProperty]
        public int? InventoryLogoffId
        {
            get { return _inventoryLogoffId; }
            set { SetProperty(value, ref _inventoryLogoffId, () => InventoryLogoffId); }
        }
        private InventoryLogoff _inventoryLogoff;
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public InventoryLogoff InventoryLogoff
        {
            get { return _inventoryLogoff; }
            set
            {
                _inventoryLogoff = value;
                OnPropertyChanged(() => InventoryLogoff);
            }
        }

        public string DisplayDescription
        {
            get { return Part == null ? string.Empty : string.Format("{0} ({1}) x {2}", Amount, Part.DisplayUnit, Part.ShortDescription); }
        }
        #endregion

        #region behavior
        public static LogOff Create(Visit visit, ILoggedPart part, string login)
        {
            if (visit == null) return null;
            if (part == null) return null;

            var logOff = new LogOff
            {
                VisitId = visit.Id,
                Visit = visit,
                PartId = part.Id,
                Part = (Part)part
            };

            logOff.SetAuditInfo(login);
            return logOff;
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
