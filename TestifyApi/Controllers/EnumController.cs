using Microsoft.AspNetCore.Mvc;
using Testify.Common;

namespace Testify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Enums")]
public class EnumController : ControllerBase
{
    private readonly EnumHelper provider;
    public EnumController()
    {
        provider = new EnumHelper();
    }

    [HttpGet]
    public IActionResult GetEnums()
    {
        var enums = provider.GetAllEnums();
        return Ok(enums);
    }
}
