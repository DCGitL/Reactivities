using Application.Activities.DTOs;
using Application.Core;
using Application.Profiles.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityList
{

    public class Query : IRequest<List<ActivityDto>> { }

    public class Handler(AppDbContext context) : IRequestHandler<Query, List<ActivityDto>>
    {

        public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Activities
            .Select(x => new ActivityDto
            {
                Id = x.Id,
                Title = x.Title,
                Date = x.Date,
                Description = x.Description,
                Category = x.Category,
                IsCancelled = x.IsCancelled,
                HostDisplayName = x.Attendees.FirstOrDefault(a => a.IsHost) != null ? x.Attendees.FirstOrDefault(a => a.IsHost)!.User.DisplayName! : string.Empty,
                HostId = x.Attendees.FirstOrDefault(a => a.IsHost) != null ? x.Attendees.FirstOrDefault(a => a.IsHost)!.User.Id : string.Empty,
                City = x.City,
                Venue = x.Venue,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Attendees = x.Attendees.Select(a => new UserProfile
                {
                    Id = a.UserId!,
                    DisplayName = a.User.DisplayName!,
                    Bio = a.User.Bio,
                    ImageUrl = a.User.ImageUrl
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        }
    }

}
