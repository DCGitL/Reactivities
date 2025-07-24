using System;
using Application.Activities.DTOs;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetComments
{
    public class Query : IRequest<Result<List<CommentDTOs>>>
    {
        public required string ActivityId { get; set; }
    }

    public class Handler(AppDbContext _context) : IRequestHandler<Query, Result<List<CommentDTOs>>>
    {


        public async Task<Result<List<CommentDTOs>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await _context.Comments
                .Where(c => c.ActivityId == request.ActivityId)
                .OrderByDescending(c => c.CreatedAt)

                .Select(item => new CommentDTOs
                {
                    Id = item.Id,
                    Body = item.Body,
                    CreatedAt = item.CreatedAt,
                    UserId = item.UserId,
                    DisplayName = item.User!.DisplayName!,
                    ImageUrl = item.User.ImageUrl
                }).ToListAsync(cancellationToken);
            if (comments == null || !comments.Any())
            {
                return Result<List<CommentDTOs>>.Failure("No comments found for this activity.", StatusCodes.Status404NotFound);
            }

            return Result<List<CommentDTOs>>.Success(comments);
        }
    }


}
