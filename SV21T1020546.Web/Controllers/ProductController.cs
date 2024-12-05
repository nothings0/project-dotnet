using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.DomainModels;
using SV21T1020546.Web.Models;

namespace SV21T1020546.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 30;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";
        public IActionResult Index()
        {
            ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
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
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            var data = new Product()
            {
                ProductID = 0,
                Photo = "nophoto.png"
            };
            return View("Edit", data);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            var data = ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool a = ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var data = ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        public IActionResult Save(Product data, IFormFile? uploadPhoto)

        {
            ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
            if (string.IsNullOrWhiteSpace(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "*");
            if (data.Price == 0 || string.IsNullOrWhiteSpace(data.Price.ToString()))
                ModelState.AddModelError(nameof(data.Price), "*");
            if (string.IsNullOrWhiteSpace(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "*");
            if (data.SupplierID == -1)
                ModelState.AddModelError(nameof(data.SupplierID), "*");
            if (data.CategoryID == -1)
                ModelState.AddModelError(nameof(data.CategoryID), "*");

            // xử lý ảnh
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\products", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            else
            {
                ModelState.AddModelError(nameof(data.Photo), "*");
            }

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            //TODO: Kiem tra du lieu dau vao dung hay khong?
            if (data.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(data);
                return RedirectToAction("Edit", new {id = id });
            }
            else
            {
                bool result = ProductDataService.UpdateProduct(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    var data = new ProductPhoto()
                    {
                        PhotoID = 0,
                        Photo = "nophoto.png",
                        ProductID = id
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh của mặt hàng";
                    var photo = ProductDataService.GetPhoto(photoId);
                    return View(photo);
                case "delete":
                    bool rs = ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    var data = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của mặt hàng";
                    var at = ProductDataService.GetAttribute(attributeId);
                    return View(at);
                case "delete":
                    bool rs = ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {

            // xử lý ảnh
            if (uploadPhoto != null && uploadPhoto is IFormFile file && file.Length > 0)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\products", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            else
            {
                    ModelState.AddModelError(nameof(data.Photo), "*");
            }
            if (!ModelState.IsValid)
            {
                return View("Photo", data);
            }
            //TODO: Kiem tra du lieu dau vao dung hay khong?
            if (data.PhotoID == 0)
            {
                long id = ProductDataService.AddPhoto(data);
            }
            else
            {
                bool result = ProductDataService.UpdatePhoto(data);
            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }

        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            if (string.IsNullOrWhiteSpace(data.AttributeName))
                ModelState.AddModelError(nameof(data.AttributeName), "*");
            if (string.IsNullOrWhiteSpace(data.AttributeValue))
                ModelState.AddModelError(nameof(data.AttributeValue), "*");

            if (!ModelState.IsValid)
            {
                return View("Attribute", data);
            }
            //TODO: Kiem tra du lieu dau vao dung hay khong?
            if (data.AttributeID == 0)
            {
                long id = ProductDataService.AddAttribute(data);
            }
            else
            {
                bool result = ProductDataService.UpdateAttribute(data);
            }
            //return RedirectToAction("Index");
            return RedirectToAction("Edit", new { id = data.ProductID });
        }
    }
}