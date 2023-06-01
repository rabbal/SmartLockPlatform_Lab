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

        const string passwordHash =
            "AQAAAAEAACcQAAAAEGXzevuZegI9XRHQNl7MRxQR/3b7SzCDoI6/ZDsMoAq/7iH+vsOiDnaW8TnqUvUtDQ=="; //@dmiN123

        const long useId = -1L;

        builder.HasData(new
        {
            Id = useId,
            PasswordHash = passwordHash,
            EmailConfirmed = false,
            IsActive = true,
            IsAdmin = true,
            LockoutEnabled = true,
            FailedLoginCount = 0,
            MobileConfirmed = false,
            SecurityStampToken = "381049F7-A730-49A8-A196-5F5BACB4F4DB"
        });

        builder.OwnsOne(u => u.Email,
            email =>
            {
                email.Property(e => e.Value).HasColumnName(nameof(User.Email)).HasMaxLength(128);
                email.HasIndex(e => e.Value).IsUnique().HasDatabaseName("UIX_User_Email");
                //TODO: email.HasIndex(e => e.Value).HasMethod("gin").HasDatabaseName("IX_User_Email");

                email.HasData(new { Value = "admin@example.com", UserId = useId });
            });
        builder.OwnsOne(u => u.FirstName,
            name =>
            {
                name.Property(n => n.Value).HasMaxLength(50).HasColumnName(nameof(User.FirstName));
                name.HasData(new { Value = "super", UserId = useId });
            });
        builder.OwnsOne(u => u.LastName,
            name =>
            {
                name.Property(n => n.Value).HasMaxLength(50).HasColumnName(nameof(User.LastName));
                name.HasData(new { Value = "admin", UserId = useId });
            });
    }
}