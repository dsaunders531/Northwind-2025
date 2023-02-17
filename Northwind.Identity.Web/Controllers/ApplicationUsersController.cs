using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Identity.Web.Models;

namespace Northwind.Identity.Web.Controllers
{
    [Authorize(Roles = "UserAdministrator")]
    public class ApplicationUsersController : Controller
    {        
        private readonly UserManager<ApplicationUser> UserManager;

        public ApplicationUsersController(UserManager<ApplicationUser> userManager)
        {            
            UserManager = userManager;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {            
            return View(await UserManager.Users.ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || UserManager.Users == null)
            {
                return NotFound();
            }

            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id.ToString());
            
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || UserManager.Users == null)
            {
                return NotFound();
            }

            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id.ToString());
            
            if (applicationUser == null)
            {
                return NotFound();
            }
            
            return View(applicationUser);
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
                ApplicationUser user = await UserManager.FindByIdAsync(id.ToString());
                
                user.PhoneNumber = applicationUser.PhoneNumber;
                user.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
                user.EmailConfirmed = applicationUser.EmailConfirmed;
                user.LockoutEnd = null;
                user.LockoutEnabled = applicationUser.LockoutEnabled;
                user.AccessFailedCount = 0;

                await UserManager.UpdateAsync(user);
                await UserManager.SetLockoutEndDateAsync(user, null);
                await UserManager.SetLockoutEnabledAsync(user, applicationUser.LockoutEnabled);
                
                await UserManager.UpdateSecurityStampAsync(user);

                return RedirectToAction(nameof(Index));
            }
            
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || UserManager.Users == null)
            {
                return NotFound();
            }

            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id.ToString());

            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id.ToString());

            if (applicationUser != null)
            {
                await UserManager.UpdateSecurityStampAsync(applicationUser);

                await UserManager.RemoveClaimsAsync(applicationUser, await UserManager.GetClaimsAsync(applicationUser));
                await UserManager.RemoveFromRolesAsync(applicationUser, await UserManager.GetRolesAsync(applicationUser));
                await UserManager.DeleteAsync(applicationUser);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(Guid id)
        {
            return UserManager.Users.Any(a => a.Id == id);
        }
    }
}
