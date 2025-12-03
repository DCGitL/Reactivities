using System;
using Application.Core;
using Application.Profiles.DTOs;
using Application.Repository.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public class DbRepository : IQueryProfileRepository
{
    private readonly AppDbContext _context;

    public DbRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<UserProfile>>> GetFollowingTypeProfile1(string predicateFallowType, string userId, string currentUserId, CancellationToken cancellationToken)
    {

        var profile = new List<UserProfile>();
        switch (predicateFallowType)
        {
            case "followers":
                profile = await _context.UserFollowings.Where(x => x.TargetId == userId)
                .Select(x => new UserProfile
                {
                    Id = x.Observer!.Id,
                    DisplayName = x.Observer.DisplayName!,
                    Bio = x.Observer.Bio,
                    ImageUrl = x.Observer.ImageUrl,
                    Following = x.Observer.Followers.Any(f => f.ObserverId == currentUserId),
                    FollowersCount = x.Observer.Followers.Count,
                    FollowingCount = x.Observer.Followings.Count
                }).ToListAsync(cancellationToken);
                break;
            case "followings":
                profile = await _context.UserFollowings.Where(x => x.ObserverId == userId)
                .Select(x => new UserProfile
                {
                    Id = x.Target!.Id,
                    DisplayName = x.Target.DisplayName!,
                    Bio = x.Target.Bio,
                    ImageUrl = x.Target.ImageUrl,
                    FollowingCount = x.Target.Followings.Count,
                    FollowersCount = x.Target.Followers.Count,
                    Following = x.Target!.Followers.Any(f => f.Observer!.Id == currentUserId)

                }).ToListAsync(cancellationToken);
                break;
        }
        return Result<List<UserProfile>>.Success(profile);
    }

    public async Task<Result<List<UserProfile>>> GetFollowingTypeProfile(string predicateFallowType, string userId, string currentUserId, CancellationToken cancellationToken)
    {

        var result = predicateFallowType switch
        {
            "followers" => await _context.UserFollowings.Where(x => x.TargetId == userId)
                .Select(x => new UserProfile
                {
                    Id = x.Observer!.Id,
                    DisplayName = x.Observer.DisplayName!,
                    Bio = x.Observer.Bio,
                    ImageUrl = x.Observer.ImageUrl,
                    Following = x.Observer.Followers.Any(f => f.ObserverId == currentUserId),
                    FollowersCount = x.Observer.Followers.Count,
                    FollowingCount = x.Observer.Followings.Count
                }).ToListAsync(cancellationToken),
            "followings" => await _context.UserFollowings.Where(x => x.ObserverId == userId)
                .Select(x => new UserProfile
                {
                    Id = x.Target!.Id,
                    DisplayName = x.Target.DisplayName!,
                    Bio = x.Target.Bio,
                    ImageUrl = x.Target.ImageUrl,
                    FollowingCount = x.Target.Followings.Count,
                    FollowersCount = x.Target.Followers.Count,
                    Following = x.Target!.Followers.Any(f => f.Observer!.Id == currentUserId)

                }).ToListAsync(cancellationToken),
            _ => new List<UserProfile>()
        };


        return Result<List<UserProfile>>.Success(result);
    }

    public async Task<Result<UserProfile>> GetUserProfileById(string userId, string currentUserId, CancellationToken cancellationToken)
    {
        var profile = await _context.Users.Where(x => x.Id == userId)
                         .Select(x => new UserProfile
                         {
                             Id = x.Id,
                             Bio = x.Bio,
                             DisplayName = x.DisplayName!,
                             ImageUrl = x.ImageUrl,
                             FollowersCount = x.Followers.Count,
                             FollowingCount = x.Followings.Count,
                             Following = x.Followers.Any(f => f.Observer!.Id == currentUserId)
                         }).SingleOrDefaultAsync(cancellationToken);


        return (profile is null) ? Result<UserProfile>.Failure("User Profile was not found", StatusCodes.Status400BadRequest)
                                  : Result<UserProfile>.Success(profile);


    }
}
