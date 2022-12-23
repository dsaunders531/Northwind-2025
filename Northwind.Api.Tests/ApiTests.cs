// <copyright file="ApiTests.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Patterns;
using Patterns.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace Northwind.Api.Tests
{
    /// <summary>
    /// Since all the api endpoints are represented by a service - implement the service or services and test each item.
    /// </summary>
    /// <remarks>There is only 1 service right now.
    /// And one set of tests already implemented. So we re-use those here by calling the api to get the data.
    /// </remarks>
    public class ApiTests : INorthwindProductsService
    {
        private HttpClient? _client;
        private NorthwindApiWebApplication? _app;

        [SetUp]
        public void Setup()
        {
            if (_app == default)
            {
                _app = new NorthwindApiWebApplication();

                _client = _app.CreateClient();
            }
        }

        [Test]
        public async Task FailedRequestTest()
        {
            // create a request which will not work
            HttpResponseMessage response = await _client.GetAsync($"/Categories?page=1&sort=321");

            Assert.That(response.IsSuccessStatusCode, Is.False);

            if (!response.IsSuccessStatusCode)
            {
                HttpErrorResponse err = await response.Content.ReadFromJsonAsync<HttpErrorResponse>();

                Assert.That(err.Title, Has.Length.GreaterThan(0));

                Assert.That(err.Errors, Has.Count.GreaterThan(0));
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task GetPagesTest()
        {
            // see if the quantity of pages matches the total items.
            IPagedResponse<ProductApi>? result = await GetProducts(1, SortBy.Name | SortBy.Ascending, string.Empty);

            Assert.That(result.TotalItems / result.ItemsPerPage, Is.LessThanOrEqualTo(result.TotalPages));

            int itemsOnLastPage = result.TotalItems % result.ItemsPerPage;

            result = await GetProducts(result.TotalPages, SortBy.Name | SortBy.Ascending, string.Empty);

            Assert.That(result.Page, Has.Length.EqualTo(itemsOnLastPage));
        }

        [Test]
        public async Task GetProductsTest()
        {
            IPagedResponse<ProductApi> result = await GetProducts(1, SortBy.Name | SortBy.Ascending, string.Empty);

            Assert.IsNotNull(result);

            Assert.That(result.Page, Has.Length.LessThanOrEqualTo(result.ItemsPerPage));

            Assert.That(result.Page.First().ProductName.First(), Is.LessThanOrEqualTo(result.Page.Last().ProductName.First()));

            Assert.That(result.TotalPages, Is.GreaterThan(1));

            result = await GetProducts(2, SortBy.Name | SortBy.Ascending, string.Empty);

            Assert.IsNotNull(result);

            Assert.That(result.CurrentPage, Is.EqualTo(2));
        }

        [Test]
        public async Task GetProductsWithSearchTest()
        {
            // there are 2 products from chef anton
            IPagedResponse<ProductApi> result = await GetProducts(1, SortBy.Name | SortBy.Ascending, "cHef aNton");

            Assert.IsNotNull(result);

            Assert.That(result.Page, Has.Length.LessThanOrEqualTo(result.ItemsPerPage));

            Assert.That(result.Page.First().ProductName.First(), Is.LessThanOrEqualTo(result.Page.Last().ProductName.First()));

            Assert.That(result.Page, Has.Length.EqualTo(2));

            foreach (ProductApi item in result.Page)
            {
                Assert.That(item.ProductName.ToLowerInvariant(), Does.Contain("chef anton"));
            }
        }

#pragma warning disable NUnit1028 // The non-test method is public
        public async Task<IPagedResponse<ProductApi>> GetProducts(int page, SortBy sort, string searchTerm)
#pragma warning restore NUnit1028 // The non-test method is public
        {
            HttpResponseMessage response = await _client.GetAsync($"/Products?page={page}&sort={(int)sort}&searchTerm={searchTerm}");

            if (!response.IsSuccessStatusCode)
            {
                HttpErrorResponse err = await response.Content.ReadFromJsonAsync<HttpErrorResponse>();

                throw new HttpRequestException($"Error: {err.Title}. {err.Errors.FirstOrDefault().Value.FirstOrDefault()}");
            }

            _ = response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PagedResponse<ProductApi>>();
        }

        [Test]
        public async Task GetProductByIdTest()
        {
            // get a product which exists.
            ProductApi? product = await GetProductById(23); // Tunnbröd

            Assert.That(product, Is.Not.Null);

            Assert.That(product?.ProductName, Is.EqualTo("Tunnbröd"));

            // get a product which does not exist.
            _ = Assert.ThrowsAsync<KeyNotFoundException>(() => GetProductById(321));
        }

#pragma warning disable NUnit1028 // The non-test method is public
        public async Task<ProductApi?> GetProductById(int productId)
#pragma warning restore NUnit1028 // The non-test method is public
        {
            HttpResponseMessage response = await _client.GetAsync($"/Products/{productId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Product {productId} was not found");
            }
            else
            {
                _ = response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ProductApi>();
            }
        }

        [Test]
        public async Task SearchProductsTest()
        {
            string[] result = await SearchProducts("chef anton");

            Assert.That(result, Has.Length.GreaterThanOrEqualTo(2));
        }

#pragma warning disable NUnit1028 // The non-test method is public
        public async Task<string[]> SearchProducts(string term)
#pragma warning restore NUnit1028 // The non-test method is public
        {
            HttpResponseMessage response = await _client.GetAsync($"/Products/search?term={term}");

            if (!response.IsSuccessStatusCode)
            {
                HttpErrorResponse? err = await response.Content.ReadFromJsonAsync<HttpErrorResponse>();

                throw new HttpRequestException($"Error: {err.Title}. {err.Errors.FirstOrDefault().Value.FirstOrDefault()}");
            }

            return await response.Content.ReadFromJsonAsync<string[]>() ?? Array.Empty<string>();
        }

        [Test]
        public async Task WorkingTest()
        {
            HttpResponseMessage response = await _client.GetAsync("/categories");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetCategoriesTest()
        {
            IPagedResponse<CategoryApi> result = await GetCategories(1, SortBy.Name | SortBy.Ascending);

            Assert.IsNotNull(result);

            Assert.That(result.Page, Has.Length.LessThanOrEqualTo(result.ItemsPerPage));

            Assert.That(result.Page.First().CategoryName.First(), Is.LessThanOrEqualTo(result.Page.Last().CategoryName.First()));
        }

#pragma warning disable NUnit1028 // The non-test method is public
        public async Task<IPagedResponse<CategoryApi>> GetCategories(int page, SortBy sort)
#pragma warning restore NUnit1028 // The non-test method is public
        {
            HttpResponseMessage response = await _client.GetAsync($"/Categories?page={page}&sort={(int)sort}");

            _ = response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            PagedResponse<CategoryApi> result = content.JsonConvert<PagedResponse<CategoryApi>>();

            return result;
        }

        [Test]
        public async Task GetProductsInCategoryTest()
        {
            IPagedResponse<ProductApi> products = await GetProductsInCategory(3, 1, SortBy.Ascending | SortBy.Name);

            Assert.IsNotNull(products);

            Assert.That(products.Page, Has.Length.LessThanOrEqualTo(products.ItemsPerPage));

            Assert.That(products.Page.First().ProductName.First(), Is.LessThanOrEqualTo(products.Page.Last().ProductName.First()));
        }

#pragma warning disable NUnit1028 // The non-test method is public
        public async Task<IPagedResponse<ProductApi>> GetProductsInCategory(int categoryId, int page, SortBy sort)
#pragma warning restore NUnit1028 // The non-test method is public
        {
            HttpResponseMessage response = await _client.GetAsync($"/Categories/{categoryId}/products");

            _ = response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PagedResponse<ProductApi>>();
        }
    }
}