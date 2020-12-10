using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Identity;

namespace RazorPages.Pages
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly UserManager<RazorPagesUser> userManager;
        private readonly SignInManager<RazorPagesUser> signInManager;

        public LogoutModel(UserManager<RazorPagesUser> userManager, SignInManager<RazorPagesUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var result = await userManager.GetUserAsync(User);
            if (result != null && (result.LockoutEnd < DateTime.Now || result.LockoutEnd == null))
            {
                result.Status = "Offline";
                await userManager.UpdateAsync(result);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                await signInManager.SignOutAsync();
            }
            return RedirectToPage("Login");
        }

    }
}
