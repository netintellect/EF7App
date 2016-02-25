using System;
using System.ComponentModel.DataAnnotations;
using ServiceCruiser.Model.Validations.Core.Common;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    public abstract class BaseValidationAttribute : ValidationAttribute
    {
        private string _ruleset;
        private string _messageTemplate;
        private string _messageTemplateResourceName;
        private Type _messageTemplateResourceType;
        private string _tag;
        
        public string GetMessageTemplate()
        {
            if (null != _messageTemplate)
                return _messageTemplate;
            if (null != _messageTemplateResourceName && null != _messageTemplateResourceType)
            {
                return ResourceStringLoader.LoadString(_messageTemplateResourceType.FullName,
                                                       _messageTemplateResourceName,
                                                       _messageTemplateResourceType.Assembly);
            }
            if (null != _messageTemplateResourceName || null != _messageTemplateResourceType)
                throw new InvalidOperationException(Translations.ExceptionPartiallyDefinedResourceForMessageTemplate);

            return null;
        }

        public string Ruleset
        {
            get { return _ruleset ?? string.Empty; }
            set { _ruleset = value; }
        }

        
        public string MessageTemplate
        {
            get { return _messageTemplate; }
            set
            {
                if (_messageTemplateResourceName != null)
                    throw new InvalidOperationException(Translations.ExceptionCannotSetResourceMessageTemplatesIfResourceTemplateIsSet);
                if (_messageTemplateResourceType != null)
                    throw new InvalidOperationException(Translations.ExceptionCannotSetResourceMessageTemplatesIfResourceTemplateIsSet);
                _messageTemplate = value;
            }
        }

        public string MessageTemplateResourceName
        {
            get { return _messageTemplateResourceName; }
            set
            {
                if (_messageTemplate != null)
                {
                    throw new InvalidOperationException(Translations.ExceptionCannotSetResourceBasedMessageTemplatesIfTemplateIsSet);
                }
                _messageTemplateResourceName = value;
            }
        }
        
        public Type MessageTemplateResourceType
        {
            get { return _messageTemplateResourceType; }
            set
            {
                if (_messageTemplate != null)
                {
                    throw new InvalidOperationException(Translations.ExceptionCannotSetResourceBasedMessageTemplatesIfTemplateIsSet);
                }
                _messageTemplateResourceType = value;
            }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return null;
        }
    }
}
