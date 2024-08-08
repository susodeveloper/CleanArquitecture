using CleanArchitecture.Domain.Permisions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Roles;
public class Role : Enumeration<Role>
{
    public static readonly Role Admin = new Role(1, "Admin");
    public static readonly Role Cliente = new Role(2, "Cliente");

    private Role(int id, string name) : base(id, name)
    {

    }

    public ICollection<Permission>? Permissions { get; set; }
}