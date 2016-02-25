using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;
using System;
using System.Linq;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Attachment : ValidatedEntity<Attachment>
    {
        #region state
        private int _id;
        [JsonProperty, Core.Attributes.Key(isIdentity:true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }

        private AttachmentType _type;
        [DomainValidator(new object[] { AttachmentType.None}, Negated = true,
                         MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "AttachmentTypeRequired")]
        [Display(ResourceType = typeof(Translations), Name = "AttachmentType")]
        [JsonProperty]
        public AttachmentType Type
        {
            get { return _type; }
            set
            {
                if (SetProperty(value, ref _type, () => Type))
                {
                    OnPropertyChanged(() => Type);
                    OnPropertyChanged(() => IsImage);
                    OnPropertyChanged(() => Filter);
                }
            }
        }

        private string _name;
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Ignore,
                               MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "AttachmentNameRequired")]
        [Display(ResourceType = typeof(Translations), Name = "AttachmentName")]
        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set
            {
                if (SetProperty(value, ref _name, () => Name))
                {
                    OnPropertyChanged(() => Extension);
                }
            }
        }

        private DateTimeOffset _attachedDate;
        [JsonProperty]
        public DateTimeOffset AttachedDate
        {
            get { return _attachedDate; }
            set { SetProperty(value, ref _attachedDate, () => AttachedDate); }
        }

        private int _attachedById;
        [JsonProperty]
        public int AttachedById
        {
            get { return _attachedById; }
            set { SetProperty(value, ref _attachedById, () => AttachedById); }
        }

        private User _attachedBy;
        [HandleOnNesting, Aggregation]
        [JsonProperty]
        public User AttachedBy
        {
            get { return _attachedBy; }
            set
            {
                _attachedBy = value;
                OnPropertyChanged(() => AttachedBy);
            }
        }

        private int _attachedByRoleId;
        [JsonProperty]
        public int AttachedByRoleId
        {
            get { return _attachedByRoleId; }
            set { SetProperty(value, ref _attachedByRoleId, () => AttachedByRoleId); }
        }

        private UserRole _attachedByRole;
        [HandleOnNesting, Aggregation]
        [JsonProperty]
        public UserRole AttachedByRole
        {
            get { return _attachedByRole; }
            set
            {
                _attachedByRole = value;
                OnPropertyChanged(() => AttachedByRole);
            }
        }

        private byte[] _fileBinary;
        [JsonProperty, IgnoreOnMap]
        public byte[] FileBinary
        {
            get { return _fileBinary; }
            set { SetProperty(value, ref _fileBinary, () => FileBinary); }
        }

        private string _filePath;
        [JsonProperty, IgnoreOnMap]
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged(() => FilePath);
            }
        }

        [JsonProperty, IgnoreOnMap]
        public int ContractModelId { get; set; }

        public string DisplayExecutor
        {
            get
            {
                return string.Format(Translations.AttachmentTitleExecutor, AttachedByRole != null ? AttachedByRole.DisplayRole : string.Empty, 
                                                                           AttachedBy != null ? AttachedBy.DisplayName :  "?",
                                                                           AttachedDate.ToString("d"));
            }
        }

        public bool IsImage
        {
            get
            {
                return (Type == AttachmentType.Bmp ||
                        Type == AttachmentType.Gif ||
                        Type == AttachmentType.Tiff ||
                        Type == AttachmentType.Jpg ||
                        Type == AttachmentType.Png);
            }
        }

        public bool IsOfficeProduct
        {
            get
            {
                return (Type == AttachmentType.Doc ||
                        Type == AttachmentType.Docx ||
                        Type == AttachmentType.Xls ||
                        Type == AttachmentType.Xlsx ||
                        Type == AttachmentType.Ppt ||
                        Type == AttachmentType.Pptx);
            }
        }

        public string Extension
        {
            get
            {
                return string.IsNullOrEmpty(Name) ? null : Name.Split('.').Last().ToLower();
            }
        }

        public string Filter
        {
            get 
            {
                switch(Type)
                {
                    case AttachmentType.Bmp:
                        return "Bmp Files|*.bmp";        
                    case AttachmentType.Gif:
                        return "Gif Files|*.gif";
                    case AttachmentType.Tiff:
                        return "Tiff Files|*.tiff";
                    case AttachmentType.Jpg:
                        return "Jpg Files|*.jpg";
                    case AttachmentType.Png:
                        return "Png Files|*.png";
                    case AttachmentType.Doc:
                        return "Doc Files|*.doc";
                    case AttachmentType.Docx:
                        return "Docx Files|*.docx";
                    case AttachmentType.Xls:
                        return "Xls Files|*.xls";
                    case AttachmentType.Xlsx:
                        return "Xlsx Files|*.xlsx";
                    case AttachmentType.Ppt:
                        return "Ppt Files|*.ppt";
                    case AttachmentType.Pptx:
                        return "Pptx Files|*.pptx";
                    case AttachmentType.Zip:
                        return "Zip Files|*.zip";
                    default:
                        return "All Files|*.*";
                }
            }
        }
        
        #endregion

        #region behavior
        public void SetType(string extension)
        {
            if (string.IsNullOrEmpty(extension)) return;
            extension = extension.Replace(".", "");

            var type = AttachmentType.None;
            if (Enum.TryParse(extension, true, out type))
            {
                Type = type;
            }
        }
        
        public static TAttachment Create<TAttachment>(User currentUser, UserRole currentRole) where TAttachment: Attachment, new()
        {
            if (currentUser == null) return null;
            if (currentRole == null) return null;
            
            var attachment = new TAttachment
            {
                AttachedDate = DateTime.Now,
                AttachedById = currentUser.Id,
                AttachedBy = currentUser,
                AttachedByRoleId = currentRole.Id,
                AttachedByRole = currentRole
            };
            attachment.SetAuditInfo(currentUser.Login);

            return attachment;
        }

        public void UpdateFileBinary(Attachment attachment)
        {
            if (attachment == null || attachment.FileBinary == null) return;

            // set the new file binary
            FileBinary = attachment.FileBinary;
            ContractModelId = attachment.ContractModelId;

            // update the audit trail
            AttachedDate = attachment.AttachedDate;
            AttachedById = attachment.AttachedById;
            AttachedBy = attachment.AttachedBy;
            AttachedByRoleId = attachment.AttachedByRoleId;
            AttachedByRole = attachment.AttachedByRole;
        }
        #endregion
    }
}
