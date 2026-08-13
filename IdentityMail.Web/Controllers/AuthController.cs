using IdentityMail.Web.DTOs.UserDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    public class AuthController(UserManager<AppUser> _userManager,
                                SignInManager<AppUser> _signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler birbiriyle ilgili değil!");
                return View(registerDto);
            }
            var user = new AppUser
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user,registerDto.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return View(registerDto);
            }
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "E-posta adresi veya şifre hatalı!");

                return View(loginDto);
            }
            if (!user.IsActive)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Hesabınız yönetici tarafından pasifleştirilmiştir.");

                return View(loginDto);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                loginDto.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "E-posta adresi veya şifre hatalı!");

                return View(loginDto);
            }

            var isAdmin = await _userManager
                .IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                return RedirectToAction(
                    "Index",
                    "AdminDashboard");
            }

            return RedirectToAction(
                "Index",
                "Message");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
