using System.Collections.Concurrent;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Base;

public static class QueryableExtensions
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> Properties = new();
    private const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";

    private static readonly Regex EscapeRegex = new(EscapedCommaPattern,
        RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
    
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
            .OrderByDynamic(sorting)
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
    
    private static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string? sorting)
    {
        string? ordering = null;
        if (sorting is not null)
        {
            var sortExpressions = EscapeRegex.Split(sorting).Where(sort => !string.IsNullOrWhiteSpace(sort))
                .Select(SortExpression.FromString).ToList();

            ordering = string.Join(",", sortExpressions);
        }

        if (string.IsNullOrEmpty(ordering)) ordering = ObtainDefaultSortingExpression(typeof(T)).ToString();

        return source.OrderBy(ordering);
    }

    private static SortExpression ObtainDefaultSortingExpression(Type type)
    {
        var properties =
            Properties.GetOrAdd(type, t => t.GetProperties(BindingFlags.Instance | BindingFlags.Public));

        var property =
            Array.Find(properties, p => string.Equals(p.Name, "id", StringComparison.OrdinalIgnoreCase));
        if (property == null)
        {
            property = Array.Find(properties, p => p.PropertyType.IsPredefinedType()) ??
                       throw new NotSupportedException(
                           "There is not any public property of primitive type for sorting");
        }

        return new SortExpression(property.Name, true);
    }

    private static readonly Type[] PredefinedTypes =
    {
        typeof(object),
        typeof(bool),
        typeof(char),
        typeof(string),
        typeof(sbyte),
        typeof(byte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(DateTime),
        typeof(TimeSpan),
        typeof(Guid),
        typeof(Math),
        typeof(Convert)
    };

    private static bool IsPredefinedType(this Type type)
    {
        return PredefinedTypes.Any(t => t == type);
    }
}