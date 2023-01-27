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
            this.Client = httpClient;
            this.BaseUrl = new Uri(configuration["ApiBaseUrl"].ToString());
            this.AuthUrl = new Uri(configuration["AuthUrl"].ToString());

            if (string.IsNullOrWhiteSpace(this.BaseUrl.Host) || string.IsNullOrWhiteSpace(this.AuthUrl.Host))
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
            if (this.OpenIdConfiguration == default)
            {
                this.OpenIdConfiguration = await this.Client.GetDiscoveryDocumentAsync(this.AuthUrl.ToString());
            }
            
            return this.OpenIdConfiguration;            
        }

        private async Task<string> GetToken()
        {            
            if (this.Token == default || (TokenExpires ?? DateTime.MinValue) <= DateTime.UtcNow)
            {
                TokenResponse tokenResponse = await this.Client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = (await this.GetDiscoveryDoc()).TokenEndpoint,

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
                    this.Token = tokenResponse;

                    TokenExpires = DateTime.UtcNow.AddMinutes(tokenResponse.ExpiresIn);
                }
            }

            return this.Token.AccessToken;                        
        }

        public async Task<global::Patterns.IPagedResponse<CategoryApi>> GetCategories(int page, global::Patterns.SortBy sort)
        {
            HttpResponseMessage response = await this.Client.GetAsync(new Uri(this.BaseUrl, $"Categories?page={page}&sort={sort}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<PagedResponse<CategoryApi>>() ?? new PagedResponse<CategoryApi>()) as IPagedResponse<CategoryApi>;
        }

        public async Task<CategoryApi> GetCategory(int categoryId)
        {
            this.Client.SetBearerToken(await this.GetToken());

            HttpResponseMessage response = await this.Client.GetAsync(new Uri(this.BaseUrl, $"Categories/{categoryId}"));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryApi>() ?? new CategoryApi();
        }

        public async Task<ProductApi?> GetProductById(int productId)
        {
            this.Client.SetBearerToken(await this.GetToken());

            HttpResponseMessage response = await this.Client.GetAsync(new Uri(this.BaseUrl, $"Products/{productId}"));

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
            HttpResponseMessage response = await this.Client.GetAsync(new Uri(this.BaseUrl, $"Products?page={page}&sort={sort}&searchTerm={searchTerm}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<PagedResponse<ProductApi>>() ?? new PagedResponse<ProductApi>()) as IPagedResponse<ProductApi>;
        }

        public async Task<global::Patterns.IPagedResponse<ProductApi>> GetProductsInCategory(int categoryId, int page, global::Patterns.SortBy sort)
        {
            this.Client.SetBearerToken(await this.GetToken());

            HttpResponseMessage response = await this.Client.GetAsync(new Uri(this.BaseUrl, $"Categories/{categoryId}/products?page={page}&sort={sort}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<PagedResponse<ProductApi>>() ?? new PagedResponse<ProductApi>()) as IPagedResponse<ProductApi>;
        }

        public async Task<string[]> SearchProducts(string term)
        {           
            HttpResponseMessage response = await this.Client.GetAsync(new Uri(this.BaseUrl, $"Products/search?term={term}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<string[]>() ?? Array.Empty<string>());
        }
    }
}