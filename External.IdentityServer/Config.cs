using IdentityServer4.Models;
using System.Collections.Generic;

namespace External.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources => new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string> { "role" }
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
                UserClaims = new List<string> { "role" }
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
                ClientId = "interactive",
                ClientSecrets = { new Secret("moresecret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = { "openid", "profile", "Chatroom.API" },
                RequirePkce = true,
                RedirectUris = { "https://localhost:5444/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:5444/signin-oidc",
                PostLogoutRedirectUris = { "https://localhost:5444/signout-callback-oidc" },
                AllowOfflineAccess = true,
                RequireConsent = true,
                AllowPlainTextPkce = false
            }
        };
    }
}
