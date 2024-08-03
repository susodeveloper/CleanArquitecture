using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Infrastructure.Authentication;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null) return Task.CompletedTask;
        
        HashSet<string> permisisons = context.User.Claims
            .Where(c => c.Type == CustomClaims.Permissions)
            .Select(c => c.Value).ToHashSet();

        if (permisisons.Contains(requirement.Permission)) context.Succeed(requirement);
        
        return Task.CompletedTask;

    }
}