using System;

namespace ServiceCruiser.Model.Entities.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationPropertyAttribute:Attribute
    {
        public string Name { get; set; }

        public ValidationPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
