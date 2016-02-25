using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Product : ValidatedEntity<Product>
    {
        #region state 
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			

		private string _commercialName;
		[JsonProperty]
		public string CommercialName 
		{ 
 			get {   return _commercialName; } 
 			set {   SetProperty(value,ref _commercialName, () => CommercialName);   } 
		}
    			

		private LicensingType _licensingType;
		[JsonProperty]
		public LicensingType LicensingType 
		{ 
 			get {   return _licensingType;  } 
 			set {   SetProperty(value,ref _licensingType, () => LicensingType); } 
		}
    			
 		private ObservableCollection<Module> _modules = new ObservableCollection<Module>();
		[JsonProperty]
        [HandleOnNesting] [Aggregation(isComposite:true)]
		public ICollection<Module> Modules 
		{
			get {   return _modules;    } 
			set {   _modules= value != null ? value.ToObservableCollection() : null;    } 
		}
        #endregion
    }
}
