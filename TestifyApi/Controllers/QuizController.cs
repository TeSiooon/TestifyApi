using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Testify.Application.Quizzes.Command.Create;
using Testify.Application.Quizzes.Command.Delete;
using Testify.Application.Quizzes.Command.Update;
using Testify.Application.Quizzes.Queries.GetAllQuizzes;
using Testify.Application.Quizzes.Queries.GetQuizById;
using Testify.Application.Quizzes.Queries.GetTopQuizzesQuery;
using Testify.Application.ViewModels;

namespace Testify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Quizzes")]
[Authorize]
public class QuizController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateQuiz([FromBody] CreateQuizCommand command)
    {
        var quizId = await mediator.Send(command);
        return Ok(quizId);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizVm>>> GetAllMatchingQuizzes([FromQuery] GetAllQuizzesQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetQuizByIdAsync([FromRoute] Guid id)
    {
        return Ok(await mediator.Send(new GetQuizByIdQuery(id)));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteQuizAsync([FromRoute] Guid id)
    {
        return Ok(await mediator.Send(new DeleteQuizCommand(id)));
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateQuizAsync([FromBody] UpdateQuizCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [AllowAnonymous]
    [HttpGet("top")]
    public async Task<ActionResult<IEnumerable<QuizVm>>> GetTopQuizzesAsync()
    {
        return Ok(await mediator.Send(new GetTopQuizzesQuery()));
    }
}
