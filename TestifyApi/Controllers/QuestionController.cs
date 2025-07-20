using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Testify.Application.Questions.Command.AddQuestionToQuiz;
using Testify.Application.Questions.Command.Delete;
using Testify.Application.Questions.Command.Update;

namespace Testify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Questions")]
[Authorize]
public class QuestionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddQuestionToQuizAsync([FromBody] AddQuestionToQuizCommand command, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteQuestionFromQuizAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new DeleteQuestionCommand(id), cancellationToken));
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateQuestionAsync([FromBody] UpdateQuestionCommand command, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }
}
