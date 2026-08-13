using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminUserDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public AdminUserController(
            UserManager<AppUser> userManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var adminUserIds = await (
                from user in _context.Users
                join userRole in _context.UserRoles
                    on user.Id equals userRole.UserId
                join role in _context.Roles
                    on userRole.RoleId equals role.Id
                where role.Name == "Admin"
                select user.Id
            ).ToListAsync();

            var query = _userManager.Users
                .AsNoTracking()
                .Where(user => !adminUserIds.Contains(user.Id));

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(user =>
                    user.FirstName.Contains(search) ||
                    user.LastName.Contains(search) ||
                    user.UserName.Contains(search) ||
                    user.Email.Contains(search));
            }

            var users = await query
                .OrderBy(user => user.FirstName)
                .ThenBy(user => user.LastName)
                .Select(user => new AdminUserListDto
                {
                    Id = user.Id,

                    FullName =
                        user.FirstName + " " + user.LastName,

                    UserName = user.UserName ?? string.Empty,

                    Email = user.Email ?? string.Empty,

                    ProfileImageUrl = user.ProfileImageUrl,

                    IsActive = user.IsActive,

                    SentMessageCount = _context.UserMessages
                        .Count(message =>
                            message.SenderId == user.Id),

                    ReceivedMessageCount = _context.UserMessages
                        .Count(message =>
                            message.ReceiverId == user.Id)
                })
                .ToListAsync();

            ViewBag.Search = search;

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _userManager.FindByIdAsync(
                id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["ErrorMessage"] =
                    "Admin hesabının durumu değiştirilemez.";

                return RedirectToAction(nameof(Index));
            }

            user.IsActive = !user.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] =
                    "Kullanıcı durumu güncellenemedi.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = user.IsActive
                ? "Kullanıcı hesabı aktifleştirildi."
                : "Kullanıcı hesabı pasifleştirildi.";

            return RedirectToAction(nameof(Index));
        }
    }
}

