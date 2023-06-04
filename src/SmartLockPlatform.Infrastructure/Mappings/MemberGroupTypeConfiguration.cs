using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Infrastructure.Mappings;

public class MemberGroupTypeConfiguration : IEntityTypeConfiguration<MemberGroup>
{
    public void Configure(EntityTypeBuilder<MemberGroup> builder)
    {
        builder.ToTable(nameof(MemberGroup), SchemaNames.Customers);

        builder.Property(mg => mg.Id).UseHiLo($"{nameof(MemberGroup)}_HiLo");
        builder.Property(mg => mg.Name).HasMaxLength(256);

        builder.HasMany(mg => mg.Members).WithMany()
            .UsingEntity("GroupMembership", membership =>
            {
                membership.Property("MembersId").HasColumnName("MemberId");
            });
    }
}