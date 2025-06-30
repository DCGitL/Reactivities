using System;
using System.Net;
using Application.Activities.DTOs;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Command;

public class EditActivity
{
    public class Command : IRequest<Result<Unit>>
    {
        public required EditActivityDto ActivityDto { get; set; }

    }
    public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
    {

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activityExisting = await context.Activities.FindAsync([request.ActivityDto.Id], cancellationToken);
            if (activityExisting is null)
            {
                return Result<Unit>.Failure("Activity not found", (int)HttpStatusCode.NotFound);
            }
            activityExisting.ToExistingActivity(request.ActivityDto);

            var result = await context.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result<Unit>.Success(Unit.Value);
            }

            return Result<Unit>.Failure("Failure to update activity => possible no changes were made", (int)HttpStatusCode.BadRequest);

        }
    }
}
