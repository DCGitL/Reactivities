using static Infrastructure.SocialMedia.Models.GitHubInfo;

namespace Infrastructure.SocialMedia.Login;

public interface ILoginService
{
    Task<GitHubTokenResponse?> GitHubTokenResponse(string code);
    Task<GitHubUser?> GetGitHubUser(GitHubTokenResponse token);
}
