using Microsoft.AspNetCore.Mvc;

namespace Northwind.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController(ILogger<TestController> logger)
        {
            Logger = logger;
        }

        private ILogger Logger { get; set; }

        [HttpGet]
        [Produces("application/json")]
        public ActionResult<IEnumerable<TestThing>> Get()
        {
            try
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                else
                {
                    return new JsonResult(new List<TestThing>()
                {
                    new TestThing() { Id = 1, Name = "Hat" },
                    new TestThing() { Id = 2, Name = "Shoe"}
                });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error getting test info.");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }

    public class TestThing
    {
        public TestThing()
        {
            Name = string.Empty;
        }

        public int Id { get; set; } = 0;
        public string Name { get; set; }
    }
}
