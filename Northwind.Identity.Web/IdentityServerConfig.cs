using Duende.IdentityServer.Models;

namespace Northwind.Identity.Web
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId()
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> { 
                new ApiScope(name: "northwind-api", displayName: "Northwind Data Api")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client> { 
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
                }
            };
    }
}
