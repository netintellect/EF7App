using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Extensions;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ValidationResult = ServiceCruiser.Model.Validations.Core.ValidationResult;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), DeletePriority(80)]
    [HasSelfValidation]
    public class Location : ValidatedEntity<Location>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}

        private double? _latitude;
		[JsonProperty]
		public double? Latitude 
		{ 
 			get {   return _latitude;   }
            set
            {
                if (SetProperty(value, ref _latitude, () => Latitude))
                {
                    OnPropertyChanged(() => DisplayLocation,
                                      () => DisplayLatitude);
                }
            } 
		}

        private double? _longitude;
		[JsonProperty]
        public double? Longitude 
		{ 
 			get {   return _longitude;  }
            set
            {
                if (SetProperty(value, ref _longitude, () => Longitude))
                {
                    OnPropertyChanged(() => DisplayLocation,
                                      () => DisplayLongitude);
                }
            } 
		}

        private string _locationRef;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="LocationLocationRef")]
		public string LocationRef 
		{ 
 			get {   return _locationRef;    } 
 			set {   SetProperty(value,ref _locationRef, () => LocationRef); } 
		}
    			
		private bool _isMaster;
		[JsonProperty]
		public bool IsMaster 
		{ 
 			get {   return _isMaster;   } 
 			set {   SetProperty(value,ref _isMaster, () => IsMaster);   } 
		}
    			
        private int? _customerId;
		[JsonProperty]
		public int? CustomerId 
		{ 
 			get {   return _customerId; } 
 			set {   SetProperty(value,ref _customerId, () => CustomerId);   } 
		}

        private int? _addressId;
		[JsonProperty]
		public int? AddressId 
		{ 
 			get {   return _addressId;  } 
 			set {   SetProperty(value,ref _addressId, () => AddressId); } 
		}

        private Address _address;
        [HandleOnNesting] [Aggregation(isComposite: true)] [ObjectValidator]
        [JsonProperty]
		public Address Address 
		{
			get {   return _address;   }
            set
            {
                _address = value;
                if (Address != null)
                    Address.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName.Equals("ZipCodeId") ||
                            args.PropertyName.Equals("ZipCode"))
                        {
                            if (Address.HasChanges())
                            {
                                Latitude = Address?.ZipCode?.Latitude;
                                Longitude = Address?.ZipCode?.Longitude;
                            }
                        }
                        OnPropertyChanged(() => DisplayLocation);
                    };
                else
                    OnPropertyChanged(() => DisplayLocation);
                OnPropertyChanged(() => Address);
            }
		}

        private int? _locationInventoryLocId;
        [JsonProperty]
        public int? LocationInventoryLocId
        {
            get { return _locationInventoryLocId; }
            set { SetProperty(value, ref _locationInventoryLocId, () => LocationInventoryLocId); }
        }
        private LocationInventoryLoc _locationInventoryLoc;
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public LocationInventoryLoc LocationInventoryLoc
        {
            get { return _locationInventoryLoc; }
            set
            {
                _locationInventoryLoc = value;
                OnPropertyChanged(() => LocationInventoryLoc);
            }
        }

        private ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();
        [HandleOnNesting] [Aggregation(isComposite: true)][ObjectCollectionValidator(typeof(Contact))]
        [JsonProperty]
        public ICollection<Contact>Contacts
        {
            get { return _contacts; }
            set { _contacts = value != null ? value.ToObservableCollection() : null; }
        }

        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged(() => SelectedContact);
            }
        }

        private Customer _customer;
        [JsonProperty, Aggregation]
        public Customer Customer
        {
            get { return _customer; }
            set {   
                _customer = value;
                OnPropertyChanged(() => Customer);
            }
        }
        public Contact FirstContact
        {
            get
            {
                if (!Contacts.Any()) return null;

                return Contacts.FirstOrDefault(c => c.IsMain);
            }
        }

        private ObservableCollection<WorkOrder> _workOrders = new ObservableCollection<WorkOrder>();
        [JsonProperty]
        public ICollection<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders = value != null ? value.ToObservableCollection() : null; }
        }
        
        public double EmptyCoordinate { get { return 0D; } }

        public bool HasAddress
        {
            get { return (Address != null); }    
        }

        public bool HasContacts
        {
            get { return (Contacts != null && Contacts.Count > 0); }
        }

        public Contact MainContact
        {
            get
            {
                return HasContacts ? Contacts.FirstOrDefault(c => c.IsMain) : null;
            }
        }
        public string DisplayLocation
        {
            get
            {
                if (Address != null)
                    return DisplayAddressLocation;

                return DisplayGpsCoordinates;
            }
        }

        public string DisplayLatitude
        {
            get { return LatitudeAsString(Latitude);    }
        }

        public string DisplayLongitude
        {
            get { return LongitudeAsString(Longitude); }
        }

        public string DisplayGpsCoordinates
        {
            get { return string.Format(Translations.LocationCoordinatesDisplay, Latitude, Longitude); }
        }

        public string DisplayAddressLocation
        {
            get
            {
                return string.Format(Translations.LocationAddressDisplay, Address.Street ?? "?",
                                    Address.HouseNumber ?? "?",
                                    Address.PostBox ?? "?",
                                    Address.ZipCode == null ? "?" : Address.ZipCode.Code,
                                    Address.ZipCode == null || Address.ZipCode.City == null ? "?" : Address.ZipCode.City);
            }
        }

        public bool HasValidGeoCoordinates
        {
            get
            {
                if (Latitude.HasValue && Longitude.HasValue)
                {
                    var latitudeIsValid = Latitude >= -90 && Latitude <= 90;
                    var longitudeIsValid = Longitude >= -180 && Longitude <= 180;
                    return latitudeIsValid && longitudeIsValid;
                }
                return false;
            }
        }
        #endregion

        #region behavior

        public static Location Create(WorkOrder workOrder)
        {
            if (workOrder == null) return null;

            var location = new Location
            {
                Address = Address.Create(),
            };
            workOrder.LocationId = -1;
            location.WorkOrders.Add(workOrder);

            var currentUser = location.GetCurrentUser(RepositoryFinder);
            if (currentUser != null)
                location.SetAuditInfo(currentUser.Login);
            
            return location;
        }

        public static Location Create(WorkOrder workOrder, User user = null)
        {
            if (workOrder == null) return null;

            var location = new Location
            {
                Address = Address.Create(),
            };
            
            if (user == null)
            {
                var currentUser = location.GetCurrentUser(RepositoryFinder);
                if (currentUser != null)
                    location.SetAuditInfo(currentUser.Login);
            }
            else
            {
                location.SetAuditInfo(user.Login);
            }
            return location;
        }

        public static Location Create(Customer customer)
        {
            var location = new Location
            {
                Address = Address.Create()
            };
            if (customer != null)
            {
                location.CustomerId = customer.Id;
                location.Customer = customer;
            }
            var user = customer.GetCurrentUser(RepositoryFinder);
            if (user != null)
                location.SetAuditInfo(user.Login);

            return location;
        }


        public static Location Create(Customer customer, User user)
        {
            if (customer == null) return null;

            var location = new Location
            {
                Address = Address.Create()
            };
            if (user == null)
            {
                var currentUser = customer.GetCurrentUser(RepositoryFinder);
                if (currentUser != null) location.SetAuditInfo(currentUser.Login);
            }
            else
            {
                location.SetAuditInfo(user.Login);
            }
            return location;
        }

        public static string LatitudeAsString(double? latitude)
        {
            if (latitude == null) return string.Empty;

            var latitudeSeconds = (int)Math.Round(latitude.Value * 3600);
            var latitudeDegrees = latitudeSeconds / 3600;
            latitudeSeconds = Math.Abs(latitudeSeconds % 3600);
            var latitudeMinutes = latitudeSeconds / 60;
            latitudeSeconds %= 60;

            return string.Format("{0}° {1}' {2}\" {3}", Math.Abs(latitudeDegrees),
                                                        latitudeMinutes,
                                                        latitudeSeconds,
                                                        latitudeDegrees >= 0 ? "N" : "S");
        }

        public static string LongitudeAsString(double? longitude)
        {
            if (longitude == null) return string.Empty;

            var longitudeSeconds = (int)Math.Round(longitude.Value * 3600);
            var longitudeDegrees = longitudeSeconds / 3600;
            longitudeSeconds = Math.Abs(longitudeSeconds % 3600);
            var longitudeMinutes = longitudeSeconds / 60;
            longitudeSeconds %= 60;

            return string.Format("{0}° {1}' {2}\" {3}", Math.Abs(longitudeDegrees),
                                                        longitudeMinutes,
                                                        longitudeSeconds,
                                                        longitudeDegrees >= 0 ? "E" : "W");
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => HasContacts, 
                              () => HasAddress);
        }

        [SelfValidation]
        public void CheckLocationInfo(ValidationResults results)
        {
            const string errorFieldLatitude = "Latitude";
            const string errorFieldLongitude = "Longitude";

            // only the coordinates or the address is obliged - but address
            // is member, when added if should be valid.
            if (Address != null || 
                (Latitude != null && Longitude != null))
            {
                ClearValidationError(errorFieldLatitude);
                ClearValidationError(errorFieldLongitude);
                return;
            }

            if (Latitude == null)
            {
                var validationResult = new ValidationResult(Translations.LocationLatitudeRequired,
                                                            this, errorFieldLatitude, null, EntityValidator);

                results.AddResult(validationResult);
                SetValidationError(validationResult);      
            }
            if (Longitude == null)
            {
                var validationResult = new ValidationResult(Translations.LocationLongitudeRequired,
                                                            this, errorFieldLongitude, null, EntityValidator);

                results.AddResult(validationResult);
                SetValidationError(validationResult);      
            }
        }

        public Region RegionFor(ContractModel contractModel)
        {
            if (contractModel == null) return null;
            if (Address?.ZipCode == null) return null;

            var addressRegionIds = Address.ZipCode.Regions.Select(r => r.Id).ToArray();
            var contractRegionIds = contractModel.Regions.Select(r => r.Id).ToArray();

            var regionId = contractRegionIds.Intersect(addressRegionIds).FirstOrDefault();

            return contractModel.Regions.FirstOrDefault(r => r.Id == regionId);
        }

        public void Clear()
        {
            Latitude = null;
            Longitude = null;

            if (Address == null) return;

            Address.Clear();
        }
        #endregion
    }
}
