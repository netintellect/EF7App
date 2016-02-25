using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttributeGroup : ValidatedEntity<AttributeGroup>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private string _name;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "AttributeGroupName")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(value, ref _name, () => Name); }
        }

        private ObservableCollection<AttributeSpec> _attributeSpecs = new ObservableCollection<AttributeSpec>();
        [HandleOnNesting][Aggregation(isComposite: true)][ObjectCollectionValidator(typeof(AttributeSpec))]
        [JsonProperty]
        public ICollection<AttributeSpec> AttributeSpecs
        {
            get { return _attributeSpecs; }
            set { _attributeSpecs = value != null ? value.ToObservableCollection() : null; }
        }

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(value, ref _isVisible, () => IsVisible); }
        }

        #endregion

        #region behavior

        public static AttributeGroup Create()
        {
            return new AttributeGroup { AttributeSpecs = new Collection<AttributeSpec>() };
        }

        public AttributeSpec FindAttributeSpec(string code)
        {
            if (AttributeSpecs == null || !AttributeSpecs.Any())
                return null;

            var specInGroup = AttributeSpecs.FirstOrDefault(a => a.Code == code);
            if (specInGroup != null)
                return specInGroup;

            return AttributeSpecs.SelectMany(attributeSpec => attributeSpec.SubGroupRules)
                                   .Select(attributeRule => attributeRule.Target)
                                   .Select(attributeGroup => attributeGroup.FindAttributeSpec(code))
                                   .FirstOrDefault(attributeSpec => attributeSpec != null);
        }

        public IEnumerable<AttributeSpec> GetAllAttributeSpecs(bool visibleOnly = false)
        {
            if (AttributeSpecs == null || !AttributeSpecs.Any())
                return null;

            //Flatten out the AttributeSpecs
            var attributeSpecList = new List<AttributeSpec>(AttributeSpecs);

            IEnumerable<IEnumerable<AttributeSpec>> childattributeSpecLists;
            if (visibleOnly)
                childattributeSpecLists = AttributeSpecs.SelectMany(attributeSpec => attributeSpec.VisibleSubGroupRules)
                                                        .Select(attributeRule => attributeRule.Target.GetAllAttributeSpecs());
            else
                childattributeSpecLists = AttributeSpecs.SelectMany(attributeSpec => attributeSpec.SubGroupRules)
                                                        .Select(attributeRule => attributeRule.Target.GetAllAttributeSpecs());

            foreach (var childattributeSpecList in childattributeSpecLists)
            {
                attributeSpecList.AddRange(childattributeSpecList);
            }

            return attributeSpecList;
        }

        public void Refresh()
        {
            OnPropertyChanged(() => AttributeSpecs);
        }
        #endregion
    }
}
