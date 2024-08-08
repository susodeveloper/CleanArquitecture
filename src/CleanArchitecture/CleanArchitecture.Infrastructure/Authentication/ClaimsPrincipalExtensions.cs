using System.Security.Claims;
using CleanArchitecture.Domain.Users;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace CleanArchitecture.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserEmail(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(JwtRegisteredClaimNames.Email) ?? throw new ApplicationException("Email no disponible");
    }

    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : throw new ApplicationException("UserId no disponible");
    }

}