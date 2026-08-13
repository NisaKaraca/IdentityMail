using IdentityMail.Web.DTOs.SettingsDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    [Authorize]
    public class SettingsController(
        UserManager<AppUser> _userManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name
            );

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new SettingsPageDto
            {
                Profile = new UpdateProfileDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    ProfileImageUrl = user.ProfileImageUrl
                },

                Password = new ChangePasswordDto()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(
    UpdateProfileDto updateProfileDto)
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name
            );

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(
                    "Index",
                    new SettingsPageDto
                    {
                        Profile = updateProfileDto,
                        Password = new ChangePasswordDto()
                    }
                );
            }

            var emailOwner = await _userManager.FindByEmailAsync(
                updateProfileDto.Email
            );

            if (emailOwner != null && emailOwner.Id != user.Id)
            {
                ModelState.AddModelError(
                    nameof(updateProfileDto.Email),
                    "Bu e-posta adresi başka bir kullanıcı tarafından kullanılıyor."
                );

                return View(
                    "Index",
                    new SettingsPageDto
                    {
                        Profile = updateProfileDto,
                        Password = new ChangePasswordDto()
                    }
                );
            }

            user.FirstName = updateProfileDto.FirstName.Trim();
            user.LastName = updateProfileDto.LastName.Trim();

            user.ProfileImageUrl =
                string.IsNullOrWhiteSpace(
                    updateProfileDto.ProfileImageUrl)
                        ? null
                        : updateProfileDto.ProfileImageUrl.Trim();

            if (user.Email != updateProfileDto.Email)
            {
                var emailResult = await _userManager.SetEmailAsync(
                    user,
                    updateProfileDto.Email
                );

                if (!emailResult.Succeeded)
                {
                    AddIdentityErrors(emailResult);

                    return View(
                        "Index",
                        new SettingsPageDto
                        {
                            Profile = updateProfileDto,
                            Password = new ChangePasswordDto()
                        }
                    );
                }

                var userNameResult =
                    await _userManager.SetUserNameAsync(
                        user,
                        updateProfileDto.Email
                    );

                if (!userNameResult.Succeeded)
                {
                    AddIdentityErrors(userNameResult);

                    return View(
                        "Index",
                        new SettingsPageDto
                        {
                            Profile = updateProfileDto,
                            Password = new ChangePasswordDto()
                        }
                    );
                }
            }

            var updateResult =
                await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                AddIdentityErrors(updateResult);

                return View(
                    "Index",
                    new SettingsPageDto
                    {
                        Profile = updateProfileDto,
                        Password = new ChangePasswordDto()
                    }
                );
            }

            TempData["SuccessMessage"] =
                "Profil bilgileriniz başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(
                    User.Identity.Name
                );

                if (user == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                return View(
                    "Index",
                    new SettingsPageDto
                    {
                        Profile = CreateProfileDto(user),
                        Password = changePasswordDto
                    }
                );
            }

            var currentUser = await _userManager.FindByNameAsync(
                User.Identity.Name
            );

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userManager.ChangePasswordAsync(
                currentUser,
                changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword
            );

            if (!result.Succeeded)
            {
                AddIdentityErrors(result);

                return View(
                    "Index",
                    new SettingsPageDto
                    {
                        Profile = CreateProfileDto(currentUser),
                        Password = changePasswordDto
                    }
                );
            }

            TempData["SuccessMessage"] =
                "Şifreniz başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        private static UpdateProfileDto CreateProfileDto(
            AppUser user)
        {
            return new UpdateProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description
                );
            }
        }
    }
}
