using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Infrastructure.Mappings;

public class LockRightTypeConfiguration : IEntityTypeConfiguration<LockRight>
{
    public void Configure(EntityTypeBuilder<LockRight> builder)
    {
        builder.ToTable(nameof(LockRight), SchemaNames.Customers);

        builder.Property(right => right.Id).UseHiLo($"{nameof(LockRight)}_HiLo");

        builder.OwnsOne(right => right.Timeframe, timeframe =>
        {
            timeframe.OwnsOne(tf => tf.Date);
            timeframe.OwnsOne(tf => tf.Time);
            timeframe.OwnsOne(tf => tf.Days);
        });

        builder.HasOne(right => right.Group).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
}