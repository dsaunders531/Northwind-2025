// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Northwind.Identity.Web.Models;
using Northwind.Security.ActionFilters;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;

namespace Northwind.Identity.Web.Areas.Identity.Pages.Account
{
    [AllowXRequestsEveryNSecondsPage("ResendEmail", 6, 60)]
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        
        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            IActionResult result = Page();

            // Add timer so this always takes n seconds for all outcomes (OWASP recommendation)
            TimeSpan minDuration = TimeSpan.FromSeconds(6);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");

                }
                else
                {
                    string userId = await _userManager.GetUserIdAsync(user);

                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    string callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = userId, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
                }
            }

            // pause if we need to - this is to not give away the user exists or not
            stopwatch.Stop();
            if (stopwatch.Elapsed < minDuration)
            {
                Thread.Sleep(minDuration - stopwatch.Elapsed);
            }

            return result;
        }
    }
}
