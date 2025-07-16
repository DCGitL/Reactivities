using System;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application.Profiles.Command;

public class DeletePhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId { get; set; }
    }

    public class Handle(AppDbContext appContext, IUserAccessor userAccessor, IPhotoService photoService) : IRequestHandler<Command, Result<Unit>>
    {
        async Task<Result<Unit>> IRequestHandler<Command, Result<Unit>>.Handle(Command request, CancellationToken cancellationToken)
        {

            var user = await userAccessor.GetUserWithPhotosAsync(cancellationToken);
            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (photo is null)
            {
                return Result<Unit>.Failure("Cannot find photo", StatusCodes.Status400BadRequest);
            }

            if (photo.Url == user.ImageUrl)
            {
                return Result<Unit>.Failure("Cannot delete you main photo", StatusCodes.Status400BadRequest);

            }

            await photoService.DeletePhoto(photo.PublicId);

            user.Photos.Remove(photo);
            var results = await appContext.SaveChangesAsync(cancellationToken) > 0;

            return results ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to remove photo from repository", StatusCodes.Status400BadRequest);

        }
    }

}
