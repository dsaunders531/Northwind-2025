using Microsoft.AspNetCore.Mvc;

namespace Northwind.Seo.Areas.Seo.Controllers
{
    [Area("Seo")]
    public class HomeController : Controller
    {
        [Route("/robots.txt")]
        [Produces("text/plain")]
        [HttpGet]
        public IActionResult Robots()
        {
            return View();
        }

        [Route("/sitemap.xml")]
        [Produces("text/xml")]
        [HttpGet]
        public IActionResult SiteMap()
        {
            return View();
        }
    }
}
