using IdentityMail.Web.DTOs.AdminLayoutDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.ViewComponents.Admin
{
    public class _AdminTopNavbarViewComponent: ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _AdminTopNavbarViewComponent(
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager
                .GetUserAsync(HttpContext.User);

            var model = new AdminNavbarDto
            {
                FullName = user == null
                    ? "Admin"
                    : $"{user.FirstName} {user.LastName}",

                ProfileImageUrl = user?.ProfileImageUrl
            };

            return View(model);
        }
    }
}
