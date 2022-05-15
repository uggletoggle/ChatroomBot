using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace External.IdentityServerUI
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources => new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "username",
                UserClaims = new List<string> { "username" }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            new ApiScope("Chatroom.API")
        };

        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource("Chatroom.API")
            {
                Scopes = new List<string> { "Chatroom.API" },
                ApiSecrets = new List<Secret> { new Secret("chatroomsecret".Sha256()) },
                UserClaims = new List<string> { "username" }
            }
        };

        public static IEnumerable<Client> Clients => new[]
        {
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "Chatroom.API" }
            },
            new Client
            {
                ClientId = "angular",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = { "openid", "profile", "Chatroom.API" },
                RedirectUris = { "https://localhost:4200/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:4200/signout-oidc" },
            }
        };
    }
}
