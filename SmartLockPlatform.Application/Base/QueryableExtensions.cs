using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Base;

public static class QueryableExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedList<T>(this IQueryable<T> source, IPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        var sorting = request.Sorting;
        var top = request.Top;
        var skip = request.Skip;

        if (string.IsNullOrWhiteSpace(sorting))
        {
            sorting = "Id Desc";
        }

        if (top > 100) top = 100;

        //TODO: implement parsing util to parse dynamic odata style filtering
        // source = source.Where(filterExpression);

        var items = await source
            .OrderBy(sorting) //TODO: support SortExpression
            .Skip(skip)
            .Take(top)
            .ToListAsync(cancellationToken);

        long? count = null;
        if (request.IncludeCount)
        {
            count = await source.LongCountAsync(cancellationToken);
        }

        return new PaginatedList<T>(items, count);
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, bool>> predicate)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        return condition
            ? query.Where(predicate)
            : query;
    }

    public static Task<TEntity?> FindById<TEntity, TKey>(this IQueryable<TEntity> source, TKey id,
        CancellationToken cancellationToken = default) where TEntity : Entity<TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        return source.SingleOrDefaultAsync(IdEqualityExpression<TEntity, TKey>(id), cancellationToken);
    }

    public static Task<TEntity?> FindWithCriteria<TEntity>(this IQueryable<TEntity> source,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) where TEntity : class
    {
        return source.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    private static Expression<Func<TEntity, bool>> IdEqualityExpression<TEntity, TKey>(TKey id)
        where TEntity : Entity<TKey>
        where TKey : IEquatable<TKey>
    {
        var instanceExpression = Expression.Parameter(typeof(TEntity));

        var bodyExpression = Expression.Equal(
            Expression.PropertyOrField(instanceExpression, nameof(Entity<TKey>.Id)),
            Expression.Constant(id, typeof(TKey))
        );

        return Expression.Lambda<Func<TEntity, bool>>(bodyExpression, instanceExpression);
    }
}