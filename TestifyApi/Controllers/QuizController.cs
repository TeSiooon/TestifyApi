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
    public async Task<ActionResult> CreateQuiz([FromBody] CreateQuizCommand command, CancellationToken cancellationToken)
    {
        var quizId = await mediator.Send(command, cancellationToken);
        return Ok(quizId);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizVm>>> GetAllMatchingQuizzes([FromQuery] GetAllQuizzesQuery query, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetQuizByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetQuizByIdQuery(id), cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteQuizAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new DeleteQuizCommand(id), cancellationToken));
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateQuizAsync([FromBody] UpdateQuizCommand command, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [AllowAnonymous]
    [HttpGet("top")]
    public async Task<ActionResult<IEnumerable<QuizVm>>> GetTopQuizzesAsync(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetTopQuizzesQuery(), cancellationToken));
    }
}
