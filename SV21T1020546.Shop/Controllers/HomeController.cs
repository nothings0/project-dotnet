using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.Shop.Models;

namespace SV21T1020546.Shop.Controllers
{
    public class HomeController : Controller
    {
        private const int PAGE_SIZE = 4;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";

        public IActionResult Index()
        {
            var featuredProducts = ProductDataService.GetProducts(4, 0);
            var latestProducts = ProductDataService.GetProducts(8, 4);

            var model = new HomeModel
            {
                FeaturedProducts = featuredProducts,
                LatestProducts = latestProducts
            };

            // Truyền model vào View
            return View(model);
        }
    }
}
