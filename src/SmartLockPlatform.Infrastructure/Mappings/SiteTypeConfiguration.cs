using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Infrastructure.Mappings;

public class SiteTypeConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable(nameof(Site), SchemaNames.Customers);

        builder.Property(s => s.Id).UseHiLo($"{nameof(Site)}_HiLo");

        builder.Property(s => s.Name).IsRequired().HasMaxLength(256);
        //builder.Property(s => s.Timezone).HasConversion(t => t.Id, id => TimeZoneInfo.FindSystemTimeZoneById(id));

        builder.HasIndex(s => s.Name).IsUnique().HasDatabaseName("UIX_Site_Name");

        builder.HasOne(s => s.Owner).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(s => s.Members).WithOne().OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(s => s.Groups).WithOne().OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(s => s.Locks).WithOne().OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(s => s.Roles).WithOne().OnDelete(DeleteBehavior.Restrict);
    }
}