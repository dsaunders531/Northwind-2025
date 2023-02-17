using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Northwind.Identity.Web
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()            
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> { 
                new ApiScope(name: "northwind-api", displayName: "Northwind Data Api")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client> { 
                // api access using a key
                new Client()
                {
                    ClientId = "northwind-web",
                    
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "northwind-api" },
                    
                    // use refresh tokens
                    AllowOfflineAccess = true
                },
                // interactive ASP.NET Core Web Northwind.web.ui hosted from https://localhost:7240
                new Client
                {
                    ClientId = "northwind-web-user",
                    
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
            
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:7240/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:7240/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
    }
}
