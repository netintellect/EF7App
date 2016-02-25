using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResultCategory : ValidatedEntity<ResultCategory>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
		private string _name;
		[JsonProperty]
		public string Name 
		{ 
 			get {   return _name;   } 
 			set {   SetProperty(value,ref _name, () => Name);   } 
		}

        // TODO (dirk) dit is eigenlijk ParentCategoryId, zie ook Map comment
		private int? _resultCategoryId;
		[JsonProperty]
		public int? ResultCategoryId 
		{ 
 			get {   return _resultCategoryId;   } 
 			set {   SetProperty(value,ref _resultCategoryId, () => ResultCategoryId);   } 
		}
        
		private ObservableCollection<ResultCategory> _subCategories = new ObservableCollection<ResultCategory>();
		[JsonProperty]
		public ICollection<ResultCategory> SubCategories 
		{
			get {   return _subCategories;  } 
			set {   _subCategories= value != null ? value.ToObservableCollection() : null;  } 
		}
        
		private ResultCategory _parentCategory;
		[JsonProperty]
		public ResultCategory ParentCategory 
		{
			get {   return _parentCategory; } 
			set 
            {   
                _parentCategory= value;
				OnPropertyChanged(() => ParentCategory);
			} 
		}
        
		private ObservableCollection<WorkSpecification> _workSpecifications = new ObservableCollection<WorkSpecification>();
		[JsonProperty]
		public ICollection<WorkSpecification> WorkSpecifications 
		{
			get {   return _workSpecifications; } 
			set {   _workSpecifications= value != null ? value.ToObservableCollection() : null; } 
		}
        
		private ObservableCollection<TaskSpecification> _taskSpecifications = new ObservableCollection<TaskSpecification>();
		[JsonProperty]
		public ICollection<TaskSpecification> TaskSpecifications 
		{
			get {   return _taskSpecifications; } 
			set {   _taskSpecifications= value != null ? value.ToObservableCollection() : null; } 
		}
        
		private ObservableCollection<ResultCode> _resultCodes = new ObservableCollection<ResultCode>();
		[JsonProperty]
		public ICollection<ResultCode> ResultCodes 
		{
			get {   return _resultCodes;    } 
			set {   _resultCodes= value != null ? value.ToObservableCollection() : null;    }
        }
        #endregion
    }
}
