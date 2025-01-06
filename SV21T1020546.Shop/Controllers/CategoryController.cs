using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.Shop.Models;

namespace SV21T1020546.Shop.Controllers
{
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 12;
        private const string PRODUCT_SEARCH_CONDITION = "ProductCategory";
        public IActionResult Index(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Product");
            }

            var category = CommonDataService.GetCategory(id);

            if (category == null)
            {
                return RedirectToAction("Index", "Product");
            }
            ViewBag.Title = category.CategoryName;
            ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION + "-" + id);
            if (condition == null)
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = id,
                    SupplierID = 0,
                    MinPrice = 0,
                    MaxPrice = 0
                };

            return View(condition);

        }
        public IActionResult Search(ProductSearchInput condition)
        {
            int rowCount;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.CategoryID, condition.SupplierID, condition.MinPrice, condition.MaxPrice);
            ProductSearchResult model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                MinPrice = condition.MinPrice,
                MaxPrice = condition.MaxPrice,
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION + "-" + condition.CategoryID, condition);
            return View(model);
        }
    }
}
