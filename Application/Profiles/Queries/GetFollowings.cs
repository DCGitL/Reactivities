using System;
using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetFollowings
{
    public class Query : IRequest<Result<List<UserProfile>>>
    {
        public string Predicate { get; set; } = "followers";
        public required string UserId { get; set; }
    }

    public class Handler(AppDbContext _context, IUserAccessor userAccessor) : IRequestHandler<Query, Result<List<UserProfile>>>
    {

        public async Task<Result<List<UserProfile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUserID = userAccessor.GetUserId();

            var profile = new List<UserProfile>();
            switch (request.Predicate)
            {
                case "followers":
                    profile = await _context.UserFollowings.Where(x => x.TargetId == request.UserId)
                    .Select(x => new UserProfile
                    {
                        Id = x.Observer!.Id,
                        DisplayName = x.Observer.DisplayName!,
                        Bio = x.Observer.Bio,
                        ImageUrl = x.Observer.ImageUrl,
                        Following = x.Observer.Followers.Any(f => f.ObserverId == currentUserID),
                        FollowersCount = x.Observer.Followers.Count,
                        FollowingCount = x.Observer.Followings.Count
                    }).ToListAsync(cancellationToken);
                    break;
                case "followings":
                    profile = await _context.UserFollowings.Where(x => x.ObserverId == request.UserId)
                    .Select(x => new UserProfile
                    {
                        Id = x.Target!.Id,
                        DisplayName = x.Target.DisplayName!,
                        Bio = x.Target.Bio,
                        ImageUrl = x.Target.ImageUrl,
                        FollowingCount = x.Target.Followings.Count,
                        FollowersCount = x.Target.Followers.Count,
                        Following = x.Target!.Followers.Any(f => f.Observer!.Id == currentUserID)

                    }).ToListAsync(cancellationToken);
                    break;
            }
            return Result<List<UserProfile>>.Success(profile);

        }
    }

}
