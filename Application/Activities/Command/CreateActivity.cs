using System;
using System.Net;
using Application.Activities.DTOs;
using Application.Core;
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

    public class Handler(AppDbContext context) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {

            var activity = request.ActivityDto.FromDto();
            context.Activities.Add(activity);
            var result = await context.SaveChangesAsync(cancellationToken);
            if (result < 1)
            {
                return Result<string>.Failure("Activity fail to save", (int)HttpStatusCode.BadRequest);
            }
            return Result<string>.Success(activity.Id);
        }
    }
}
