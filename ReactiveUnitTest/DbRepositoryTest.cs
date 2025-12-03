using System;
using Application.Profiles.DTOs;
using NSubstitute.ExceptionExtensions;
using Persistence.Repository;

namespace ReactiveUnitTest;

public class DbRepositoryTest : IClassFixture<DbRepositoryUnitTest>
{
    private readonly DbRepository _dbRepository;


    public DbRepositoryTest(DbRepositoryUnitTest testFixture)
    {

        _dbRepository = new DbRepository(testFixture.GetDbContext);
    }

    [Fact]
    public async Task GetUserProfileById_UserExists_ReturnsSuccessResult()
    {
        // Arrange
        var userId = "user123";
        var currentUserId = "currentUser123";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _dbRepository.GetUserProfileById(userId, currentUserId, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.Id);
        Assert.Equal("Test Bio", result.Value.Bio);
        Assert.Equal("Test User", result.Value.DisplayName);
        Assert.Equal("test.jpg", result.Value.ImageUrl);
        Assert.True(result.Value.Following); // currentUser123 follows user123
    }

    [Fact]
    public async Task GetUserProfileById_UserNotFound_ReturnsFailureResult()
    {
        // Arrange
        var userId = "nonExistentUser";
        var currentUserId = "currentUser123";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _dbRepository.GetUserProfileById(userId, currentUserId, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User Profile was not found", result.Error);
    }

    [Fact]
    public async Task GetFollowingTypeProfile_Followers_ReturnsFollowersList()
    {
        // Arrange
        var predicateFollowType = "followers";
        var userId = "user123";
        var currentUserId = "currentUser123";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _dbRepository.GetFollowingTypeProfile(predicateFollowType, userId, currentUserId, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var followers = result.Value.Where(u => u.Id != currentUserId).ToList();
        Assert.Single(followers!);
        var profile = followers!.First();
        Assert.Equal("follower123", profile.Id);
        Assert.Equal("Follower User", profile.DisplayName);
        Assert.Equal("Follower Bio", profile.Bio);
        Assert.Equal("follower.jpg", profile.ImageUrl);
        Assert.False(profile.Following); // currentUser123 doesn't follow follower123
    }

    [Fact]
    public async Task GetFollowingTypeProfile_Followings_ReturnsFollowingsList()
    {
        // Arrange
        var predicateFollowType = "followings";
        var userId = "user123";
        var currentUserId = "currentUser123";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _dbRepository.GetFollowingTypeProfile(predicateFollowType, userId, currentUserId, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var followings = result.Value.Where(u => u.Id != currentUserId).ToList();
        Assert.Single(followings);
        var profile = followings.First();
        Assert.Equal("following123", profile.Id);
        Assert.Equal("Following User", profile.DisplayName);
        Assert.Equal("Following Bio", profile.Bio);
        Assert.Equal("following.jpg", profile.ImageUrl);
        Assert.False(profile.Following); // currentUser123 doesn't follow following123
    }

    [Fact]
    public async Task GetFollowingTypeProfile_InvalidFollowType_ReturnsEmptyList()
    {
        // Arrange
        var predicateFollowType = "invalidType";
        var userId = "user123";
        var currentUserId = "currentUser123";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _dbRepository.GetFollowingTypeProfile(predicateFollowType, userId, currentUserId, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task GetFollowingTypeProfile_Followers_WithNoFollowers_ReturnsEmptyList()
    {
        // Arrange
        var predicateFollowType = "followers";
        var userId = "following123"; // This user has no followers
        var currentUserId = "currentUser123";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _dbRepository.GetFollowingTypeProfile(predicateFollowType, userId, currentUserId, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var userprofile = Assert.IsType<List<UserProfile>>(result.Value);
        Assert.True(userprofile.Where(x => x.Following).Any());
    }

    [Fact]
    public async Task GetFollowingTypeProfile_Followings_WithNoFollowings_ReturnsEmptyList()
    {
        // Arrange
        var predicateFollowType = "followings";
        var userId = "follower123"; // This user has no followings
        var currentUserId = "currentUser123";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _dbRepository.GetFollowingTypeProfile(predicateFollowType, userId, currentUserId, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var userprofile = Assert.IsType<List<UserProfile>>(result.Value);
        Assert.True(userprofile.Where(x => x.Following).Any());
    }

}
