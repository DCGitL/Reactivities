using System;
using Application.Core;
using Application.Profiles.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetProfile
{
    public class Query : IRequest<Result<UserProfile>>
    {
        public required string UserId { get; set; }
    }
    public class Handler(AppDbContext appDbContext) : IRequestHandler<Query, Result<UserProfile>>
    {
        public async Task<Result<UserProfile>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profile = await appDbContext.Users.Where(x => x.Id == request.UserId)
                           .Select(x => new UserProfile
                           {
                               Id = x.Id,
                               Bio = x.Bio,
                               DisplayName = x.DisplayName!,
                               ImageUrl = x.ImageUrl
                           }).SingleOrDefaultAsync(cancellationToken);


            return (profile is null) ? Result<UserProfile>.Failure("User Profile was not found", StatusCodes.Status400BadRequest)
                                      : Result<UserProfile>.Success(profile);

        }
    }


}
