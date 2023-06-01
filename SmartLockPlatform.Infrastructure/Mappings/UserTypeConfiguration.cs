using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLockPlatform.Domain.Identity;

namespace SmartLockPlatform.Infrastructure.Mappings;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), SchemaNames.Customers);

        builder.Property(u => u.Id).UseHiLo($"{nameof(User)}_HiLo");

        builder.OwnsOne(u => u.Email,
            email =>
            {
                email.Property(e => e.Value).HasColumnName(nameof(User.Email)).HasMaxLength(128);
                email.HasIndex(e => e.Value).IsUnique().HasDatabaseName("UIX_User_Email");
                //TODO: email.HasIndex(e => e.Value).HasMethod("gin").HasDatabaseName("IX_User_Email");
            });
        builder.OwnsOne(u => u.FirstName,
            name => { name.Property(n => n.Value).HasMaxLength(50).HasColumnName(nameof(User.FirstName)); });
        builder.OwnsOne(u => u.LastName,
            name => { name.Property(n => n.Value).HasMaxLength(50).HasColumnName(nameof(User.LastName)); });
    }
}