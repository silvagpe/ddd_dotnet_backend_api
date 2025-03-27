using System.Linq.Expressions;
using DeveloperStore.Application.Helpers;
namespace DeveloperStore.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> query, string field, string operation, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var member = Expression.PropertyOrField(parameter, field);
        var constant = Expression.Constant(Convert.ChangeType(value, member.Type));
        var comparison = operation switch
        {
            ">=" => Expression.GreaterThanOrEqual(member, constant),
            "<=" => Expression.LessThanOrEqual(member, constant),
            "=" => (Expression)(value.StartsWith("*") 
                    ? Expression.Call(
                        Expression.Call(member, "ToLower", null),
                        typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        Expression.Constant(value.TrimStart('*').ToLower())
                    )
                    : value.EndsWith("*")
                    ? Expression.Call(
                        Expression.Call(member, "ToLower", null),
                        typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        Expression.Constant(value.TrimEnd('*').ToLower())
                    )
                    : Expression.Equal(
                        Expression.Call(member, "ToLower", null),
                        Expression.Constant(value.ToLower())
                    )),
            _ => throw new NotSupportedException($"Operation '{operation}' is not supported")
        };

        var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
        return query.Where(lambda);
    }

    public static IQueryable<T> WhereDynamic2<T>(this IQueryable<T> query, string field, string operation, string value)
    {
        var comparison = ExpressionBuilder.BuildComparisonExpression<T>(field, operation, value);
        var parameter = Expression.Parameter(typeof(T), "x");
        var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
        return query.Where(lambda);
    }

    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string order)
    {
        var fields = order.Split(',');
        bool isFirstOrder = true;

        foreach (var field in fields)
        {
            var parts = field.Trim().Split(' ');
            var fieldName = parts[0];
            var direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? "Descending" : "Ascending";

            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.PropertyOrField(parameter, fieldName);
            var lambda = Expression.Lambda(member, parameter);

            if (isFirstOrder)
            {
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == (direction == "Descending" ? "OrderByDescending" : "OrderBy") && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), member.Type);

                query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
                isFirstOrder = false;
            }
            else
            {
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == (direction == "Descending" ? "ThenByDescending" : "ThenBy") && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), member.Type);

                query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
            }
        }

        return query;
    }
}