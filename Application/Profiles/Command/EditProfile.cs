using System;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Profiles.Command;

public class EditProfile
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
    }

    public class Handler(IUserAccessor userAccessor, AppDbContext appDbContext) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserAsync();
            if (user is null)
            {
                return Result<Unit>.Failure("User not found", StatusCodes.Status404NotFound);
            }

            user.DisplayName = request.DisplayName;
            user.Bio = request.Bio;
            var result = await appDbContext.SaveChangesAsync(cancellationToken) > 0;
            if (result)
            {
                return Result<Unit>.Success(Unit.Value);
            }

            return Result<Unit>.Failure("Failed to update profile", StatusCodes.Status400BadRequest);


        }
    }

}
