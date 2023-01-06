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

            if (string.IsNullOrWhiteSpace(this.BaseUrl.Host))
            {
                throw new UriFormatException("Url does not look correct. Check the configuration.");
            }
        }

        private HttpClient Client { get; set; }

        private Uri BaseUrl { get; set; }

        public async Task<global::Patterns.IPagedResponse<CategoryApi>> GetCategories(int page, global::Patterns.SortBy sort)
        {            
            HttpResponseMessage response = await this.Client.GetAsync(new Uri(this.BaseUrl, $"Categories?page={page}&sort={sort}"));

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<PagedResponse<CategoryApi>>() ?? new PagedResponse<CategoryApi>()) as IPagedResponse<CategoryApi>;
        }

        public async Task<ProductApi?> GetProductById(int productId)
        {            
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