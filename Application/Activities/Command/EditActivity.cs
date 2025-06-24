using System;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Command;

public class EditActivity
{
    public class Command : IRequest
    {
        public required Activity Activity { get; set; }

    }
    public class Handler(AppDbContext context) : IRequestHandler<Command>
    {

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activityExisting = await context.Activities.FindAsync([request.Activity.Id], cancellationToken) ?? throw new Exception("Activity not found");
            activityExisting.ToExistingActivity(request.Activity);
            await context.SaveChangesAsync(cancellationToken);

        }
    }
}
