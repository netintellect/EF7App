using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Extensions;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Capacities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CapacityDistribution : ValidatedEntity<CapacityDistribution>
    {
        #region state
        private int _id;
        [JsonProperty, Key(true)]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private DateTimeOffset _date;
        [JsonProperty]
        public DateTimeOffset Date
        {
            get {   return _date; }
            set {   SetProperty(value, ref _date, () => Date);  }
        }

        private double _percent;
        [JsonProperty]
        public double Percent
        {
            get {   return _percent; }
            set {   SetProperty(value, ref _percent, () => Percent);    }
        }

        private int _capacityRequestId;
        [JsonProperty]
        public int CapacityRequestId
        {
            get {   return _capacityRequestId; }
            set {   SetProperty(value, ref _capacityRequestId, () => CapacityRequestId);    }
        }

        private CapacityRequest _capacityRequest;
        [JsonProperty]
        public CapacityRequest CapacityRequest
        {
            get {    return _capacityRequest; }
            set
            {
                _capacityRequest = value;
                OnPropertyChanged(() => CapacityRequest);
            }
        }

        private ObservableCollection<Assignment> _assignments = new ObservableCollection<Assignment>();
        [HandleOnNesting] [Aggregation(isComposite: true)] [ObjectCollectionValidator(typeof(Assignment))]
        [JsonProperty]
        public ICollection<Assignment> Assignments
        {
            get { return _assignments; }
            set { _assignments = value != null ? value.ToObservableCollection() : null; }
        }

        private int _contractSubscriptionId;
        [JsonProperty]
        public int ContractSubscriptionId
        {
            get {   return _contractSubscriptionId; }
            set {   SetProperty(value, ref _contractSubscriptionId, () => ContractSubscriptionId);  }
        }

        private ContractSubscription _contractSubscription;
        [HandleOnNesting] [Aggregation]
        [JsonProperty]
        public ContractSubscription ContractSubscription
        {
            get {   return _contractSubscription; }
            set 
            {   
                _contractSubscription = value;
                OnPropertyChanged(() => ContractSubscription);
            }
        }

        private int _contractShiftId;
        [JsonProperty]
        public int ContractShiftId
        {
            get {   return _contractShiftId; }
            set {   SetProperty(value, ref _contractShiftId, () => ContractShiftId);    }
        }

        private ContractShift _contractShift;
        [HandleOnNesting] [Aggregation]
        [JsonProperty]
        public ContractShift ContractShift
        {
            get { return _contractShift; }
            set 
            {   
                _contractShift = value;
                OnPropertyChanged(() => ContractShift);
            }
        }
        
        #endregion
        
        #region behavior
        public static  CapacityDistribution Create()
        {
            var capacityDistribution = new CapacityDistribution();
            var user = capacityDistribution.GetCurrentUser(RepositoryFinder);
            if (user != null)
                capacityDistribution.SetAuditInfo(user.Login);

            return capacityDistribution;
        }
        #endregion
    }
}
