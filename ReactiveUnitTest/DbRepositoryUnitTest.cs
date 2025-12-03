using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace ReactiveUnitTest;

public class DbRepositoryUnitTest
{
    private readonly AppDbContext _context;

    public DbRepositoryUnitTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        SeedTestData();

    }

    public AppDbContext GetDbContext => _context;

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void SeedTestData()
    {
        var users = new List<User>
        {
            new User
            {
                Id = "user123",
                DisplayName = "Test User",
                Bio = "Test Bio",
                ImageUrl = "test.jpg"
            },
            new User
            {
                Id = "follower123",
                DisplayName = "Follower User",
                Bio = "Follower Bio",
                ImageUrl = "follower.jpg"
            },
            new User
            {
                Id = "following123",
                DisplayName = "Following User",
                Bio = "Following Bio",
                ImageUrl = "following.jpg"
            },
            new User
            {
                Id = "currentUser123",
                DisplayName = "Current User",
                Bio = "Current Bio",
                ImageUrl = "current.jpg",


            }
        };

        var followings = new List<UserFollowing>
        {
            new UserFollowing { ObserverId = "follower123", TargetId = "user123" },
            new UserFollowing { ObserverId = "user123", TargetId = "following123" },
            new UserFollowing { ObserverId = "currentUser123", TargetId = "user123" }
        };

        _context.Users.AddRange(users);
        _context.UserFollowings.AddRange(followings);
        _context.SaveChanges();
    }
}
