using Microsoft.EntityFrameworkCore;

namespace SmartLockPlatform.Application.Base;

public interface IAppDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task Complete(CancellationToken cancellationToken = default);
}