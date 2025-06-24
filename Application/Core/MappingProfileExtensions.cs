using System;
using Domain;

namespace Application.Core;

public static class MappingProfileExtensions
{
    public static void ToExistingActivity(this Activity existingActivity, Activity Activity)
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
    public static void ToExistingActivityList(this List<Activity> sourceActivities, List<Activity> Activities)
    {
        if (sourceActivities == null) throw new ArgumentNullException(nameof(sourceActivities));
        if (Activities == null) throw new ArgumentNullException(nameof(Activities));

        foreach (var sourceActivity in sourceActivities)
        {
            var targetActivity = Activities.FirstOrDefault(a => a.Id == sourceActivity.Id);
            if (targetActivity != null)
            {
                sourceActivity.ToExistingActivity(targetActivity);
            }
            else
            {
                Activities.Add(sourceActivity);
            }
        }

    }


}
