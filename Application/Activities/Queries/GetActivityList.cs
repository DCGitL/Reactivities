using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityList
{
    public class Query : IRequest<Result<PageList<ActivityDto, DateTime?>>>
    {

        public required ActivityParams Params { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor) : IRequestHandler<Query, Result<PageList<ActivityDto, DateTime?>>>
    {

        public async Task<Result<PageList<ActivityDto, DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
        {
            // request.PageSize = request.PageSize == 0 ? 3 : request.PageSize;
            var currentUserID = userAccessor.GetUserId();
            var query = context.Activities
                       .OrderBy(x => x.Date)
                       .Where(x => x.Date >= (request.Params.Cursor ?? request.Params.StartDate))
                       .AsQueryable();

            if (!string.IsNullOrEmpty(request.Params.Filter))
            {
                query = request.Params.Filter switch
                {
                    "isGoing" => query.Where(x => x.Attendees.Any(x => x.UserId == currentUserID)),
                    "isHost" => query.Where(x => x.Attendees.Any(x => x.IsHost && x.UserId == currentUserID)),
                    _ => query
                };
            }
     
            var projectedActivities = query.Select(x => new ActivityDto
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
                    ImageUrl = a.User.ImageUrl,
                    FollowersCount = a.User.Followers.Count,
                    FollowingCount = a.User.Followings.Count,
                    Following = a.User.Followers.Any(f => f.Observer!.Id == currentUserID)

                }).ToList()
            });


            var activities = await projectedActivities
             .Take(request.Params.PageSize + 1)
              .ToListAsync(cancellationToken);

            DateTime? nextCursor = null;
            if (activities.Count > request.Params.PageSize)
            {
                nextCursor = activities.Last().Date;
                activities.RemoveAt(activities.Count - 1);
            }

            return Result<PageList<ActivityDto, DateTime?>>.Success(
                new PageList<ActivityDto, DateTime?>
                {
                    Items = activities,
                    NextCursor = nextCursor
                });

        }


    }

}
