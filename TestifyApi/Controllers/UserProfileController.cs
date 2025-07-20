using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Testify.Application.Users.Queries.GetUserProfile;

namespace Testify.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "UserProfile")]
[Authorize]
public class UserProfileController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserProfileAsync()
    {
        return Ok(await mediator.Send(new GetUserProfileQuery()));
    }
}
