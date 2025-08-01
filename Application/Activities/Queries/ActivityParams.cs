using System;
using Application.Core;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Application.Activities.Queries;

public class ActivityParams : PaginationParams<DateTime?>
{
    public string? Filter { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

}

public class UserActivityParams : ActivityParams
{
    public required string UserId { get; set; }
}