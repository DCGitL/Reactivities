using System;
using System.Net;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities.Command;

public class DeleteActivity
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities.FindAsync([request.Id], cancellationToken);
            if (activity is null)
            {
                return Result<Unit>.Failure("Activity not found", (int)HttpStatusCode.NotFound);
            }
            context.Activities.Remove(activity);
            var result = await context.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result<Unit>.Success(Unit.Value);
            }

            return Result<Unit>.Failure("Failure to remove activity", (int)HttpStatusCode.BadRequest);
        }
    }

}
