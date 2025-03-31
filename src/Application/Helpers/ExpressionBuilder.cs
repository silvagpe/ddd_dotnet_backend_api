using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Application.Helpers;

public static class ExpressionBuilder
{
    public static Expression BuildComparisonExpression<T>(
        MemberExpression member, 
        string operation, 
        string originalValue, 
        object? convertedValue = null){
        
        var constant = Expression.Constant(convertedValue ?? Convert.ChangeType(originalValue, member.Type));

        if (operation == "=")
        {
            if (originalValue.StartsWith("*"))
            {                
                return Expression.Call(
                        Expression.Call(member, "ToLower", null),
                        typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        Expression.Constant(originalValue.TrimStart('*').ToLower())
                    );
            }
            else if (originalValue.EndsWith("*"))
            {                
                return Expression.Call(
                        Expression.Call(member, "ToLower", null),
                        typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        Expression.Constant(originalValue.TrimEnd('*').ToLower())
                );
            }
            else
            {                
                return Expression.Equal(
                    Expression.Call(member, "ToLower", null),
                    Expression.Constant(originalValue.ToLower())
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