using System;

namespace ServiceCruiser.Model.Entities.Core.Attributes
{
    /// <summary>
    /// Use this attribute to specify for a part of the object graph (and composite aggregations)
    /// what would be the order of deletion to avoid foreign key constraint exceptions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DeletePriorityAttribute : Attribute
    {
        #region public properties
        private readonly int _priority;
        public int Priority
        {
            get { return _priority; }
        }
        #endregion

        #region Ctor

        public DeletePriorityAttribute(int priority)
        {
            _priority = priority;
        }

        #endregion
    }
}
