using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using Microsoft.AspNetCore.Authorization;
namespace SV21T1020546.Shop.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Đơn mua";
            var userData = User.GetUserData();
            int id = int.Parse(userData.UserId);
            var orders = OrderDataService.GetOrdersOfUser(id);
            return View(orders);
        }

        public IActionResult Detail(int id)
        {
            ViewBag.Title = "Chi tiết đơn hàng";
            var order = OrderDataService.GetOrder(id);
            return View(order);
        }

        public IActionResult Cancel(int id = 0)
        {
            var data = OrderDataService.CancelOrder(id);

            return RedirectToAction("Detail", new { id = id });
        }
    }
}
