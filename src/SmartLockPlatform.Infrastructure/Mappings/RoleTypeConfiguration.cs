using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Infrastructure.Mappings;

public class RoleTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role), SchemaNames.Customers);

        builder.Property(r => r.Id).UseHiLo($"{nameof(Role)}_HiLo");
        builder.Property(r => r.Name).HasMaxLength(50);

        const string siteId = "SiteId";
        builder.Property<long>(siteId);
        
        builder.HasIndex(siteId, nameof(Role.Name)).IsUnique().HasDatabaseName("UIX_Role_SiteId_Name");

        builder.OwnsMany(r => r.Permissions, p =>
        {
            p.ToTable(nameof(RolePermission), SchemaNames.Customers);
            p.Property(r => r.Id).UseHiLo($"{nameof(RolePermission)}_HiLo");
            builder.Property(r => r.Name).HasMaxLength(50);
        });
    }
}