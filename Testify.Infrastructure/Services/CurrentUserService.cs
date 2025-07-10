using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Testify.Application.Common;

namespace Testify.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        Guid.Parse(httpContextAccessor.HttpContext!
                          .User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
}