using MediatR;
using Microsoft.AspNetCore.Mvc;
using Testify.Application.Quizzes.Create;
using Testify.Application.Quizzes.Queries.GetQuizById;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuizByIdAsync([FromRoute] Guid id)
    {
        return Ok(await mediator.Send(new GetQuizByIdQuery(id)));
    }
}
