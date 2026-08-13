using IdentityMail.Web.DTOs.AdminSettingDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminSettingController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminSettingController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var model = new AdminSettingsDto
            {
                Profile = new UpdateAdminProfileDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    ProfileImageUrl = user.ProfileImageUrl
                }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(
            AdminSettingsDto model)
        {
            ModelState.Remove("Password.CurrentPassword");
            ModelState.Remove("Password.NewPassword");
            ModelState.Remove("Password.ConfirmNewPassword");

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var emailOwner =
                await _userManager.FindByEmailAsync(model.Profile.Email);

            if (emailOwner != null && emailOwner.Id != user.Id)
            {
                ModelState.AddModelError(
                    "Profile.Email",
                    "Bu e-posta başka bir kullanıcı tarafından kullanılıyor.");

                return View("Index", model);
            }

            var userNameOwner =
                await _userManager.FindByNameAsync(model.Profile.UserName);

            if (userNameOwner != null && userNameOwner.Id != user.Id)
            {
                ModelState.AddModelError(
                    "Profile.UserName",
                    "Bu kullanıcı adı daha önce alınmış.");

                return View("Index", model);
            }

            user.FirstName = model.Profile.FirstName.Trim();
            user.LastName = model.Profile.LastName.Trim();
            user.UserName = model.Profile.UserName.Trim();
            user.Email = model.Profile.Email.Trim();
            user.ProfileImageUrl =
                model.Profile.ProfileImageUrl?.Trim();

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View("Index", model);
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] =
                "Profil bilgileriniz güncellendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            AdminSettingsDto model)
        {
            ModelState.Remove("Profile.FirstName");
            ModelState.Remove("Profile.LastName");
            ModelState.Remove("Profile.UserName");
            ModelState.Remove("Profile.Email");

            if (!ModelState.IsValid)
            {
                await FillProfileAsync(model);
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.Password.CurrentPassword,
                model.Password.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                await FillProfileAsync(model);
                return View("Index", model);
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] =
                "Şifreniz başarıyla değiştirildi.";

            return RedirectToAction(nameof(Index));
        }

        private async Task FillProfileAsync(AdminSettingsDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return;
            }

            model.Profile = new UpdateAdminProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }
    }
}
