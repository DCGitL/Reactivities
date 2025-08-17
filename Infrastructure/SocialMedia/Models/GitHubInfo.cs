using System;
using System.Text.Json.Serialization;

namespace Infrastructure.SocialMedia.Models;

public class GitHubInfo
{

    public class GitHubAuthRequest
    {
        [JsonPropertyName("code")]
        public required string Code { get; set; }

        [JsonPropertyName("client_id")]
        public required string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public required string ClientSecret { get; set; }
        [JsonPropertyName("redirect_uri")]
        public required string RedirectUri { get; set; }
    }


    public class GitHubTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
    }

    public class GitHubUser
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("avatar_url")]
        public string? ImageUrl { get; set; }
    }

    public class GitHubEmail
    {
        public string Email { get; set; } = string.Empty;
        public bool Primary { get; set; }

        public bool verified { get; set; }
    }



}
