using System;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Profiles.Command;

public class SetMainPhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId { get; set; }
    }

    public class Handle(AppDbContext appContext, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
    {
        async Task<Result<Unit>> IRequestHandler<Command, Result<Unit>>.Handle(Command request, CancellationToken cancellationToken)
        {

            var user = await userAccessor.GetUserWithPhotosAsync(cancellationToken);
            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (photo is null)
            {
                return Result<Unit>.Failure("Cannot find photo", StatusCodes.Status400BadRequest);
            }

            user.ImageUrl = photo.Url;

            var results = await appContext.SaveChangesAsync(cancellationToken) > 0;

            return results ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating photo", StatusCodes.Status400BadRequest);

        }
    }

}
