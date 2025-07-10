using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Testify.IntegrationTests.Helpers;

public static class HttpContextHelper
{
    public static HttpContext PrepareTestHttpContext(
    Guid userId,
    string userName,
    string email)
    {
        var httpContext = new DefaultHttpContext();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Email, email),
        };

        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "UserAccess.IntegrationTests"));

        return httpContext;
    }
}
