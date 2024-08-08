
using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("users_roles");
        builder.HasKey(ur => new { ur.RoleId, ur.userId });

        builder.Property(ui => ui.userId)
        .HasConversion(userId => userId!.Value, id => new UserId(id));
    }
}
