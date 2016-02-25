using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Extensions;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Technicians;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), DeletePriority(50)]
    public class Address : ValidatedEntity<Address>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
        
        private string _street;
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Ignore,
                                MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "AddressStreetRequired")]
        [Display(ResourceType = typeof(Translations), Name ="AddressStreet")]		
		[JsonProperty]
		public string Street 
		{ 
 			get {   return _street; }
            set
            {
                if (SetProperty(value, ref _street, () => Street))
                {
                    OnPropertyChanged(() => IsPinnable);
                }
            } 
		}

        private string _houseNumber;
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Ignore,
                               MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "AddressHouseNumberRequired")]
        [Display(ResourceType = typeof(Translations), Name ="AddressHouseNumber")]
        [JsonProperty]
		public string HouseNumber 
		{ 
 			get {   return _houseNumber;    }
            set
            {
                if (SetProperty(value, ref _houseNumber, () => HouseNumber))
                {
                    OnPropertyChanged(() => IsPinnable);
                }
            } 
		}

        private string _postBox;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="AddressPostBox")]
		public string PostBox 
		{ 
 			get {   return _postBox;    } 
 			set {   SetProperty(value,ref _postBox, () => PostBox); } 
		}

        private int _zipCodeId;
        [RangeValidator(1, RangeBoundaryType.Inclusive, 1, RangeBoundaryType.Ignore,
                        MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "AddressPostalCodeRequired")]
        [Display(ResourceType = typeof(Translations), Name = "AddressPostalCode")]
        [JsonProperty]
        public int ZipCodeId
        {
            get { return _zipCodeId; }
            set { SetProperty(value, ref _zipCodeId, () => ZipCodeId); }
        }

        private ZipCode _zipCode;
        [JsonProperty, Aggregation]
		public ZipCode ZipCode 
		{ 
 			get {   return _zipCode; }
            set
            {
                _zipCode = value;

                OnPropertyChanged(() => ZipCode);
                OnPropertyChanged(() => IsPinnable);
            } 
		}
        
        private ObservableCollection<TechnicianAddress> _technicianAddresses = new ObservableCollection<TechnicianAddress>();
        [JsonProperty]
        public ICollection<TechnicianAddress> TechnicianAddresses
        {
            get { return _technicianAddresses; }
            set { _technicianAddresses = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<Location> _locations = new ObservableCollection<Location>();
        [JsonProperty]
        public ICollection<Location> Locations
        {
            get { return _locations; }
            set { _locations = value != null ? value.ToObservableCollection() : null; }
        }

        public bool IsPinnable
        {
            get
            {
                return !string.IsNullOrEmpty(Street) &&
                       !string.IsNullOrEmpty(HouseNumber) &&
                       ZipCode != null;
            }
        }

        public string DisplayAddress
        {
            get
            {
                var zipCode = ZipCode == null ? "?" : $"{ZipCode.Code} {ZipCode.City}";
                return $" {Street ?? "?"} {HouseNumber ?? "?"} {PostBox ?? ""}, {zipCode}";
            }
        }

        #endregion

        #region behavior
        public static Address Create(User user = null)
        {
            var address = new Address();
            if (user == null)
            {
                var currentUser = address.GetCurrentUser(RepositoryFinder);
                if (currentUser != null) address.SetAuditInfo(currentUser.Login);
            }
            else
            {
                address.SetAuditInfo(user.Login);
            }
            return address;
        }

        public void Clear()
        {
            Street = null;
            HouseNumber = null;
            PostBox = null;
            ZipCode = null;
        }
        #endregion
    }
}
