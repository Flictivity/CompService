using System.Linq.Expressions;

namespace CompService.Database.Extensions;

public static class DbSetExtension
{
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> query,
        Expression<Func<TSource, TKey>> property, bool desc)
    {
        return !desc ? query.OrderBy(property) : query.OrderByDescending(property);
    }
}