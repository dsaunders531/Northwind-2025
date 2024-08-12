// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Northwind.Identity.Web.Models;
namespace Northwind.Identity.Web.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel(
        UserManager<ApplicationUser> userManager,
        ILogger<PersonalDataModel> logger) : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            logger.LogInformation($"{0} is getting thier personal data.", User?.Identity?.Name ?? "Unknown");

            if (User != null)
            {
                ApplicationUser? user = await userManager.GetUserAsync(User);

                return user == null ? NotFound() : Page();
            }

            return NotFound();
        }
    }
}
