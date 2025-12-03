using System;
using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repository;

namespace Application.Profiles.Queries;

public class GetFollowings
{
    public class Query : IRequest<Result<List<UserProfile>>>
    {
        public string Predicate { get; set; } = FollowType.Followers.ToString().ToLower();
        public required string UserId { get; set; }
    }

    public class Handler(IQueryProfileRepository _queryprofile, IUserAccessor userAccessor) : IRequestHandler<Query, Result<List<UserProfile>>>
    {

        public async Task<Result<List<UserProfile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUserID = userAccessor.GetUserId();

            var profiles = await _queryprofile.GetFollowingTypeProfile(request.Predicate, request.UserId, currentUserID, cancellationToken);
            return profiles;
        }
    }

}
