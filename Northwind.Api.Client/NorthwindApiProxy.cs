using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Patterns;
using System.Net.Http.Json;

namespace Northwind.Api.Client
{
    /// <summary>
    /// An client which acts as a proxy to the real api.
    /// </summary>
    /// <remarks>The configuration needs a setting at top-level "ApiBaseUrl".</remarks>
    public class NorthwindApiProxy : INorthwindProductsService
    {
        public NorthwindApiProxy(HttpClient httpClient, IConfiguration configuration)
        {
            Client = httpClient;
            BaseUrl = new Uri(configuration["ApiBaseUrl"].ToString());
            AuthUrl = new Uri(configuration["AuthUrl"].ToString());

            if (string.IsNullOrWhiteSpace(BaseUrl.Host) || string.IsNullOrWhiteSpace(AuthUrl.Host))
            {
                throw new UriFormatException("Url does not look correct for base or auth. Check the configuration.");
            }                                   
        }

        private HttpClient Client { get; set; }

        private Uri BaseUrl { get; set; }

        private Uri AuthUrl { get; set; }

        private DiscoveryDocumentResponse? OpenIdConfiguration { get; set; } = default;

        private TokenResponse? Token { get; set; } = default;

        private DateTime? TokenExpires { get; set; } = default;
        
        private async Task<DiscoveryDocumentResponse> GetDiscoveryDoc()
        {                       
            if (OpenIdConfiguration == default)
            {
                OpenIdConfiguration = await Client.GetDiscoveryDocumentAsync(AuthUrl.ToString());
            }
            
            return OpenIdConfiguration;            
        }

        private async Task<string> GetToken()
        {            
            if (Token == default || (TokenExpires ?? DateTime.MinValue) <= DateTime.UtcNow)
            {
                TokenResponse tokenResponse = await Client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = (await GetDiscoveryDoc()).TokenEndpoint,

                    // TODO add to config
                    ClientId = "northwind-web",
                    ClientSecret = "secret",
                    Scope = "northwind-api"
                });

                if (tokenResponse.IsError)
                {
                    throw new ApplicationException("Cannot get a token!", new Exception(tokenResponse.Error));
                }
                else
                {
                    Token = tokenResponse;

                    TokenExpires = DateTime.UtcNow.AddMinutes(tokenResponse.ExpiresIn);
                }
            }

            return Token.AccessToken;                        
        }

        public async Task<global::Patterns.IPagedResponse<CategoryApi>> GetCategories(int page, global::Patterns.SortBy sort)
        {
            HttpResponseMessage response = await Client.GetAsync(new Uri(BaseUrl, $"Categories?page={page}&sort={sort}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<PagedResponse<CategoryApi>>() ?? new PagedResponse<CategoryApi>()) as IPagedResponse<CategoryApi>;
        }

        public async Task<CategoryApi> GetCategory(int categoryId)
        {
            Client.SetBearerToken(await GetToken());

            HttpResponseMessage response = await Client.GetAsync(new Uri(BaseUrl, $"Categories/{categoryId}"));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryApi>() ?? new CategoryApi();
        }

        public async Task<ProductApi?> GetProductById(int productId)
        {
            Client.SetBearerToken(await GetToken());

            HttpResponseMessage response = await Client.GetAsync(new Uri(BaseUrl, $"Products/{productId}"));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
            else
            {
                return (await response.Content.ReadFromJsonAsync<ProductApi>());
            }
        }

        public async Task<global::Patterns.IPagedResponse<ProductApi>> GetProducts(int page, global::Patterns.SortBy sort, string searchTerm)
        {
            HttpResponseMessage response = await Client.GetAsync(new Uri(BaseUrl, $"Products?page={page}&sort={sort}&searchTerm={searchTerm}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<PagedResponse<ProductApi>>() ?? new PagedResponse<ProductApi>()) as IPagedResponse<ProductApi>;
        }

        public async Task<global::Patterns.IPagedResponse<ProductApi>> GetProductsInCategory(int categoryId, int page, global::Patterns.SortBy sort)
        {
            Client.SetBearerToken(await GetToken());

            HttpResponseMessage response = await Client.GetAsync(new Uri(BaseUrl, $"Categories/{categoryId}/products?page={page}&sort={sort}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<PagedResponse<ProductApi>>() ?? new PagedResponse<ProductApi>()) as IPagedResponse<ProductApi>;
        }

        public async Task<string[]> SearchProducts(string term)
        {           
            HttpResponseMessage response = await Client.GetAsync(new Uri(BaseUrl, $"Products/search?term={term}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<string[]>() ?? Array.Empty<string>());
        }
    }
}