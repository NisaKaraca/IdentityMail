using IdentityMail.Web.Context;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.ViewComponents.Mail
{
    public class _MailSidebarViewComponent(
       UserManager<AppUser> _userManager,
       AppDbContext _context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userName =
                UserClaimsPrincipal.Identity?.Name;

            if (string.IsNullOrWhiteSpace(userName))
            {
                return View(0);
            }

            var user = await _userManager.FindByNameAsync(
                userName
            );

            if (user == null)
            {
                return View(0);
            }

            var unreadMessageCount =
                await _context.UserMessages.CountAsync(x =>
                    x.ReceiverId == user.Id &&
                    !x.IsRead &&
                    !x.IsTrashedByReceiver &&
                    !x.IsPermanentlyDeletedByReceiver);

            return View(unreadMessageCount);
        }
    }
}
