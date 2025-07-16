using System;
using Application.Interfaces;
using Application.Profiles;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Photos;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> cloudinaryOptions)
    {
        var account = new Account(cloud: cloudinaryOptions.Value.CloudName,
             apiKey: cloudinaryOptions.Value.ApiKey,
             apiSecret: cloudinaryOptions.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);

    }
    public async Task<string> DeletePhoto(string publicId)
    {
        var deleteParam = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParam);
        if (result.Error != null)
        {
            throw new Exception(result.Error.Message);
        }

        return result.Result;
    }

    public async Task<PhotoUploadResult?> UpLoadFileAsync(IFormFile formFile, CancellationToken cancellationToken)
    {

        if (formFile.Length > 0)
        {
            await using var stream = formFile.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(formFile.FileName, stream),
                Folder = "Reactivity"



            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);
            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return new PhotoUploadResult
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };

        }
        return null;

    }
}
