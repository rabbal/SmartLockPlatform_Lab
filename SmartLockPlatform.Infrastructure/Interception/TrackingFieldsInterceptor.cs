using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Infrastructure.Context;

namespace SmartLockPlatform.Infrastructure.Interception;

public class TrackingFieldsInterceptor : SaveChangesInterceptor
{
    private readonly IUserIdentitySession _identitySession;
    private readonly IDateTime _dateTime;

    public TrackingFieldsInterceptor(IUserIdentitySession identitySession, IDateTime dateTime)
    {
        _identitySession = identitySession;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        PopulateTrackingFields(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        PopulateTrackingFields(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private void PopulateTrackingFields(DbContext? context)
    {
        if (context is null) return;

        var now = _dateTime.Now;
        foreach (var entry in context.ChangeTracker.Entries<IEntity>().Where(e => !e.Metadata.IsOwned()))
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    PopulateCreationTracking(entry);
                    break;
                case EntityState.Modified: //TODO: check owned properties` states
                    PopulateModificationTracking(entry);
                    break;
                case EntityState.Deleted:
                    if (ShadowProperties.SoftDeletionTypes.Contains(entry.Metadata.ClrType) == false)
                    {
                        break;
                    }

                    entry.State =
                        EntityState.Unchanged; //NOTE: For soft-deletes to work with the original `Remove` method.

                    PopulateModificationTracking(entry);

                    entry.Property(ShadowProperties.IsDeleted).CurrentValue = true;
                    break;
            }
        }

        void PopulateModificationTracking(EntityEntry entry)
        {
            entry.Property(ShadowProperties.ModifiedDateTime).CurrentValue = now;
            entry.Property(ShadowProperties.ModifiedByIP).CurrentValue = _identitySession.UserIP;
            entry.Property(ShadowProperties.ModifiedByUserId).CurrentValue = _identitySession.UserId;
        }

        void PopulateCreationTracking(EntityEntry entry)
        {
            entry.Property(ShadowProperties.CreatedDateTime).CurrentValue = now;
            entry.Property(ShadowProperties.CreatedByIP).CurrentValue = _identitySession.UserIP;
            entry.Property(ShadowProperties.CreatedByUserId).CurrentValue = _identitySession.UserId;
        }
    }
}