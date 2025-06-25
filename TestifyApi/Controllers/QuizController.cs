using MediatR;
using Microsoft.AspNetCore.Mvc;
using Testify.Application.Quizzes.Create;

namespace Testify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Quizzes")]
//[Authorize]
public class QuizController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizCommand command)
    {
        var quizId = await mediator.Send(command);
        return Ok(quizId);
    }
}
