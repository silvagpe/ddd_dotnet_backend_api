using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Application.Helpers;

public static class ExpressionBuilder
{
    public static Expression BuildComparisonExpression<T>(string field, string operation, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var member = Expression.PropertyOrField(parameter, field);

        if (operation == "=")
        {
            if (value.StartsWith("*"))
            {
                // Termina com (LIKE '%value')
                var likeValue = "%" + value.TrimStart('*').ToLower();
                return Expression.Call(
                    typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new[] { typeof(DbFunctions), typeof(string), typeof(string) }),
                    Expression.Constant(EF.Functions),
                    Expression.Call(member, "ToLower", null),
                    Expression.Constant(likeValue)
                );
            }
            else if (value.EndsWith("*"))
            {
                // Começa com (LIKE 'value%')
                // var likeValue = value.TrimEnd('*').ToLower() + "%";
                // return Expression.Call(
                //     typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new[] { typeof(DbFunctions), typeof(string), typeof(string) }),
                //     Expression.Constant(EF.Functions),
                //     Expression.Call(member, "ToLower", null),
                //     Expression.Constant(likeValue)
                // );

                return Expression.Call(
                    Expression.Call(member, "ToLower", null),
                    typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                    Expression.Constant(value.TrimEnd('*').ToLower())
                );
            }
            else
            {
                // Igualdade (case-insensitive)
                return Expression.Equal(
                    Expression.Call(member, "ToLower", null),
                    Expression.Constant(value.ToLower())
                );
            }
        }

        var constant = Expression.Constant(Convert.ChangeType(value, member.Type));
        return operation switch
        {
            ">=" => Expression.GreaterThanOrEqual(member, constant),
            "<=" => Expression.LessThanOrEqual(member, constant),
            _ => throw new NotSupportedException($"Operation '{operation}' is not supported")
        };
    }
}