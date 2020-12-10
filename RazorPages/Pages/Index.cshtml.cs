using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RazorPages.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorPages.Models;
using Microsoft.AspNetCore.Authentication;

namespace RazorPages.Pages
{
    
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly SignInManager<RazorPagesUser> signInManager;
        private readonly UserManager<RazorPagesUser> userManager;

        [BindProperty]
        public String Email { get; set; }
        [BindProperty]
        public String Password { get; set; }
        [BindProperty]
        public String Name { get; set; }
        [BindProperty]
        public String FormAction { get; set; }

        public IndexModel(ILogger<IndexModel> logger,SignInManager<RazorPagesUser> signInManager,UserManager<RazorPagesUser> userManager)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            if (signInManager.IsSignedIn(User))
            {
                var currentUser = await userManager.GetUserAsync(User);
                if (currentUser == null || await userManager.IsLockedOutAsync(currentUser))
                {
                    return await SignOut();
                }
                return Page();
            }
            return await SignOut();
        }

        private async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await signInManager.SignOutAsync();
            return RedirectToPage("Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return await SignOut();
            }
            var lockedOut = await userManager.IsLockedOutAsync(currentUser);
            if (lockedOut)
            {
                return await SignOut();
            } else
            {
                var ids = Request.Form["Selected"];
                foreach (String id in ids)
                {
                    var user = await userManager.FindByIdAsync(id);
                    switch (FormAction)
                    {
                        case "Block":
                            await userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);
                            user.Status = "Banned";
                            await userManager.UpdateAsync(user);
                            break;
                        case "Unblock":
                            if(await userManager.GetLockoutEndDateAsync(user) > DateTime.Now)
                            {
                                await userManager.SetLockoutEndDateAsync(user, DateTime.Now);
                                user.Status = "Offline";
                                await userManager.UpdateAsync(user);
                            }
                            break;
                        case "Delete":
                            await userManager.DeleteAsync(user);
                            break;
                    }
                }
                return RedirectToPage();
            }
        }
        public String GetStatusClass(String status)
        {
            return status switch
            {
                "Offline" => "text-info",
                "Online" => "text-success",
                "Banned" => "text-danger",
                _ => "text-info",
            };
        }
    }
}
