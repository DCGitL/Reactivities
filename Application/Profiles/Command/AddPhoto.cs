using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Profiles.Command;

public class AddPhoto
{
    public class Command : IRequest<Result<Photo>>
    {
        public required IFormFile FormFile { get; set; }
    }

    public class Handler(IUserAccessor userAccessor, AppDbContext appContext, IPhotoService photoService) : IRequestHandler<Command, Result<Photo>>
    {
        public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
        {
            var uploadPhoto = await photoService.UpLoadFileAsync(request.FormFile, cancellationToken);
            if (uploadPhoto is null)
            {
                return Result<Photo>.Failure("Upload photo failed", StatusCodes.Status400BadRequest);
            }

            var user = await userAccessor.GetUserAsync();

            var photo = new Photo
            {
                Url = uploadPhoto.Url,
                PublicId = uploadPhoto.PublicId,
                UserId = user.Id

            };
            user.ImageUrl ??= photo.Url;

            appContext.Photos.Add(photo);

            var result = await appContext.SaveChangesAsync(cancellationToken) > 0;

            return result ? Result<Photo>.Success(photo)
            : Result<Photo>.Failure("Fail to save photo", StatusCodes.Status400BadRequest);



        }
    }

}
