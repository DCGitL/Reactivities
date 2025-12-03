using Application.Core;
using Application.Profiles.DTOs;

namespace Persistence.Repository;

public interface IQueryProfileRepository
{
    public Task<Result<List<UserProfile>>> GetFollowingTypeProfile(string predicate, string userId, string currentUserId, CancellationToken cancellationToken);

    public Task<Result<UserProfile>> GetUserProfileById(string userId, string currentUserId, CancellationToken cancellationToken);
}
