using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.DataLayers;
using SV21T1020546.DomainModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020546.Shop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var userData = User.GetUserData();
            int id = int.Parse(userData.UserId);
            var account = CommonDataService.GetCustomer(id);
            return View(account);
        }

        public IActionResult Edit()
        {
            var userData = User.GetUserData();
            int id = int.Parse(userData.UserId);
            ViewBag.Title = "Cập nhật thông tin tài khoản";
            var data = CommonDataService.GetCustomer(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }


        [HttpPost]
        public IActionResult Save(Customer data)
        {
            //TODO: Kiem tra du lieu dau vao dung hay khong?
            ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật thông tin khách hàng";

            //Kiểm tra hợp lệ từ dữ liệu đầu vào
            //Kiểm tra nếu thấy dữ liệu không hợp lệ thì lưu trữ vào ModelState
            if (string.IsNullOrWhiteSpace(data.CustomerName))
                ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được rỗng");
            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được rỗng");
            if (string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Vui lòng nhập địa chỉ khách hàng");
            if (string.IsNullOrWhiteSpace(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn Tỉnh/Thành");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số điện thoại");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập email khách hàng");

            //Dựa vào thuộc tính IsValid của ModelState để biết có thông báo lỗi hay không
            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            if (data.CustomerID == 0)
            {
                int id = CommonDataService.AddCustomer(data);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateCustomer(data);
                if (result == false)
                {
                    ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword)
        {
            if (Request.Method == "POST")
            {

                if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    ModelState.AddModelError("Error", "Vui lòng nhập đầy đủ");
                    return View();
                }

                if (confirmPassword.Trim().Equals(newPassword.Trim()) == false)
                    ModelState.AddModelError("confirmPass", "Xác nhận lại mật khẩu sai");
                if (ModelState.IsValid == false)
                {
                    return View();
                }
                else
                {
                    var result = UserAccountService.ChangePassword(UserTypes.Customer, userName, oldPassword, newPassword);
                    if (result == true)
                    {
                        return RedirectToAction("Logout");
                    }
                    else
                    {
                        ModelState.AddModelError("oldPass", "Mật khẩu cũ không đúng");
                        return View();
                    }
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
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

            var userAccount = UserAccountService.Authorize(UserTypes.Customer, username, password);
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
                Address = userAccount.Address,
                Phone = userAccount.Phone,
                Province = userAccount.Province,
                Roles = userAccount.RoleNames?.Split(",").ToList()
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string rePassword)
        {
            ViewBag.Username = username;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rePassword))
            {
                ModelState.AddModelError("Error", "Vui lòng nhập đầy đủ thông tin.");
                return View();
            }

            if (!password.Equals(rePassword))
            {
                ModelState.AddModelError("Error", "Nhập lại mật khẩu không khớp.");
                return View();
            }

            var id = UserAccountService.Register(username, password);

            if (id <= 0)
            {
                ModelState.AddModelError("Error", "Email đã tồn tại.");
                return View();
            }

            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
