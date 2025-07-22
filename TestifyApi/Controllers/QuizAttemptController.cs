using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Testify.Application.QuizAttempts.Command.Finish;
using Testify.Application.QuizAttempts.Command.Start;
using Testify.Application.QuizAttempts.Command.Submit;
using Testify.Application.QuizAttempts.Queries.GetNextQuestion;
using Testify.Application.QuizAttempts.Queries.GetQuizAttemptResult;

namespace Testify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "QuizAttempts")]
[Authorize]
public class QuizAttemptController(IMediator mediator) : ControllerBase
{
    [HttpPost("start")]
    public async Task<ActionResult> StartQuizAsync([FromBody] StartQuizAttemptCommand command, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpGet("{attemptId}/next")]
    public async Task<ActionResult> GetNextQuestion([FromRoute]Guid attemptId, CancellationToken cancellationToken)
    {
        var questionVm = await mediator.Send(new GetNextQuestionQuery(attemptId), cancellationToken);
        if (questionVm == null)
            return NoContent();
        return Ok(questionVm);
    }

    [HttpPost("{attemptId}/answer")]
    public async Task<IActionResult> SubmitAnswer(
    [FromRoute] Guid attemptId,
    [FromBody] SubmitAnswerCommand command, CancellationToken cancellationToken)
    {
        if (command.AttemptId != attemptId)
            return BadRequest("AttemptId mismatch.");

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{attemptId}/finish")]
    public async Task<IActionResult> FinishAttempt([FromRoute] Guid attemptId, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new FinishAttemptCommand(attemptId), cancellationToken));
    }

    [HttpGet("{resultId}/result")]
    public async Task<IActionResult> GetAttemptResultAsync([FromRoute] Guid resultId, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetQuizAttemptResultQuery(resultId), cancellationToken));
    }
}
