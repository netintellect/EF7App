using System;

namespace ServiceCruiser.Model.Entities.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AggregationAttribute : Attribute
    {
        #region state
        /// <summary>
        /// Indicated is the association is shared or composed. In case of a composed aggregation the lifetime
        /// will be handled by the parent object. With a shared association the lifetime of the parent is managed
        /// outside the object.
        /// </summary>
        private readonly bool _isShared;
        public bool IsShared
        {
            get { return _isShared; }
        }

        private readonly bool _isIndependent;

        public bool IsIndependent
        {
            get { return _isIndependent; }
        }

        /// <summary>
        /// Only used when IsShared = true. When a relation is marked as shared the lifetime is managed outside
        /// the object (delete / create) but it is still possible that the state of the shared object is changed
        /// and we want to persist these changes at the same moment of the persistence of the object
        /// </summary>
        private readonly bool _doesParticipate;
        public bool DoesParticipate
        {
            get { return _doesParticipate; }
        }

        private string _key;
        public string Key
        {
            get { return _key;}
        }
        #endregion

        #region behavior

        public AggregationAttribute(bool isComposite = false, bool isIndependent = false, bool doesParticipate = false, string key = null)
        {
            _isShared = !isComposite;
            _doesParticipate = doesParticipate;
            _isIndependent = isIndependent;
            _key = key;
        }

        #endregion
    }
}
