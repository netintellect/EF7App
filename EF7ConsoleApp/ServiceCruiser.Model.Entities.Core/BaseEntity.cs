using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Repositories;
using ServiceCruiser.Model.Validations.Core.Common.Utility;

namespace ServiceCruiser.Model.Entities.Core
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndependentState
    {
        NotSet,
        Added, 
        Deleted,
        Modified
    }

    public abstract class BaseEntity : NotificationObject, IEditableObject
    {
        #region state

        public static IRepositoryFinder RepositoryFinder { get; private set; }
        private bool _useRepositoryFinder = true;
        protected bool UseRepositoryFinder
        {
            get
            {
                if (!_useRepositoryFinder) return false;
                
                return (RepositoryFinder != null);
            }
            private set
            {
                _useRepositoryFinder = value;
            }
        }
        private readonly IDictionary<string, object> _changedPropertyNames = new Dictionary<string, object>();

        [JsonProperty]
        public IDictionary<string, object> ChangedPropertyNames
        {
            get { return _changedPropertyNames; }
        }

        /// <summary>
        /// LocalId is used to temporaily add an Id (Mobile)
        /// The IsNew property takes the LocalId into account
        /// The Value of the LocalId is never serialized to the backend
        /// </summary>
        public Guid LocalId { get; set; }

        public bool IsNew
        {
            get
            {
                if (LocalId != Guid.Empty)
                {
                    return false;
                }
                // check if there is a meta data type defined that 
                // describes which property is the key property
                if (KeyInfo == null)
                {
                    object idValue = GetProperty("Id");
                    int id;
                    if (idValue != null &&
                        Int32.TryParse(idValue.ToString(), out id))
                    {
                        return id < 1;
                    }
                    return false;
                }

                // check if the key property is an id
                var attribute = KeyInfo.GetAttributesOfType<KeyAttribute>()
                    .FirstOrDefault();
                if (attribute == null) return false;
                if (!attribute.IsIdentity) return false;

                // get the property that is decorated
                var propertyInfo = GetType().GetProperty(KeyInfo.Name);
                if (propertyInfo == null) return false;

                var value = propertyInfo.GetValue(this, null);
                if (value == null) return false;

                int identifier;
                if (Int32.TryParse(value.ToString(), out identifier))
                {
                    return (identifier < 1);
                }
                return false;
            }
        }

        [IgnoreOnMap]
        public bool IsModified
        {
            get { return _changedPropertyNames.Any(); }
        }

        private bool _isDeleted;

        [JsonProperty]
        [IgnoreOnMap]
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        private IndependentState _independentState = IndependentState.NotSet;

        [JsonProperty]
        [IgnoreOnMap]
        public IndependentState IndependentState
        {
            get { return _independentState; }
            set { _independentState = value; }
        }

        private bool _isReadOnly;

        public virtual bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        protected bool IsSerializing { get; set; }

        private PropertyInfo _keyInfo;

        protected PropertyInfo KeyInfo
        {
            get
            {
                return _keyInfo ??
                       (_keyInfo = GetType().GetProperties()
                           .FirstOrDefault(a => a.GetCustomAttributes(true).OfType<KeyAttribute>().Any()));
            }
        }

        private List<PropertyInfo> _handleOnNestingList;

        protected IEnumerable<PropertyInfo> HandleOnNestingList
        {
            get
            {
                return _handleOnNestingList ??
                       (_handleOnNestingList = GetType().GetProperties()
                           .Where(mp => mp.GetCustomAttributes(true).OfType<HandleOnNestingAttribute>().Any())
                           .ToList());
            }
        }

        private List<PropertyInfo> _sharedAggregations;

        protected IEnumerable<PropertyInfo> SharedAggregations
        {
            get
            {
                return _sharedAggregations ??
                       (_sharedAggregations = GetType().GetProperties()
                           .Where(mp => mp.GetCustomAttributes(true).OfType<AggregationAttribute>()
                               .Any(a => (a.IsShared && !a.IsIndependent)))
                           .ToList());
            }
        }

        private List<PropertyInfo> _independentAggregations;

        protected IEnumerable<PropertyInfo> IndependentAggregations
        {
            get
            {
                return _independentAggregations ??
                       (_independentAggregations = GetType().GetProperties()
                           .Where(mp => mp.GetCustomAttributes(true).OfType<AggregationAttribute>()
                               .Any(a => (a.IsIndependent)))
                           .ToList());
            }
        }

        private List<PropertyInfo> _compositeAggregations;

        private IEnumerable<PropertyInfo> CompositeAggregations
        {
            get
            {
                return _compositeAggregations ??
                       (_compositeAggregations = GetType().GetProperties()
                           .Where(mp => mp.GetCustomAttributes(true).OfType<AggregationAttribute>()
                               .Any(a => !a.IsShared))
                           .ToList());
            }
        }

        private DeletePriorityAttribute _deletePriorityInfo;

        private DeletePriorityAttribute DeletePriorityInfo
        {
            get
            {
                return _deletePriorityInfo ??
                       (_deletePriorityInfo = GetType().GetCustomAttributes(true)
                           .OfType<DeletePriorityAttribute>()
                           .FirstOrDefault());
            }
        }

        protected IDictionary<string, object> AggregationsStore;

        private readonly IDictionary<string, object> _removalsStore = new Dictionary<string, object>();
        
        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                OnPropertyChanged(() => IsEditing);
            }
        }

        private bool _isAutoEdit = true;

        public bool IsAutoEdit
        {
            get { return _isAutoEdit; }
            set { _isAutoEdit = value; }
        }

        private bool _isCurrent = false;
        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                _isCurrent = value;
                OnPropertyChanged(() => IsCurrent);
            }
        }

        #endregion

        #region behavior

        public BaseEntity()
        {
        }

        public BaseEntity(PropertyInfo keyInfo)
        {
            _keyInfo = keyInfo;
        }

        public static void SetFindRepository(IRepositoryFinder repositoryFinder)
        {
            RepositoryFinder = repositoryFinder;
        }

        public override string ToString()
        {
            // check if default text is specified
            var defaultText = GetType().GetProperties()
                .FirstOrDefault(a => a.GetCustomAttributes(true).OfType<DefaultTextAttribute>().Any());

            var toString = defaultText != null ? defaultText.GetValue(this, null) as string : base.ToString();

            // if not use Key  info
            if (defaultText == null && KeyInfo != null)
            {
                toString = String.Format("Type {0} with keyinfo {1}: {2}", GetType().Name, KeyInfo.Name, GetKeyValue());
            }
            return toString ?? base.ToString();
        }

        public virtual void OnDeserializing(StreamingContext value)
        {
            UseRepositoryFinder = false;
        }

        public virtual void OnDeserialized(StreamingContext value)
        {
            UseRepositoryFinder = true;
        }

        public virtual void MarkForDeletion()
        {

        }

        #region setproperty

        public bool SetProperty<T>(T value, ref T field, Expression<Func<T>> propertyExpression, bool validate = true)
        {
            if (propertyExpression == null) return false;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return false;

            return SetProperty(value, ref field, body.Member.Name, validate);
        }

        public bool SetProperty<T>(T value, ref T field, string propertyName, bool validate = true)
        {
            ActOnPropertyChanging(propertyName);

            // check for trivial assignments
            var areSame = EqualityComparer<T>.Default.Equals(value, field);
            if (areSame && !(validate && HasValidationErrors))
                return false;

            // set field
            var oldValue = field;
            field = value;
            bool changed = false;

            if (!IsSerializing && !String.IsNullOrEmpty(propertyName))
            {
                // force the validation mechanism to refresh
                if (validate)
                    IsPropertyValid(propertyName, value);

                // Add change to history if propertychange is valid
                if (!areSame)
                {
                    SetChange(propertyName, oldValue);
                    changed = true;

                    if (!String.IsNullOrEmpty(propertyName))
                        OnPropertyChanged(propertyName);
                }
            }

            ActOnPropertyChanged(propertyName);

            return changed;
        }

        public void SetProperty(string propertyName, object value)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(propertyName);
            if (propertyInfo == null) return;

            propertyInfo.SetValue(this, value, null);
        }

        public void SetProperty<T>(Expression<Func<T>> propertyExpression, object value)
        {
            if (propertyExpression == null) return;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return;

            SetProperty(body.Member.Name, value);
        }

        public void SetProperty<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            if (propertyExpression == null) return;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return;

            SetProperty(body.Member.Name, value);
        }

        #endregion

        #region getproperty

        public object GetProperty(string propertyName)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(propertyName);
            if (propertyInfo == null) return null;

            return propertyInfo.GetValue(this, null);
        }

        public object GetProperty<TProperty, TType>(Expression<Func<TProperty, TType>> propertyExpression)
        {
            if (propertyExpression == null) return false;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return false;

            return GetProperty(body.Member.Name);
        }

        #endregion

        #region getoriginalvalue

        public object GetOriginalValue<TProperty, TType>(Expression<Func<TProperty, TType>> propertyExpression)
        {
            if (propertyExpression == null) return false;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return false;

            return GetOriginalValue(body.Member.Name);
        }

        public object GetOriginalValue<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null) return false;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return false;

            return GetOriginalValue(body.Member.Name);
        }

        public object GetOriginalValue(string propertyName)
        {
            // check if the original value has changed
            if (_changedPropertyNames.ContainsKey(propertyName))
                return _changedPropertyNames[propertyName];

            return GetProperty(propertyName);
        }

        #endregion

        #region ispropertychanged

        public bool IsPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null) return false;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return false;

            return IsPropertyChanged(body.Member.Name);
        }

        public bool IsPropertyChanged<T>(Expression<Func<T, object>> propertyExpression)
        {
            if (propertyExpression == null) return false;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return false;

            return IsPropertyChanged(body.Member.Name);
        }

        public bool IsPropertyChanged(string propertyName)
        {
            // check if the original value has changed
            return _changedPropertyNames.ContainsKey(propertyName);
        }

        public bool IsPersistableProperty(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName)) return false;

            return !GetType().GetProperty(propertyName)
                .GetCustomAttributes(typeof (IgnoreOnMapAttribute), true)
                .Any();
        }

        #endregion

        public virtual bool HasValidationErrors
        {
            get { return false; }
        }

        public int DeletePriority
        {
            get
            {
                if (DeletePriorityInfo == null) return 0;


                return DeletePriorityInfo.Priority;
            }
        }


        public bool HasCompositeAggregations
        {
            get
            {
                if (CompositeAggregations == null) return false;

                return CompositeAggregations.Any();
            }
        }

        public bool HasIndependentAssociations
        {
            get
            {
                if (IndependentAggregations == null) return false;

                return IndependentAggregations.Any();
            }
        }

        public bool IsPropertyValid<T>(Expression<Func<T>> propertyExpression, object value,
            bool forceValidation = false)
        {
            if (propertyExpression == null) return false;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return false;

            return IsPropertyValid(body.Member.Name, value, forceValidation);
        }

        public virtual bool IsPropertyValid(string propertyName, object value, bool forceValidation = false)
        {
            return true;
        }

        private void SetChange(string propertyName, object value)
        {
            if (_changedPropertyNames.All(kvp => kvp.Key != propertyName))
            {
                if (!_changedPropertyNames.Any() && IsAutoEdit) BeginEdit();
                _changedPropertyNames.Add(propertyName, value);
            }
        }

        public bool HasChanges()
        {
            bool hasChanges = IsModified || (IsNew && !IsDeleted) || (IsDeleted && !IsNew);
            if (hasChanges) return true;

            hasChanges = HaveNestedEntitiesChanges(dto => dto.HasChanges());
            if (hasChanges) return true;

            return false;
        }

        public void Delete()
        {
            _isDeleted = true;

            MarkForDeletion();
            ActOnAllNestedEntities(dto => dto.Delete());
        }

        public void UndoDelete()
        {
            _isDeleted = false;

            ActOnAllNestedEntities(dto => dto.UndoDelete());
        }

        /// <summary>
        /// For Mobile app => set the LocalId for added entities (those that have their id set to 0)
        /// </summary>
        public virtual void ApplyLocalId()
        {
            if (LocalId == Guid.Empty) LocalId = Guid.NewGuid();
        }

        protected virtual void ActOnPropertyChanging(string propertyName)
        {
        }

        protected virtual void ActOnPropertyChanged(string propertyName)
        {
        }

        protected virtual void SetIsValid()
        {
        }

        [OnDeserializing]
        public void OnDeserializating(StreamingContext value)
        {
            IsSerializing = true;

            OnDeserializing(value);
        }

        [OnDeserialized]
        public void OnDeserialization(StreamingContext value)
        {
            IsSerializing = false;

            OnDeserialized(value);
        }

        /// <summary>
        /// Method will perform a given action on all nested DTO (Recursive)
        /// </summary>
        private void ActOnAllNestedEntities(Action<BaseEntity> action)
        {
            List<PropertyInfo> propertyInfos = GetType().GetPropertiesOfType<BaseEntity>()
                .ToList();

            foreach (var propertyInfo in propertyInfos)
            {
                if (!IsPartOfHandleOnNestingList(propertyInfo)) continue;

                bool isDeleteOfIndependentAggregation = false;
                if (isDeleteAction(action))
                {
                    if (IsPartOfSharedAggregations(propertyInfo)) continue;
                    isDeleteOfIndependentAggregation = IsPartOfIndependentAggregations(propertyInfo);
                }

                object propertyValue = propertyInfo.GetValue(this, null);

                // check if we are dealing with a nested member
                var baseData = propertyValue as BaseEntity;
                if (baseData != null)
                {
                    if (isDeleteOfIndependentAggregation)
                        baseData.IndependentState = IndependentState.Deleted;
                    else
                        action(baseData);
                }
                else
                {
                    // check if we are dealing with nested members
                    var values = propertyValue as IEnumerable;
                    if (values != null)
                    {
                        foreach (var item in values)
                        {
                            var baseEntity = item as BaseEntity;
                            if (baseEntity == null) continue;
                            if (isDeleteOfIndependentAggregation)
                                baseEntity.IndependentState = IndependentState.Deleted;
                            else
                                action(baseEntity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method will perform a given action on all nested DTO (Recursive)
        /// </summary>
        private bool HaveNestedEntitiesChanges(Func<BaseEntity, bool> action)
        {
            List<PropertyInfo> propertyInfos = GetType().GetPropertiesOfType<BaseEntity>()
                .ToList();

            var result = false;
            foreach (var propertyInfo in propertyInfos)
            {
                if (!IsPartOfHandleOnNestingList(propertyInfo)) continue;

                var propertyValue = propertyInfo.GetValue(this, null);

                var baseData = propertyValue as BaseEntity;
                if (baseData != null)
                {
                    if (action(baseData)) result = true;
                }
                else
                {
                    var values = propertyValue as IEnumerable;
                    if (values != null)
                    {
                        foreach (var item in values)
                        {
                            var baseEntity = item as BaseEntity;
                            if (baseEntity == null) continue;
                            if (action(baseEntity)) result = true;
                        }
                    }
                }
            }
            return result;
        }

        protected bool IsPartOfHandleOnNestingList(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return false;
            if (HandleOnNestingList == null) return false;

            return HandleOnNestingList.Any(item => item.PropertyType.FullName.Equals(propertyInfo.PropertyType.FullName));
        }

        private bool IsPartOfSharedAggregations(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return false;
            if (SharedAggregations == null) return false;

            return SharedAggregations.Any(item => item.PropertyType.FullName.Equals(propertyInfo.PropertyType.FullName));
        }

        private bool IsPartOfIndependentAggregations(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return false;
            if (IndependentAggregations == null) return false;

            return
                IndependentAggregations.Any(
                    item => item.PropertyType.FullName.Equals(propertyInfo.PropertyType.FullName));
        }

        public bool IsPartOfCompositeAggregations(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return false;
            if (CompositeAggregations == null) return false;

            return
                CompositeAggregations.Any(item => item.PropertyType.FullName.Equals(propertyInfo.PropertyType.FullName));
        }

        private bool isDeleteAction(Action<BaseEntity> action)
        {
            if (action == null) return false;

            return action.Method.Name.Contains("Delete");
        }

        public void ShowSharedAggregations()
        {
            ShowSharedNestedAggregations();

            AggregationsStore = null;
        }


        public void HideSharedAggregations()
        {
            AggregationsStore = new Dictionary<string, object>();
            HideSharedNestedAggregations();
        }

        private void HideSharedNestedAggregations()
        {
            if (SharedAggregations != null)
            {
                SharedAggregations.ToList()
                    .ForEach(HideSharedAggregation);
            }
            ActOnAllNestedEntities(e => e.HideSharedAggregations());
        }

        private void ShowSharedNestedAggregations()
        {
            if (SharedAggregations != null)
            {
                SharedAggregations.ToList()
                    .ForEach(ShowSharedAggregation);
            }
            ActOnAllNestedEntities(e => e.ShowSharedAggregations());
        }


        private void ShowSharedAggregation(PropertyInfo metadataPropertyInfo)
        {
            if (metadataPropertyInfo == null) return;

            var propertyInfo = GetType().GetProperty(metadataPropertyInfo.Name);
            if (propertyInfo == null) return;

            var baseEntity = propertyInfo.GetValue(this, null) as BaseEntity;
            if (baseEntity != null) return;

            object storedEntity;
            if (AggregationsStore != null &&
                AggregationsStore.TryGetValue(metadataPropertyInfo.Name, out storedEntity))
            {
                propertyInfo.SetValue(this, storedEntity, null);
            }
        }

        private void HideSharedAggregation(PropertyInfo metadataPropertyInfo)
        {
            if (metadataPropertyInfo == null) return;

            var propertyInfo = GetType().GetProperty(metadataPropertyInfo.Name);
            if (propertyInfo == null) return;

            object entity = null;
            var baseEntity = propertyInfo.GetValue(this, null) as BaseEntity;
            if (baseEntity != null) entity = baseEntity;
            var baseEntities = propertyInfo.GetValue(this, null) as ICollection;
            if (baseEntities != null) entity = baseEntities;

            if (AggregationsStore.ContainsKey(metadataPropertyInfo.Name)) return;

            AggregationsStore.Add(metadataPropertyInfo.Name, entity);

            propertyInfo.SetValue(this, null, null);
        }

        protected void RemoveChangedPropertyName(string propertyName)
        {
            if (_changedPropertyNames == null) return;
            if (!_changedPropertyNames.Any()) return;
            if (!_changedPropertyNames.ContainsKey(propertyName)) return;

            _changedPropertyNames.Remove(propertyName);
        }

        public IEnumerable<PropertyInfo> GetCompositeAggregations()
        {
            return CompositeAggregations;
        }

        public IEnumerable<PropertyInfo> GetIndependentAccociations()
        {
            return IndependentAggregations;
        }

        public IEnumerable<string> GetChangedPropertyNames()
        {
            return _changedPropertyNames.Keys;
        }

        public BaseEntity DeepClone()
        {
            return MemberwiseClone() as BaseEntity;
        }

        public TEntity ShallowClone<TEntity>() where TEntity : BaseEntity, new()
        {
            var clonedEntity = new TEntity();

            foreach (var propertyInfo in GetType().GetProperties()
                .Where(p => p.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().Any()))
            {
                if (propertyInfo.IsCollectionOf<BaseEntity>()) continue;
                if (!propertyInfo.PropertyType.IsValueType) continue;
                if (!propertyInfo.CanRead || !propertyInfo.CanWrite) continue;

                clonedEntity.SetProperty(propertyInfo.Name, GetProperty(propertyInfo.Name));
            }

            return clonedEntity;
        }

        public TEntity ShallowMerge<TEntity>(TEntity targetEntity) where TEntity : BaseEntity, new()
        {
            foreach (var propertyInfo in GetType().GetProperties()
                                                  .Where(p => p.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().Any()))
            {
                if (propertyInfo.IsCollectionOf<BaseEntity>()) continue;
                if (!propertyInfo.PropertyType.IsValueType) continue;
                if (!propertyInfo.CanRead || !propertyInfo.CanWrite) continue;

                targetEntity.SetProperty(propertyInfo.Name, GetProperty(propertyInfo.Name));
            }
            targetEntity.EndEdit();

            return targetEntity;
        }

        public object GetKeyValue()
        {
            if (KeyInfo == null) return null;

            return GetProperty(KeyInfo.Name);
        }

        private void CancelEditProperties()
        {
            var propertyNames = _changedPropertyNames.Keys;
            foreach (var propertyName in propertyNames)
            {
                object value = _changedPropertyNames[propertyName];

                SetProperty(propertyName, value);
            }
        }

        private void CancelEditMembers()
        {
            CompositeAggregations.ToList()
                                 .ForEach(pi =>
                                 {
                                    var collection = GetProperty(pi.Name) as IList;
                                    if (collection != null)
                                    {
                                        var newEntities = collection.Cast<object>().OfType<BaseEntity>()
                                                                    .Where(entity => entity.IsNew)
                                                                    .ToList();
                                        newEntities.ForEach(collection.Remove);
                                    }
                                 });
        }

    #endregion

        #region IEditableObject methods
        public virtual void BeginEdit()
        {
            IsEditing = true;
            if (IsAutoEdit)
                ActOnAllNestedEntities(e => e.BeginEdit());
        }

        public virtual void CancelEdit()
        {
            // change the properties back to their original value
            CancelEditProperties();
            
            // check if deleted, if yes rollback
            if (_isDeleted) _isDeleted = false;
           
            if (IsAutoEdit)
                ActOnAllNestedEntities(e => e.CancelEdit());

            // all the state of the members is cancelled - now handle the collection 
            // (only add - delete is handle by entity.)
            CancelEditMembers();

            EndEdit();
        }

        public virtual void EndEdit()
        {
            IndependentState = IndependentState.NotSet;
            
            _changedPropertyNames.Clear();
            _removalsStore.Clear();
            
            IsEditing = false;
            if (IsAutoEdit)
                ActOnAllNestedEntities(e => e.EndEdit());
        }
        #endregion
    }
}
