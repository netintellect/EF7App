using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Resources;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Technicians
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Skill : ValidatedEntity<Skill>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        [Display(ResourceType = typeof(Translations), Name = "SkillId")]
        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(value, ref _id, () => Id);
            }
        }

        private double _proficiency;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "SkillProficiency")]
        public double Proficiency
        {
            get { return _proficiency; }
            set
            {
                SetProperty(value, ref _proficiency, () => Proficiency);
            }
        }

        private bool _isCertified;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "SkillIsCertified")]
        public bool IsCertified
        {
            get { return _isCertified; }
            set
            {
                SetProperty(value, ref _isCertified, () => IsCertified);
            }
        }

        private DateTimeOffset _validFrom;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "SkillValidFrom")]
        public DateTimeOffset ValidFrom
        {
            get { return _validFrom; }
            set
            {
                SetProperty(value, ref _validFrom, () => ValidFrom);
            }
        }

        private DateTimeOffset? _validUntil;

        [JsonProperty]
        [Display(ResourceType = typeof (Translations), Name = "SkillValidUntil")]
        public DateTimeOffset? ValidUntil
        {
            get { return _validUntil; }
            set
            {
                SetProperty(value, ref _validUntil, () => ValidUntil);
            }
        }

        private int _skillDefinitionId;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "SkillSkillDefinitionId")]
        public int SkillDefinitionId
        {
            get { return _skillDefinitionId; }
            set
            {
                SetProperty(value, ref _skillDefinitionId, () => SkillDefinitionId);
            }
        }

        private int _technicianRoleId;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "SkillTechnicianRoleId")]
        public int TechnicianRoleId
        {
            get { return _technicianRoleId; }
            set
            {
                SetProperty(value, ref _technicianRoleId, () => TechnicianRoleId);
            }
        }

        private SkillDefinition _skillDefinition;
        [JsonProperty]
        public SkillDefinition SkillDefinition
        {
            get { return _skillDefinition; }
            set
            {
                _skillDefinition = value;
                OnPropertyChanged(() => SkillDefinition);
            }
        }

        private TechnicianRole _technicianRole;
        [JsonProperty]
        public TechnicianRole TechnicianRole
        {
            get { return _technicianRole; }
            set
            {
                _technicianRole = value;
                OnPropertyChanged(() => TechnicianRole);
            }
        }
        #endregion

        #region behavior
        
        #endregion
    }
}
