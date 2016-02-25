using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), DeletePriority(50)]
    public class InventoryAction : ValidatedEntity<InventoryAction>
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
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: false)]
        public Part Part
        {
            get { return _part; }
            set
            {
                _part = value;
                OnPropertyChanged(() => Part);
            }
        }
        private DateTimeOffset _time;
        [JsonProperty]
        public DateTimeOffset Time
        {
            get { return _time; }
            set { SetProperty(value, ref _time, () => Time); }
        }
        private int _userRoleId;
        [JsonProperty]
        public int UserRoleId
        {
            get { return _userRoleId; }
            set { SetProperty(value, ref _userRoleId, () => UserRoleId); }
        }
        private UserRole _user;
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: false)]
        public UserRole User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(() => User);
            }
        }
        private string _serial;
        [JsonProperty]
        public string Serial
        {
            get { return _serial; }
            set { SetProperty(value, ref _serial, () => Serial); }
        }
        private double _amount;
        [JsonProperty]
        public double Amount
        {
            get { return _amount; }
            set { SetProperty(value, ref _amount, () => Amount); }
        }
        private int _fromLocationId;
        [JsonProperty]
        public int FromLocationId
        {
            get { return _fromLocationId; }
            set { SetProperty(value, ref _fromLocationId, () => FromLocationId); }
        }
        private InventoryLocation _fromLocation;
        [JsonProperty, Aggregation(isComposite: false)]
        public InventoryLocation FromLocation
        {
            get { return _fromLocation; }
            set
            {
                _fromLocation = value;
                OnPropertyChanged(() => FromLocation);
            }
        }

        private int _toLocationId;
        [JsonProperty]
        public int ToLocationId
        {
            get { return _toLocationId; }
            set { SetProperty(value, ref _toLocationId, () => ToLocationId); }
        }
        private InventoryLocation _toLocation;
        [JsonProperty, Aggregation(isComposite: false)]
        public InventoryLocation ToLocation
        {
            get { return _toLocation; }
            set
            {
                _toLocation = value;
                OnPropertyChanged(() => ToLocation);
            }
        }
        #endregion
    }
}
