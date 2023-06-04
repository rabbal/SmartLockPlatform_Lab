
// ReSharper disable InconsistentNaming

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Infrastructure.Context;

public static class ShadowProperties
{
    public static readonly HashSet<Type> SoftDeletionTypes = new();

    public const string CreatedDateTime = nameof(CreatedDateTime);
    public const string CreatedByUserId = nameof(CreatedByUserId);
    public const string CreatedByBrowserName = nameof(CreatedByBrowserName);
    public const string CreatedByIP = nameof(CreatedByIP);

    public const string ModifiedDateTime = nameof(ModifiedDateTime);
    public const string ModifiedByUserId = nameof(ModifiedByUserId);
    public const string ModifiedByBrowserName = nameof(ModifiedByBrowserName);
    public const string ModifiedByIP = nameof(ModifiedByIP);

    public const string IsDeleted = nameof(IsDeleted);

    public const string Version = nameof(Version);
    public const string Hash = nameof(Hash);

    public static void IgnoreDomainEvents(this ModelBuilder builder)
    {
        var types = builder.Model.GetEntityTypes().Where(t => typeof(IAggregateRoot).IsAssignableFrom(t.ClrType)).ToList();
        types.ForEach(t => builder.Entity(t.ClrType).Ignore(nameof(IAggregateRoot.DomainEvents)));
    }

    public static void AddTrackingFields(this ModelBuilder builder)
    {
        var types = builder.Model.GetEntityTypes().Where(t =>
            !t.IsOwned() &&
            typeof(IEntity).IsAssignableFrom(t.ClrType)).ToList();

        foreach (var type in types.Select(t => t.ClrType))
        {
            //CreationTracking

            builder.Entity(type)
                .Property<DateTime?>(CreatedDateTime)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            // builder.Entity(type)
            //     .Property<string?>(CreatedByBrowserName).HasMaxLength(1024)
            //     .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity(type)
                .Property<string?>(CreatedByIP).HasMaxLength(256)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity(type)
                .Property<long?>(CreatedByUserId)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            //ModificationTracking

            builder.Entity(type)
                .Property<DateTime?>(ModifiedDateTime);

            // builder.Entity(type)
            //     .Property<string?>(ModifiedByBrowserName).HasMaxLength(1024);

            builder.Entity(type)
                .Property<string?>(ModifiedByIP).HasMaxLength(256);

            builder.Entity(type)
                .Property<long?>(ModifiedByUserId);

            if (typeof(IAggregateRoot).IsAssignableFrom(type))
            {
                builder.Entity(type)
                    .Property<uint>(Version)
                    .IsRowVersion();
            }
        }
    }

    /// <summary>
    /// Define IsDeleted shadow property and setup respective global query filtering.
    /// You must consider filtered indices when utilizing soft deletion.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TEntity"></typeparam>
    public static void HasSoftDeletion<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        builder.Property<bool>(IsDeleted);

        builder.HasQueryFilter(obj => !EF.Property<bool>(obj, IsDeleted));

        SoftDeletionTypes.Add(typeof(TEntity));
    }

    /// <summary>
    /// Define IsDeleted shadow property and setup respective global query filtering.
    /// You must consider filtered indices when utilizing soft deletion.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TOwnerEntity"></typeparam>
    /// <typeparam name="TDependentEntity"></typeparam>
    public static void HasSoftDeletion<TOwnerEntity, TDependentEntity>(
        this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
        where TOwnerEntity : class where TDependentEntity : class
    {
        builder.Property<bool>(IsDeleted);

        Expression<Func<TDependentEntity, bool>> lambdaExpression =
            obj => !EF.Property<bool>(obj, IsDeleted);

        builder.OwnedEntityType.SetQueryFilter(lambdaExpression);

        SoftDeletionTypes.Add(typeof(TDependentEntity));
    }

    public static void WithRowIntegrity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        builder.Property<string>(Hash)
            .HasMaxLength(256);
    }
}