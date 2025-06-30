using System;
using Application.Activities.DTOs;
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


}
