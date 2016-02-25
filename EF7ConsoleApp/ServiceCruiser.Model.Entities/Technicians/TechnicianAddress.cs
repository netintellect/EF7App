using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Technicians
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TechnicianAddress : ValidatedEntity<TechnicianAddress>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        [Display(ResourceType = typeof(Translations), Name = "TechnicianAddressId")]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id); }
        }
        
        private DateTimeOffset _validFrom;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "TechnicianAddressValidFrom")]
        public DateTimeOffset ValidFrom
        {
            get {   return _validFrom; }
            set
            {
                if (SetProperty(value, ref _validFrom, () => ValidFrom))
                {
                    OnPropertyChanged(() => DisplayPeriod);
                }
            }
        }

        private DateTimeOffset? _validUntil;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "TechnicianAddressValidUntil")]
        public DateTimeOffset? ValidUntil
        {
            get {   return _validUntil; }
            set
            {
                if (SetProperty(value, ref _validUntil, () => ValidUntil))
                {
                    OnPropertyChanged(() => DisplayPeriod);
                }
            }
        }
        
        private int _addressId;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "TechnicianAddressAddressId")]
        public int AddressId
        {
            get {   return _addressId; }
            set {   SetProperty(value, ref _addressId, () => AddressId);    }
        }


        private Address _address;
        [JsonProperty]
        [HandleOnNesting] [Aggregation(isComposite: true)] [ObjectValidator]
        public Address Address
        {
            get { return _address; }
            set 
            {   
                _address = value;
                OnPropertyChanged(() => Address);
                OnPropertyChanged(() => DisplayAddress);
            }
        }

        private int _technicianId;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "TechnicianAddressTechnicianId")]
        public int TechnicianId
        {
            get {   return _technicianId; }
            set {   SetProperty(value, ref _technicianId, () => TechnicianId);  }
        }

        private Technician _technician;
        [JsonProperty, Aggregation]
        public Technician Technician
        {
            get { return _technician; }
            set 
            {
                _technician = value;
                OnPropertyChanged(() => Technician);
            }
        }

        private ObservableCollection<WorkDay> _workDays = new ObservableCollection<WorkDay>();
        [JsonProperty]
        public ICollection<WorkDay> WorkDays
        {
            get { return _workDays; }
            set { _workDays = value?.ToObservableCollection(); }
        }

        
        public string DisplayAddress
        {
            get
            {
                if (Address == null) return "?";

                return
                    $"{Address.Street ?? "?"} {Address.HouseNumber ?? "?"}, {(Address.ZipCode == null ? "?" : Address.ZipCode.Code)} {(Address.ZipCode == null || Address.ZipCode.City == null ? "?" : Address.ZipCode.City)}";
            }
        }

        public string DisplayAddressLine1
        {
            get
            {
                if (Address == null) return "?";

                return $"{Address.Street ?? "?"} {Address.HouseNumber ?? "?"}";
            }
        }

        public string DisplayAddressLine2
        {
            get
            {
                if (Address == null) return "?";

                return
                    $"{(Address.ZipCode == null ? "?" : Address.ZipCode.Code)} {(Address.ZipCode == null || Address.ZipCode.City == null ? "?" : Address.ZipCode.City)}";
            }
        }
        
        public string DisplayAddressLine3
        {
            get
            {
                if (Address?.ZipCode == null) return "?";

                string countryValue = StaticFactory.Instance.GetCountryValue(Address.ZipCode.Country);

                return $"{countryValue ?? "?"}";
            }
        }

        public string DisplayPeriod
        {
            get
            {
                return $"{ValidFrom.ToString("d")} - {(ValidUntil != null ? ValidUntil.Value.ToString("d") : "?")}";
            }
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => DisplayAddress);
        }
        #endregion
    }
}
