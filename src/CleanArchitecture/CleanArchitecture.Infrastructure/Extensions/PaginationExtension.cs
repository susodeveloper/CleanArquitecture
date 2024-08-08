using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.Extensions;

public static class PaginationExtension
{
    public static IQueryable<T> OrderByPropertyOrField<T>(this IQueryable<T> queryable, string propertyOrFieldName, bool ascending)
    {
        var elementType = typeof(T);
        var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";
        var parameterExpression = Expression.Parameter(elementType);
        var propertyOrFiledExpression = Expression
            .PropertyOrField(parameterExpression, propertyOrFieldName);

        var selector = Expression.Lambda(propertyOrFiledExpression, parameterExpression);

        var orderByExp = Expression.Call(typeof(Queryable), orderByMethodName,new[] { elementType, propertyOrFiledExpression.Type },
            queryable.Expression, selector);

        return queryable.Provider.CreateQuery<T>(orderByExp);
    }
}