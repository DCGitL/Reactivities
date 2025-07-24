using System;
using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Command;

public class AddComment
{
    public class Command : IRequest<Result<CommentDTOs>>
    {
        public required string ActivityId { get; set; }
        public required string Body { get; set; }
    }

    public class Handler(AppDbContext appDbContext, IUserAccessor userAccessor) : IRequestHandler<Command, Result<CommentDTOs>>
    {
        public async Task<Result<CommentDTOs>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await appDbContext.Activities
                 .Include(a => a.Comments)
                 .ThenInclude(a => a.User)
                 .FirstOrDefaultAsync(a => a.Id == request.ActivityId, cancellationToken);

            if (activity == null)
            {
                return Result<CommentDTOs>.Failure("Activity not found", StatusCodes.Status404NotFound);
            }
            var user = await userAccessor.GetUserAsync();
            var comment = new Comment
            {
                UserId = user.Id,
                ActivityId = activity.Id,
                Body = request.Body,

            };
            activity.Comments.Add(comment);
            var result = await appDbContext.SaveChangesAsync(cancellationToken) > 0;
            return result ? Result<CommentDTOs>.Success(comment.ToDto())
                       : Result<CommentDTOs>.Failure("Failed to add comment", StatusCodes.Status400BadRequest);
        }
    }

}
