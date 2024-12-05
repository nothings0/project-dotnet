using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.DataLayers;

namespace SV21T1020546.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Error", "Vui lòng nhập đầy đủ");
                return View();
            }

            // Check username && password

            var userAccount = UserAccountService.Authorize(UserTypes.Employee, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập không thành công");
                return View();
            }

            var userData = new WebUserData()
            {
                UserId = userAccount.UserId,
                DisplayName = userAccount.DisplayName,
                Photo = userAccount.Photo,
                UserName = userAccount.UserName,
                Roles = userAccount.RoleNames?.Split(",").ToList()
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult AccessDenined()
        {
            return View();
        }
        
    }
}
