using Microsoft.EntityFrameworkCore;

namespace SmartLockPlatform.Application.Base;

public static class DbSetExtensions
{
    public static ValueTask<TEntity?> FindById<TEntity, TKey>(this DbSet<TEntity> dbSet, TKey id,
        CancellationToken cancellationToken = default) where TEntity : class where TKey : IEquatable<TKey>
    {
        return dbSet.FindAsync(new object[] { id }, cancellationToken);
    }
}