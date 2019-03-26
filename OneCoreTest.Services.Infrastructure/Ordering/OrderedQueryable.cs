using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.Services.Infrastructure.Ordering
{
    /// <summary>
    /// Ordering query helpers
    /// </summary>
    public static class OrderedQueryable
    {
        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty); // I don't care about some naming
            MemberExpression property;

            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields. 
                String[] childProperties = propertyName.Split('.');
                var childProperty = typeof(T).GetProperty(childProperties[0]);
                property = Expression.MakeMemberAccess(param, childProperty);

                for (int i = 1; i < childProperties.Length; i++)
                {
                    Type t = childProperty.PropertyType;
                    if (!t.IsGenericType)
                    {
                        childProperty = t.GetProperty(childProperties[i]);
                    }
                    else
                    {
                        childProperty = t.GetGenericArguments().First().GetProperty(childProperties[i]);
                    }

                    property = Expression.MakeMemberAccess(property, childProperty);
                }
            }
            else
            {
                property = Expression.PropertyOrField(param, propertyName);
            }

            LambdaExpression sort = Expression.Lambda(property, param);

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, false);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, false);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, true);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, true);
        }
    }
}
