using IdentityMail.Web.DTOs.UserDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.ViewComponents.Mail
{
    public class _MailTopNavbarViewComponent(
    UserManager<AppUser> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userName =
                UserClaimsPrincipal.Identity?.Name;

            if (string.IsNullOrWhiteSpace(userName))
            {
                return View(new MailTopNavbarDto
                {
                    FullName = "Kullanıcı",
                    ProfileImageUrl =
                        "/images/default-user.png"
                });
            }

            var user = await _userManager.FindByNameAsync(
                userName
            );

            if (user == null)
            {
                return View(new MailTopNavbarDto
                {
                    FullName = "Kullanıcı",
                    ProfileImageUrl =
                        "/images/default-user.png"
                });
            }

            var model = new MailTopNavbarDto
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,

                ProfileImageUrl =
        string.IsNullOrWhiteSpace(user.ProfileImageUrl)
            ? "/images/default-user.png"
            : user.ProfileImageUrl
            };

            return View(model);
        }
    }
}
