using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
            ?? throw new InvalidOperationException("Mediator service is not registered.");

        protected IActionResult HandleResult<T>(Result<T> result, VerbActions actions)
        {
            if (!result.IsSuccess && result.Code == StatusCodes.Status404NotFound)
            {
                return NotFound();
            }
            if ((result.IsSuccess && result.Value != null) && (actions == VerbActions.Get) || actions == VerbActions.Delete)
            {
                return Ok(result.Value);
            }
            if ((result.IsSuccess && result.Value != null) && actions == VerbActions.Post)
                return CreatedAtAction("GetActivityDetail", new { id = result.Value }, result.Value);
            if ((result.IsSuccess && result.Value != null) && actions == VerbActions.Put)
                return NoContent();

            return BadRequest(result.Error);

        }
    }

    public enum VerbActions
    {
        Post,
        Put,
        Get,

        Delete
    }
}
