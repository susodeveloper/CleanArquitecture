using CleanArchitecture.Domain.Permisions;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Infrastructure.Authentication;
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionEnum permission) :base(policy: permission.ToString())
    {
        
    }
}
