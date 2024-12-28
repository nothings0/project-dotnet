using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;

namespace SV21T1020546.Shop.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            int userId = 4311;
            var orders = OrderDataService.GetOrdersOfUser(userId);
            return View(orders);
        }

        public IActionResult Detail(int id)
        {
            var order = OrderDataService.GetOrder(id);
            return View(order);
        }
    }
}
