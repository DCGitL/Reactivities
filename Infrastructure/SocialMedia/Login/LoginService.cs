using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Infrastructure.SocialMedia.Models;
using Microsoft.Extensions.Configuration;
using static Infrastructure.SocialMedia.Models.GitHubInfo;

namespace Infrastructure.SocialMedia.Login;

public class LoginService : ILoginService
{
    private readonly IHttpClientFactory clientFactory;
    private readonly IConfiguration configuration;

    public LoginService(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        this.clientFactory = clientFactory;
        this.configuration = configuration;
    }

    public async Task<GitHubInfo.GitHubTokenResponse?> GitHubTokenResponse(string code)
    {

        var endpoint = configuration.GetSection("Authentication:Github:TokenEndpoint").Value;
        if (string.IsNullOrEmpty(endpoint))
        {
            return null;
        }
        var client = clientFactory.CreateClient("GitHubClient");

        var tokenresponse = await client.PostAsJsonAsync(endpoint, new GitHubAuthRequest
        {
            Code = code,
            ClientId = configuration.GetSection("Authentication:Github:ClientId").Value!,
            ClientSecret = configuration.GetSection("Authentication:Github:ClientSecret").Value!,
            RedirectUri = $"{configuration.GetSection("ClientApUrl").Value!}/auth-callback"
        });
        if (tokenresponse.IsSuccessStatusCode)
        {
            var result = await tokenresponse.Content.ReadFromJsonAsync<GitHubTokenResponse>();
            if (string.IsNullOrEmpty(result?.AccessToken))
            { return null; }

            return result;
        }

        return null;

    }


    public async Task<GitHubUser?> GetGitHubUser(GitHubTokenResponse token)
    {
        var apiBasurl = configuration.GetSection("Authentication:Github:ApiBaseUrl").Value!;
        var client = clientFactory.CreateClient("GitHubClient");
        client.BaseAddress = new Uri(apiBasurl); //endpoint => /user
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Reactivities");

        var userResponse = await client.GetAsync("/user");
        if (userResponse.IsSuccessStatusCode)
        {
            var result = await userResponse.Content.ReadFromJsonAsync<GitHubUser>();
            if (result is not null && string.IsNullOrEmpty(result.Email))
            {
                var emailResponse = await client.GetAsync("/user/emails");
                if (emailResponse.IsSuccessStatusCode)
                {
                    var emails = await emailResponse.Content.ReadFromJsonAsync<List<GitHubEmail>>();
                    var primary = emails?.FirstOrDefault(x => x is { Primary: true, verified: true })?.Email;
                    if (string.IsNullOrEmpty(primary))
                    {
                        return null;
                    }

                    result.Email = primary;

                }
            }
            return result;
        }
        return null;
    }



}
