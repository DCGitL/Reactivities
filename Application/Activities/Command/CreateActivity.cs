using System;
using System.Net;
using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities.Command;

public class CreateActivity
{

    public class Command : IRequest<Result<string>>
    {
        public required CreateActivityDto ActivityDto { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {

            var user = await userAccessor.GetUserAsync();

            var activity = request.ActivityDto.FromDto();
            context.Activities.Add(activity);
            var attendee = new ActivityAttendee
            {
                ActivityId = activity.Id,
                UserId = user.Id,
                IsHost = true
            };
            activity.Attendees.Add(attendee);

            var result = await context.SaveChangesAsync(cancellationToken);
            if (result < 1)
            {
                return Result<string>.Failure("Activity fail to save", (int)HttpStatusCode.BadRequest);
            }
            return Result<string>.Success(activity.Id);
        }
    }
}
