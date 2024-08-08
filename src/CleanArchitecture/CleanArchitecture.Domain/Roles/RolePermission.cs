using CleanArchitecture.Domain.Permisions;

namespace CleanArchitecture.Domain.Roles;
public sealed class RolePermission
{
    public int RoleId { get; set; }
    public PermissionId? PermissionId { get; set; }
}