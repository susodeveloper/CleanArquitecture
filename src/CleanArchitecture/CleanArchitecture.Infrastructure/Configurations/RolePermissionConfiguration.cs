using CleanArchitecture.Domain.Permisions;
using CleanArchitecture.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;
public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("roles_permissions");
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.Property(pi => pi.PermissionId)
        .HasConversion( p => p!.Value, p => new PermissionId(p));

        builder.HasData(
            Create(Role.Cliente, PermissionEnum.ReadUser),
            Create(Role.Admin, PermissionEnum.WriteUser),
            Create(Role.Admin, PermissionEnum.UpdateUser),
            Create(Role.Admin, PermissionEnum.ReadUser)
        );
    }

    private static RolePermission Create(Role role, PermissionEnum permissionEnum)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = new PermissionId((int)permissionEnum)
        };
    }
}