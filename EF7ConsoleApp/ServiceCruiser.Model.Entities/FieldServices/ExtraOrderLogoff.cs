using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Entities.Core.Utilities;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExtraOrderLogoff : LogOff
    {
        #region state
        private int _extraOrderId;
        [JsonProperty]
        public int ExtraOrderId
        {
            get { return _extraOrderId; }
            set { SetProperty(value, ref _extraOrderId, () => ExtraOrderId); }
        }

        private ExtraOrder _extraOrder;
        [JsonProperty]
        public ExtraOrder ExtraOrder
        {
            get { return _extraOrder; }
            set
            {
                _extraOrder = value;
                OnPropertyChanged(() => Visit);
            }
        }
        private decimal _unitPrice;
        [JsonProperty]
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(value, ref _unitPrice, () => UnitPrice); }
        }
        private int _vat;
        [JsonProperty]
        public int Vat
        {
            get { return _vat; }
            set { SetProperty(value, ref _vat, () => Vat); }
        }
        private bool _specialCare;
        [JsonProperty]
        public bool SpecialCare
        {
            get { return _specialCare; }
            set { SetProperty(value, ref _specialCare, () => SpecialCare); }
        }

        public decimal TotalPrice
        {
            get
            {
                if (SpecialCare)
                    return 0m;
                var totalEx = (decimal)Amount * UnitPrice;
                var totalVat = totalEx * Vat / 100;
                return totalEx + totalVat;
            }
        }

        public string DisplayTotalPrice
        {
            get { return string.Format("{0} euro", TotalPrice.ToString("0.00")); }
        }

        public string DisplayExtraShortDescription
        {
            get
            {
                string shortDescription = Part.ShortDescription;
                var valueMaterial = Part as ValueMaterial;
                
                if (valueMaterial != null)
                    if (valueMaterial.GroupId != null && valueMaterial.Group != null)
                        shortDescription = valueMaterial.Group.ShortDescription;

                if (Part is ITrackedPart)
                    return string.Format("{0} ({1})", shortDescription, Part.PartNo);
                return shortDescription;
            } 
        }

        public string DisplayExtraDescription
        {
            get
            {
                if (Part == null) return string.Empty;
                var partType = StaticFactory.Instance.GetValue(CodeGroupType.PartType, Part.GetType().Name) ?? "?";
                return string.Format("{0} {1} - {2} - {3} euro/{4}", Amount, partType, DisplayExtraShortDescription, UnitPrice, Part.DisplayUnit);
            }
        }

        #endregion

        #region behavior
        public static ExtraOrderLogoff Create(Visit visit, ILoggedPart part, ExtraOrder extraOrder, string login)
        {
            if (visit == null) return null;
            if (part == null) return null;
            if (extraOrder == null) return null;

            var unitPrice = part.PriceExtra;
            var vat = part.VatExtra;
            var valueMaterial = part as ValueMaterial;
            if (valueMaterial != null)
            {
                if (valueMaterial.GroupId != null && valueMaterial.Group != null) 
                {
                    //When a part belongs to a group => the price and vat of the group is used on the ExtraOrder
                    unitPrice = valueMaterial.Group.PriceExtra;
                    vat = valueMaterial.Group.VatExtra;
                }
            }

            var logOff = new ExtraOrderLogoff
            {
                VisitId = visit.Id,
                Visit = visit,
                PartId = part.Id,
                Part = (Part)part,
                ExtraOrderId = extraOrder.VisitId,
                ExtraOrder = extraOrder,
                UnitPrice = unitPrice,
                Vat = vat
            };

            logOff.SetAuditInfo(login);
            return logOff;
        }
        #endregion
    }
}
