using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.Shop.Models;

namespace SV21T1020546.Shop.Controllers
{
    public class ProductDetailController : Controller
    {
        public IActionResult Index(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Product");
            }

            var product = ProductDataService.GetProduct(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            ViewBag.Title = product.ProductName;

            var relatedProducts = ProductDataService.GetProducts(8, 0);
            var data = new ProductDetailModel
            {
                ProductData = product,
                RelatedProducts = relatedProducts
            };
            
            return View(data);
        }
    }
}
