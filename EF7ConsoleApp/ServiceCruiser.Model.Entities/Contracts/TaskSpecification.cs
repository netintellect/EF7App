using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Resources;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TaskSpecification : ValidatedEntity<TaskSpecification>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private string _code;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "TaskSpecificationCode")]
        public string Code
        {
            get { return _code; }
            set {   SetProperty(value, ref _code, () => Code);  }
        }


        private string _description;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "TaskSpecificationDescription")]
        public string Description
        {
            get { return _description; }
            set {   SetProperty(value, ref _description, () => Description);    }
        }


        private int? _workSpecId;
        [JsonProperty]
        public int? WorkSpecId
        {
            get {   return _workSpecId; }
            set {   SetProperty(value, ref _workSpecId, () => WorkSpecId);  }
        }
        
        private int? _onsiteTime;
        [JsonProperty]
        public int? OnsiteTime
        {
            get {   return _onsiteTime; }
            set {   SetProperty(value, ref _onsiteTime, () => OnsiteTime);  }
        }
        
        private int? _resultCategoryId;
        [JsonProperty]
        public int? ResultCategoryId
        {
            get {   return _resultCategoryId; }
            set {   SetProperty(value, ref _resultCategoryId, () => ResultCategoryId);  }
        }


        private WorkSpecification _workSpecification;
        [JsonProperty]
        public WorkSpecification WorkSpecification
        {
            get { return _workSpecification; }
            set 
            {   
                _workSpecification = value;
                OnPropertyChanged(() => WorkSpecification);
            }
        }

        private ObservableCollection<TaskInfo> _taskInfos = new ObservableCollection<TaskInfo>();
        [JsonProperty]
        public ICollection<TaskInfo> TaskInfos
        {
            get { return _taskInfos; }
            set { _taskInfos = value?.ToObservableCollection(); }
        }

        private ResultCategory _resultCategory;
        [JsonProperty]
        public ResultCategory ResultCategory
        {
            get { return _resultCategory; }
            set 
            {   
                _resultCategory = value;
                OnPropertyChanged(() => ResultCategory);
            }
        }

        private int? _attributeSpecId;
        [JsonProperty]
        public int? AttributeSpecId
        {
            get { return _attributeSpecId; }
            set { SetProperty(value, ref _attributeSpecId, () => AttributeSpecId); }
        }
        private AttributeGroup _attributeSpecs;
        [JsonProperty]
        public AttributeGroup AttributeSpecs
        {
            get { return _attributeSpecs; }
            set { SetProperty(value, ref _attributeSpecs, () => AttributeSpecs); }
        }

        #endregion
    }
}
