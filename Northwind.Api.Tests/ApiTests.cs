using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;

namespace Northwind.Api.Tests
{
    public class ApiTests
    {
        private HttpClient _client;
        private NorthwindApiWebApplication _app;

        [SetUp]
        public void Setup()
        {
            _app = new NorthwindApiWebApplication();
            _client = _app.CreateClient();
        }

        [TestCase]
        public async Task CategoriesTest()
        {
            HttpResponseMessage response = await _client.GetAsync("/categories");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // TODO write more tests - check the output etc.
            
        }
    }
}