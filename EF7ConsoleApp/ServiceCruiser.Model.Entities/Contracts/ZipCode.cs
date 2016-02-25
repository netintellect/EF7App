using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.FieldServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    public class ZipCode : ValidatedEntity<ZipCode>
    {
        #region state         
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
                
        private string _code;
		[JsonProperty]
		public string Code 
		{
            get { return _code; }
		    set
		    {
		        if (SetProperty(value, ref _code, () => Code))
		        {
		            OnPropertyChanged(() => DisplayZipCode);
		        }
		    } 
		}

        private string _city;
		[JsonProperty]
		public string City 
		{ 
 			get {   return _city; }
		    set
		    {
		        if (SetProperty(value, ref _city, () => City))
		        {
		            OnPropertyChanged(() => DisplayZipCode);
		        }
		    } 
		}

        private string _country;
		[JsonProperty]
        public string  Country 
		{ 
 			get {   return _country; }
		    set
		    {
		        if (SetProperty(value, ref _country, () => Country))
		        {
		            OnPropertyChanged(() => DisplayCountry);
		        }
		    } 
		}

        private double? _latitude;
        [JsonProperty]
        public double? Latitude
        {
            get { return _latitude; }
            set { SetProperty(value, ref _latitude, () => Latitude); }
        }

        private double? _longitude;
        [JsonProperty]
        public double? Longitude
        {
            get { return _longitude; }
            set { SetProperty(value, ref _longitude, () => Longitude); }
        }

        private ObservableCollection<Region> _regions = new ObservableCollection<Region>();
        [JsonProperty, Aggregation(isIndependent: true)]
        public ICollection<Region> Regions
        {
            get { return _regions; }
            set { _regions = value?.ToObservableCollection(); }
        }

        public string DisplayZipCode => $"{Code ?? "?"} - {City ?? "?"}";

        public string DisplayRegion => Regions.FirstOrDefault()?.Name;
        
        public string DisplayCountry
        {
            get { return StaticFactory.Instance.GetCountryValue(Country); }
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

        public override string ToString()
        {
            return $"ZipCode {Id} with code {Code} - {City} - {Country}";
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => City);
            OnPropertyChanged(() => Country);
        }
        #endregion
    }
}
