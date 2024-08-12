using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Northwind.Web.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return SignOut(new AuthenticationProperties() { RedirectUri = "https://localhost:7240/" }, "Cookies", "oidc");
            }
            else
            {
                return RedirectToPage("Index");
            }
        }
    }
}
