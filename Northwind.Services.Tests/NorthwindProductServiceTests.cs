// <copyright file="NorthwindProductServiceTests.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Northwind.Context.Contexts;
using Northwind.Context.InMemory.Contexts;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Northwind.Context.Services;
using Patterns;

namespace Northwind.Services.Tests
{
    public class NorthwindProductServiceTests : INorthwindProductsService
    {
        private NorthwindContext? _context;
        
        private INorthwindProductsService? _service;

        [Test]
        public async Task GetCategoriesTest()
        {
            IPagedResponse<CategoryApi> result = await GetCategories(1, SortBy.Name | SortBy.Ascending);

            Assert.IsNotNull(result);
                        
            Assert.That(result.Page, Has.Length.LessThanOrEqualTo(result.ItemsPerPage));

            Assert.That(result.Page.First().CategoryName.First(), Is.LessThanOrEqualTo(result.Page.Last().CategoryName.First()));
        }

        public virtual Task<IPagedResponse<CategoryApi>> GetCategories(int page, SortBy sort)
        {
            return _service.GetCategories(page, sort);
        }

        [Test]
        public async Task GetProductByIdTest()
        {
            // get a product which exists.
            ProductApi? product = await GetProductById(23); // Tunnbröd

            Assert.That(product, Is.Not.Null);

            Assert.That(product?.ProductName, Is.EqualTo("Tunnbröd"));

            // get a product which does not exist.
            product = await GetProductById(123);

            Assert.That(product, Is.Null);
        }

        public virtual Task<ProductApi?> GetProductById(int productId)
        {
            return _service.GetProductById(productId);
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
                Assert.IsTrue(item.ProductName.ToLowerInvariant().Contains("chef anton"));
            }
        }

        public virtual Task<IPagedResponse<ProductApi>> GetProducts(int page, SortBy sort, string searchTerm)
        {
            return _service.GetProducts(page, sort, searchTerm);
        }

        [Test]
        public async Task GetProductsInCategoryTest()
        {
            IPagedResponse<ProductApi> products = await GetProductsInCategory(3, 1, SortBy.Ascending | SortBy.Name);

            Assert.IsNotNull(products);

            Assert.That(products.Page, Has.Length.LessThanOrEqualTo(products.ItemsPerPage));

            Assert.That(products.Page.First().ProductName.First(), Is.LessThanOrEqualTo(products.Page.Last().ProductName.First()));
        }

        public virtual Task<IPagedResponse<ProductApi>> GetProductsInCategory(int categoryId, int page, SortBy sort)
        {
            return _service.GetProductsInCategory(categoryId, page, sort);
        }

        [Test]
        public async Task SearchProductsTest()
        {
            string[] result = await SearchProducts("chef anton");

            Assert.That(result, Has.Length.GreaterThanOrEqualTo(2));
        }

        public virtual Task<string[]> SearchProducts(string term)
        {
            return _service.SearchProducts(term);
        }

        // It is handy to reuse tests so I use virtal here.
        [SetUp]
        public virtual void Setup()
        {
            if (_context == default)
            {
                _context = new NorthwindContextInMemory(string.Empty);
            }

            if (_service == default)
            {
                _service = new NorthwindProductsService(_context);
            }            
        }
        
    }
}