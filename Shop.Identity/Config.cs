using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace Shop.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("Shop.Api.Products", "Products API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "Shop.Jobs.PaymentConciliation",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "Shop.Api.Products" }
                },
                new Client
                {
                    ClientId = "Shop.Ui.WebSite",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    RedirectUris = { "https://localhost:3001/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:3001/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "Shop.Api.Products"
                    },
                    AllowOfflineAccess = true // Habilita refresh tokens
                },
                new Client
                {
                    ClientId = "Shop.Ui.Backoffice",
                    ClientName = "Backoffice App",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris =           { "http://localhost:3002/callback" },
                    PostLogoutRedirectUris = { "http://localhost:3002/" },
                    AllowedCorsOrigins =     { "http://localhost:3002" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Shop.Api.Products"
                    },
                    AllowAccessTokensViaBrowser = true // Required for SPA
                },
                new Client
                {
                    ClientId = "Shop.Mobile",
                    ClientName = "Mobile App",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris =           { "http://localhost:3000/" },
                    PostLogoutRedirectUris = { "http://localhost:3000" },
                    AllowedCorsOrigins =     { "http://localhost:3000" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Shop.Api.Products"
                    },
                },
            };

    }
}