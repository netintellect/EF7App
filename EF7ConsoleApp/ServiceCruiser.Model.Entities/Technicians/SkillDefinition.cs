using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Resources;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Technicians
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SkillDefinition : ValidatedEntity<SkillDefinition>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		[Display(ResourceType = typeof(Translations), Name ="SkillDefinitionId")]
		public int Id 
		{ 
 			get {   return _id; } 
 			set
            {
				SetProperty(value, ref _id, () => Id);
			} 
		}
    			

		private string _name;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="SkillDefinitionName")]
		public string Name 
		{ 
 			get {   return _name;   } 
 			set
            {
				SetProperty(value, ref _name, () => Name);
			} 
		}
    			
		private bool _isCertificationNeeded;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="SkillDefinitionIsCertificationNeeded")]
		public bool IsCertificationNeeded 
		{ 
 			get {   return _isCertificationNeeded;  } 
 			set
            {
				SetProperty(value, ref _isCertificationNeeded, () => IsCertificationNeeded);
			} 
		}
    			
		private int _contractModelId;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="SkillDefinitionContractModelId")]
		public int ContractModelId 
		{ 
 			get {   return _contractModelId;   } 
 			set
            {
				SetProperty(value,ref _contractModelId, () => ContractModelId);
			} 
		}
    
		private ContractModel _contractModel;
		[JsonProperty]
		public ContractModel ContractModel 
		{
			get {   return _contractModel;  } 
			set
            {
				_contractModel= value;
				OnPropertyChanged(() => ContractModel);
		    } 
		}
        
		private ObservableCollection<Skill> _skills = new ObservableCollection<Skill>();
		[JsonProperty]
		public ICollection<Skill> Skills 
		{
			get {   return _skills;    } 
			set {   _skills= value != null ? value.ToObservableCollection() : null;}
        }
        #endregion

        #region behavior
        
        #endregion
    }
}
