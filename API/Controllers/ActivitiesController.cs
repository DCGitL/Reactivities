using Application.Activities.Command;
using Application.Activities.DTOs;
using Application.Activities.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetActivities(CancellationToken cancellationToken)
    {

        var activities = await Mediator.Send(new GetActivityList.Query(), cancellationToken);  // context.Activities.ToListAsync();
        return Ok(activities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityDetail(string id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetActivityDetails.Query { Id = id }, cancellationToken);

        return HandleResult(result, VerbActions.Get);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(CreateActivityDto activityDto, CancellationToken cancellationToken)
    {
        var resultId = await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto }, cancellationToken); // context.Activities.AddAsync(activity);

        return HandleResult(resultId, VerbActions.Post);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(string id, EditActivityDto activity, CancellationToken cancellationToken)
    {
        // if (id != activity.Id) return BadRequest("Activity ID mismatch");

        var result = await Mediator.Send(new EditActivity.Command { ActivityDto = activity }, cancellationToken); // context.Activities.Update(activity);

        return HandleResult(result, VerbActions.Put);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(string id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteActivity.Command { Id = id }, cancellationToken); // var activity = await context.Activities.FindAsync(id);

        return HandleResult(result, VerbActions.Delete);
    }
}
