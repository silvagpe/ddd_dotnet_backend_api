using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Application.Helpers;

public static class ExpressionBuilder
{
    public static Expression BuildComparisonExpression<T>(MemberExpression member, string operation, string value){
        
        var constant = Expression.Constant(Convert.ChangeType(value, member.Type));

        if (operation == "=")
        {
            if (value.StartsWith("*"))
            {                
                return Expression.Call(
                        Expression.Call(member, "ToLower", null),
                        typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        Expression.Constant(value.TrimStart('*').ToLower())
                    );
            }
            else if (value.EndsWith("*"))
            {                
                return Expression.Call(
                        Expression.Call(member, "ToLower", null),
                        typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        Expression.Constant(value.TrimEnd('*').ToLower())
                );
            }
            else
            {                
                return Expression.Equal(
                    Expression.Call(member, "ToLower", null),
                    Expression.Constant(value.ToLower())
                );
            }
        }        
        return operation switch
        {
            ">=" => Expression.GreaterThanOrEqual(member, constant),
            "<=" => Expression.LessThanOrEqual(member, constant),
            _ => throw new NotSupportedException($"Operation '{operation}' is not supported")
        };     
    }
}