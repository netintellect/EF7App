using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), DeletePriority(52)]
    public class ExtraOrder : ValidatedEntity<ExtraOrder>
    {
        #region state
        private int _id;
        [JsonProperty, Key(true)]
        public int VisitId
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => VisitId); }
        }
        private decimal _totalPrice;
        [JsonProperty]
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(value, ref _totalPrice, () => TotalPrice); }
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

        private ObservableCollection<ExtraOrderLogoff> _extraOrderLogoffItems = new ObservableCollection<ExtraOrderLogoff>();
        [JsonProperty]
        public ICollection<ExtraOrderLogoff> ExtraOrderLogoffItems
        {
            get { return _extraOrderLogoffItems; }
            set { _extraOrderLogoffItems = value != null ? value.ToObservableCollection() : null; }
        }

        public string DisplayTotalPrice
        {
            get { return string.Format("{0} euro", TotalPrice.ToString("0.00")); }
        }

        #endregion

        #region Behavior
        public static ExtraOrder Create(Visit visit, string login)
        {
            if (visit == null) return null;

            var extraOrder = new ExtraOrder { };
            extraOrder.SetAuditInfo(login);
            return extraOrder;
        }
        #endregion
    }
}
