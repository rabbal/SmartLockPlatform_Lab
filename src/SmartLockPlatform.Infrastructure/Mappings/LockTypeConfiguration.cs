using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Infrastructure.Mappings;

public class LockTypeConfiguration : IEntityTypeConfiguration<Lock>
{
    public void Configure(EntityTypeBuilder<Lock> builder)
    {
        builder.ToTable(nameof(Lock), SchemaNames.Customers);

        builder.Property(l => l.Id).UseHiLo($"{nameof(Lock)}_HiLo");
        builder.Property(l => l.Name).HasMaxLength(256);
        builder.Property(l => l.Uuid).HasMaxLength(128);

        builder.HasIndex(l => l.Uuid).IsUnique().HasDatabaseName("UIX_Lock_Uuid");

        builder.HasMany(l => l.Rights).WithOne().OnDelete(DeleteBehavior.Restrict);
    }
}