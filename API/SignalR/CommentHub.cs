using System;
using Application.Activities.Command;
using Application.Activities.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class CommentHub(IMediator mediator) : Hub
{
    public async Task SendComment(AddComment.Command command)
    {
        var result = await mediator.Send(command);
        if (result.IsSuccess)
        {
            await Clients.Group(command.ActivityId).SendAsync("ReceiveComment", result.Value);
        }
        else
        {
            throw new HubException(result.Error);
        }
    }
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var activityId = httpContext?.Request.Query["activityId"].ToString();
        if (string.IsNullOrEmpty(activityId))
        {
            throw new HubException("Activity ID is required to connect to the CommentHub.");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

        var result = await mediator.Send(new GetComments.Query { ActivityId = activityId });

        await Clients.Caller.SendAsync("LoadComments", result.Value);

    }
}
