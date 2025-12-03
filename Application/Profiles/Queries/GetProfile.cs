using System;
using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repository;

namespace Application.Profiles.Queries;

public class GetProfile
{
    public class Query : IRequest<Result<UserProfile>>
    {
        public required string UserId { get; set; }
    }
    public class Handler(IQueryProfileRepository _queryProfile, IUserAccessor userAccessor) : IRequestHandler<Query, Result<UserProfile>>
    {
        public async Task<Result<UserProfile>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUserID = userAccessor.GetUserId();
            var profile = await _queryProfile.GetUserProfileById(request.UserId, currentUserID, cancellationToken);
            return profile;

        }
    }


}
