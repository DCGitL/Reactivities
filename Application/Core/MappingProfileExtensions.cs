using System;
using System.Security.AccessControl;
using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using Domain;

namespace Application.Core;

public static class MappingProfileExtensions
{
    public static void ToExistingActivity(this Activity existingActivity, EditActivityDto Activity)
    {
        if (existingActivity == null) throw new ArgumentNullException(nameof(existingActivity));

        if (Activity == null) throw new ArgumentNullException(nameof(Activity));
        if (existingActivity.Id != Activity.Id) throw new ArgumentException("Activity ID mismatch");
        existingActivity.Title = Activity.Title;
        existingActivity.Description = Activity.Description;
        existingActivity.Category = Activity.Category;
        existingActivity.Date = Activity.Date;
        existingActivity.City = Activity.City;
        existingActivity.Venue = Activity.Venue;
        existingActivity.Longitude = Activity.Longitude;
        existingActivity.Latitude = Activity.Latitude;

    }


    public static CreateActivityDto ToDto(this Activity item)
    {
        return new CreateActivityDto
        {

            Category = item.Category,
            City = item.City,
            Date = item.Date,
            Description = item.Description,
            Title = item.Title,
            Venue = item.Venue,
            Latitude = item.Latitude,
            Longitude = item.Longitude,


        };
    }
    public static CommentDTOs ToDto(this Comment item)
    {
        return new CommentDTOs
        {
            Id = item.Id,
            Body = item.Body,
            CreatedAt = item.CreatedAt,
            UserId = item.UserId,
            DisplayName = item.User!.DisplayName!,
            ImageUrl = item.User.ImageUrl
        };
    }

    public static List<CreateActivityDto> ToDto(this List<Activity> items)
    {
        return items.Select(item => ToDto(item)).ToList();
    }

    public static Activity FromDto(this CreateActivityDto item)
    {
        return new Activity
        {

            Category = item.Category,
            City = item.City,
            Description = item.Description,
            Title = item.Title,
            Venue = item.Venue,
            Date = item.Date,
            Latitude = item.Latitude,
            IsCancelled = default!,
            Longitude = item.Longitude,
        };

    }

    public static UserProfile ToUserProfile(this ActivityAttendee activityAttendee)
    {
        return new UserProfile
        {
            Id = activityAttendee?.UserId!,
            DisplayName = activityAttendee?.User.DisplayName!,
            Bio = activityAttendee?.User.Bio,
            ImageUrl = activityAttendee?.User.ImageUrl

        };
    }
    public static ActivityDto ToDTOs(this Activity x)
    {
        return new ActivityDto
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
        };
    }

    public static ActivityDto ProjectToDto(Activity x)
    {
        var hostAttendee = x.Attendees.FirstOrDefault(a => a.IsHost);
        return new ActivityDto
        {
            Id = x.Id,
            Title = x.Title,
            Date = x.Date,
            Description = x.Description,
            Category = x.Category,
            IsCancelled = x.IsCancelled,
            HostDisplayName = hostAttendee?.User?.DisplayName ?? string.Empty,
            HostId = hostAttendee?.User?.Id ?? string.Empty,
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
        };
    }


}
