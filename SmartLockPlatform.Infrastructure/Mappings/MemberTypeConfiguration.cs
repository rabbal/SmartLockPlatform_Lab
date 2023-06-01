using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Infrastructure.Mappings;

public class MemberTypeConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable(nameof(Member), SchemaNames.Customers);

        builder.Property(m => m.Id).UseHiLo($"{nameof(Member)}_HiLo");
        builder.Property(m => m.Alias).HasMaxLength(256);
        builder.Property(m => m.BlockedReason).HasMaxLength(1024);

        builder.HasOne(m => m.User).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
}