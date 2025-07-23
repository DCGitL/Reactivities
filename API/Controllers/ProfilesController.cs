using System.Reflection.Metadata;
using Application.Profiles.Command;
using Application.Profiles.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ProfilesController : BaseApiController
    {

        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto(IFormFile formFile, CancellationToken cancellationToken)
        {
            var command = new AddPhoto.Command
            {
                FormFile = formFile
            };
            var result = await Mediator.Send(command, cancellationToken);
            return HandleResult(result, VerbActions.Post, string.Empty);

        }

        [HttpGet("{userId}/photos")]
        public async Task<IActionResult> GetPhotoForUser(string userId)
        {
            var result = await Mediator.Send(new GetProfilePhotos.Query { UserId = userId });

            return HandleResult(result, VerbActions.Get);

        }

        [HttpDelete("{photoId}/photos")]
        public async Task<IActionResult> DeletePhoto(string photoId)
        {
            var results = await Mediator.Send(new DeletePhoto.Command { PhotoId = photoId });

            return HandleResult(results, VerbActions.Delete);
        }

        [HttpPut("{photoId}/setmain")]
        public async Task<IActionResult> SetMainPhoto(string photoId)
        {
            var result = await Mediator.Send(new SetMainPhoto.Command { PhotoId = photoId });
            return HandleResult(result, VerbActions.Put);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var result = await Mediator.Send(new GetProfile.Query { UserId = userId });

            return HandleResult(result, VerbActions.Get);

        }

        [HttpPut]
        public async Task<IActionResult> EditProfile(EditProfile.Command command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);
            return HandleResult(result, VerbActions.Put);
        }

    }
}
