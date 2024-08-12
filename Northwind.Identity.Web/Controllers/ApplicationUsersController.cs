using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Identity.Web.Models;

namespace Northwind.Identity.Web.Controllers
{
    [Authorize(Roles = "UserAdministrator")]
    public class ApplicationUsersController(UserManager<ApplicationUser> userManager) : Controller
    {

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {            
            return View(await userManager.Users.ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || userManager.Users == null)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser? applicationUser = await userManager.FindByIdAsync((id ?? Guid.Empty).ToString());

                return applicationUser == null ? NotFound() : View(applicationUser);
            }
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || userManager.Users == null)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser? applicationUser = await userManager.FindByIdAsync((id ?? Guid.Empty).ToString());

                return applicationUser == null ? NotFound() : View(applicationUser);
            }
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {                
                ApplicationUser? user = await userManager.FindByIdAsync(id.ToString());

                if (user != null)
                {
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
                    user.EmailConfirmed = applicationUser.EmailConfirmed;
                    user.LockoutEnd = null;
                    user.LockoutEnabled = applicationUser.LockoutEnabled;
                    user.AccessFailedCount = 0;

                    await userManager.UpdateAsync(user);
                    await userManager.SetLockoutEndDateAsync(user, null);
                    await userManager.SetLockoutEnabledAsync(user, applicationUser.LockoutEnabled);

                    await userManager.UpdateSecurityStampAsync(user);

                    return RedirectToAction(nameof(Index));
                }                
            }
            
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || userManager.Users == null)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser? applicationUser = await userManager.FindByIdAsync((id ?? Guid.Empty).ToString());

                return applicationUser == null ? NotFound() : View(applicationUser);
            }
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            ApplicationUser? applicationUser = await userManager.FindByIdAsync(id.ToString());

            if (applicationUser != null)
            {
                await userManager.UpdateSecurityStampAsync(applicationUser);

                await userManager.RemoveClaimsAsync(applicationUser, await userManager.GetClaimsAsync(applicationUser));
                await userManager.RemoveFromRolesAsync(applicationUser, await userManager.GetRolesAsync(applicationUser));
                await userManager.DeleteAsync(applicationUser);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(Guid id)
        {
            return userManager.Users.Any(a => a.Id == id);
        }
    }
}
