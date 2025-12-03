using System;
using Application.Activities.Command;
using Application.Activities.DTOs;
using Application.Activities.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

// Define the client methods that can be called from the server
public interface ICommentHubClient
{
    // Method to receive a new comment
    Task ReceiveComment(CommentDTOs comment);
    // Method to load existing comments when a client connects
    Task LoadComments(List<CommentDTOs> comments);
}

public class CommentHub(IMediator mediator) : Hub<ICommentHubClient>
{
    public async Task SendComment(AddComment.Command command)
    {
        var result = await mediator.Send(command);
        if (result.IsSuccess)
        {
            await Clients.Group(command.ActivityId).ReceiveComment(result.Value!);
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

        await Clients.Caller.LoadComments(result.Value!); // Send existing comments to the connected client

    }
}
