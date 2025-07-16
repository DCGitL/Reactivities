using System;
using Application.Profiles;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPhotoService
{
    public Task<PhotoUploadResult?> UpLoadFileAsync(IFormFile formFile, CancellationToken cancellationToken);
    public Task<string> DeletePhoto(string publicId);
}
