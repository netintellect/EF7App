using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ServiceCruiser.Model.Entities.Core.Data;

namespace ServiceCruiser.Model.Entities.Core.Extensibility
{
    public static class QueryableExtensions
    {
        #region extensions
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> collection, string orderColumn, SortDirectionType orderDirection)
        {
            return ParseOrderBy(orderColumn, orderDirection).Aggregate(collection, ApplyOrderBy);
        }
        #endregion

        #region implementation
        private static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> collection, OrderByInfo orderByInfo)
        {
            string[] props = orderByInfo.PropertyName.Split('.');
            Type type = typeof(T);

            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            string methodName;

            if (!orderByInfo.Initial && collection is IOrderedQueryable<T>)
            {
                if (orderByInfo.Direction == SortDirectionType.Ascending)
                    methodName = "ThenBy";
                else
                    methodName = "ThenByDescending";
            }
            else
            {
                if (orderByInfo.Direction == SortDirectionType.Ascending)
                    methodName = "OrderBy";
                else
                    methodName = "OrderByDescending";
            }

            //TODO: apply caching to the generic methodsinfos?
            return (IOrderedQueryable<T>)typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { collection, lambda });

        }

        private static IEnumerable<OrderByInfo> ParseOrderBy(string orderColumn, SortDirectionType orderDirection)
        {
            if (String.IsNullOrEmpty(orderColumn))
                yield break;

            string[] items = orderColumn.Split(',');
            bool initial = true;
            foreach (string item in items)
            {
                string[] pair = item.Trim().Split(' ');

                if (pair.Length > 2)
                    throw new ArgumentException(String.Format("Invalid OrderBy string '{0}'. Order By Format: Property, Property2 ASC, Property2 DESC", item));

                string prop = pair[0].Trim();

                if (String.IsNullOrEmpty(prop))
                    throw new ArgumentException("Invalid Property. Order By Format: Property, Property2 ASC, Property2 DESC");

                yield return new OrderByInfo { PropertyName = prop, Direction = orderDirection, Initial = initial };

                initial = false;
            }

        }

        private class OrderByInfo
        {
            public string PropertyName { get; set; }
            public SortDirectionType Direction { get; set; }
            public bool Initial { get; set; }
        }
        
        private static IQueryable<T> Page<T>(IQueryable<T> queryable, int take, int skip)
        {
            return queryable.Skip(skip).Take(take);
        }
        #endregion
    }
}
