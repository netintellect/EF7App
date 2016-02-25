using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Linq;
using ServiceCruiser.Model.Validations.Core.Common.Utility;

namespace ServiceCruiser.Model.Entities.Core.Data
{
    public class InclusionDataFilter<TEntity> : DataFilter where TEntity: BaseEntity
    {
        #region state
        private readonly List<Expression<Func<TEntity, object>>> _includeMembers;

        [JsonIgnore]
        public Expression<Func<TEntity, bool>> EntityFilter { get; set; } 

        [JsonIgnore]
        public List<Expression<Func<TEntity, object>>> IncludeMembers
        {
            get { return _includeMembers; }
        }

        private readonly List<string> _includedPaths; 
        public IEnumerable<string> IncludedPaths
        {
            get
            {
              
                if (IncludeMembers == null) return null;
                if (!IncludeMembers.Any())
                {
                    return _includedPaths;
                }

                var propertyNames = new List<string>();
                IncludeMembers.ForEach(ip =>
                {
                    string inclusionPath = null;
                    if (TryParsePath(ip.Body, out inclusionPath))
                    {
                        if (!string.IsNullOrEmpty(inclusionPath))
                            propertyNames.Add(inclusionPath);
                    }            
                });
                return propertyNames;
            }
            set
            {
                if (value == null) return;
                _includedPaths.Clear();
                _includedPaths.AddRange(value);         
            }
        }

        private string _entityFilterString;

        public string EntityFilterString
        {
            get { return EntityFilter == null ? _entityFilterString : EntityFilter.ToString(); }
            set { _entityFilterString = value; }
        }
        #endregion

        #region behavior
        public InclusionDataFilter()
        {
            _includeMembers = new List<Expression<Func<TEntity, object>>>();
            _includedPaths = new List<string>();
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("FilteringId"))
                {
                    if (FilteringId != null)
                        EntityFilter = Combine(EntityFilter, entity => ((int) (entity.GetKeyValue() ?? 0) == FilteringId));
                }
            };
        }

        // the data filter will mainly be used to specify filters on the client, using the properties
        // of an entity (more safe than the stringed property names), but json won't be able to serialize
        // the expression trees to the back-end, so we are going to cheat and convert them to the string path
        // because EF understands this syntact as well.
        public bool TryParsePath(Expression expression, out string path)
        {
            path = null;
            var withoutConvert = RemoveConvert(expression); 
            var memberExpression = withoutConvert as MemberExpression;
            var callExpression = withoutConvert as MethodCallExpression;

            if (memberExpression != null)
            {
                var thisPart = memberExpression.Member.Name;
                string parentPart;
                if (!TryParsePath(memberExpression.Expression, out parentPart))
                {
                    return false;
                }
                path = parentPart == null ? thisPart : (parentPart + "." + thisPart);
            }
            else if (callExpression != null)
            {
                if (callExpression.Method.Name == "Select"
                    && callExpression.Arguments.Count == 2)
                {
                    string parentPart;
                    if (!TryParsePath(callExpression.Arguments[0], out parentPart))
                    {
                        return false;
                    }
                    if (parentPart != null)
                    {
                        var subExpression = callExpression.Arguments[1] as LambdaExpression;
                        if (subExpression != null)
                        {
                            string thisPart;
                            if (!TryParsePath(subExpression.Body, out thisPart))
                            {
                                return false;
                            }
                            if (thisPart != null)
                            {
                                path = parentPart + "." + thisPart;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            return true;
        }

        public static Expression RemoveConvert(Expression expression)
        {
            while (expression.NodeType == ExpressionType.Convert
                   || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            return expression;
        }

        protected static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> filter1,
                                                           Expression<Func<T, bool>> filter2)
        {
            if (filter1 == null && filter2 == null) return null;
            if (filter1 == null) return filter2;
            if (filter2 == null) return filter1;

            // combine two predicates:
            // need to rewrite one of the lambdas, swapping in the parameter from the other
            var rewrittenBody1 = new ReplaceVisitor(filter1.Parameters[0],
                                                    filter2.Parameters[0]).Visit(filter1.Body);
            var newFilter = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(rewrittenBody1, filter2.Body), filter2.Parameters);
            return newFilter;
        }
        #endregion
    }
}
