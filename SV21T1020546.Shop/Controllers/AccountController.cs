using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.DataLayers;
using SV21T1020546.Shop;

namespace SV21T1020546.Shop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            int id = 4311;
            var account = CommonDataService.GetCustomer(id);
            return View(account);
        }
    }
}
