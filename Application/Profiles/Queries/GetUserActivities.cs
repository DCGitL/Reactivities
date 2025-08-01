using System;
using System.Security.Cryptography.X509Certificates;
using Application.Activities.Queries;
using Application.Core;
using Application.Profiles.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetUserActivities
{
    public class Query : IRequest<Result<List<UserActivityDto>>>
    {
        public required string UserId { get; set; }
        /// <summary>
        /// filter is past and future events
        /// filter=past
        /// filter=future
        /// </summary>
        public required string Filter { get; set; }
    }

    public class Handler(AppDbContext appDbContext) : IRequestHandler<Query, Result<List<UserActivityDto>>>
    {
        public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = appDbContext.ActivityAttendees
            .Where(x => x.UserId == request.UserId)
             .OrderBy(x => x.Activity.Date)
              .Select(x => x.Activity)
              .AsQueryable();

            var today = DateTime.UtcNow;

            query = request.Filter switch
            {
                "past" => query.Where(x => x.Date <= today && x.Attendees.Any(x => x.UserId == request.UserId)),
                "hosting" => query.Where(x => x.Attendees.Any(x => x.IsHost && x.UserId == request.UserId)),
                _ => query.Where(x => x.Date > today && x.Attendees.Any(x => x.UserId == request.UserId))

            };

            var projectedQuery = query.Select(x => new UserActivityDto
            {
                Id = x.Id,
                Title = x.Title,
                Category = x.Category,
                Date = x.Date
            });

            var result = await projectedQuery.ToListAsync(cancellationToken);
            return Result<List<UserActivityDto>>.Success(result);

            //past => any date lest than today
            //future =>  any date greater than today
            //isHost ==
            // isGoing == 

        }
    }


}
