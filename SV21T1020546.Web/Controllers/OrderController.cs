using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using SV21T1020546.BusinessLayers;
using SV21T1020546.DomainModels;
using SV21T1020546.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace SV21T1020546.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]
    public class OrderController : Controller
    {
        private const int PAGE_SIZE = 50;
        private const string ORDER_SEARCH_CONDITION = "OrderSearchCondition";

        private const int PRODUCT_PAGE_SIZE = 5;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchForSale";

        private const string SHOPPING_CART = "ShoppingCart";
        public IActionResult Index()
        {
            OrderSearchInput? condition = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH_CONDITION);
            if (condition == null)
                condition = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    OrderTime = null
                };

            return View(condition);
        }
        public IActionResult Search(OrderSearchInput condition)
        {
            int rowCount;
            DateTime? fromTime = null, toTime = null;
            if (!string.IsNullOrEmpty(condition.OrderTime))
            {
                string[] dates = condition.OrderTime.Split(" - ");
                fromTime = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                toTime = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }


            var data = OrderDataService.ListOrders(out rowCount, condition.Page, condition.PageSize, condition.Status, fromTime, toTime, condition.SearchValue ?? " ");
            OrderSearchResult model = new OrderSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
                Status = condition.Status,
                FromTime = fromTime,
                ToTime = toTime
            };
            ApplicationContext.SetSessionData(ORDER_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Details(int id = 0)
        {
            var data = OrderDataService.GetOrder(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public IActionResult EditDetail(int id = 0, int productId = 0)
        {
            var data = OrderDataService.GetOrderDetail(id, productId);
            if (data == null)
                return RedirectToAction("Details");
            return View(data);
        }

        public IActionResult DeleteDetail(int id = 0, int productId = 0)
        {
            var data = OrderDataService.DeleteOrderDetail(id, productId);
            //if (data)
            return RedirectToAction("Details", new { id = id });

        }
        [HttpPost]
        public IActionResult UpdateDetail(int id = 0, int ProductID = 0, int quantity = 0, decimal salePrice = 0)
        {
            var data = OrderDataService.SaveOrderDetail(id, ProductID, quantity, salePrice);
            if (!data)
                return RedirectToAction("Details", new { id = id });
            else
                return RedirectToAction("Details", new { id = id });
        }
        public IActionResult Shipping(int id = 0, int shipperID = 0)
        {
            if(Request.Method == "POST")
            {
                if (shipperID == 0)
                    return Json("Vui lòng chọn người giao hàng");

                var data = OrderDataService.ShipOrder(id, shipperID);

                return Json("");
            }
            else
            {
                var data = OrderDataService.GetOrder(id);
                if (data == null)
                    return RedirectToAction("Details", new { id = id });
                return View(data);
            }
            
        }

        public IActionResult Accept(int id = 0)
        {
            var data = OrderDataService.AcceptOrder(id);

            return RedirectToAction("Details", new { id = id });
        }
        public IActionResult Finish(int id = 0)
        {
            var data = OrderDataService.FinishOrder(id);

            return RedirectToAction("Details", new { id = id });
        }
        public IActionResult Cancel(int id = 0)
        {
            var data = OrderDataService.CancelOrder(id);

            return RedirectToAction("Details", new { id = id });
        }
        public IActionResult Reject(int id = 0)
        {
            var data = OrderDataService.RejectOrder(id);

            return RedirectToAction("Details", new { id = id });
        }

        public IActionResult Delete(int id = 0)
        {
            var data = OrderDataService.DeleteOrder(id);

            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PRODUCT_PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(condition);
        }

        public IActionResult SearchProduct(ProductSearchInput condition)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            var model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
            return View(model);
        }
        private List<CartItem> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<CartItem>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }
            return shoppingCart;
        }
        public IActionResult AddToCart(CartItem item)
        {
            if (item.SalePrice < 0 || item.Quantity <= 0)
                return Json("Giá bán và số lượng không hợp lệ");
            var shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == item.ProductID);
            if (existsProduct == null)
            {
                shoppingCart.Add(item);
            }
            else
            {
                existsProduct.Quantity += item.Quantity;
                existsProduct.SalePrice = item.SalePrice;
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");

        }
        public IActionResult RemoveFromcart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public IActionResult ShoppingCart()
        {
            return View(GetShoppingCart());
        }
        public IActionResult Init(int customerID = 0, string deliveryProvince = "", string deliveryAddress = "")
        {
            var shoppingCart = GetShoppingCart();
            if (shoppingCart.Count == 0)
                return Json("Giỏ hàng trống. Vui lòng chọn mặt hàng cần bán");

            if (customerID == 0 || string.IsNullOrWhiteSpace(deliveryProvince) || string.IsNullOrWhiteSpace(deliveryAddress))
                return Json("Vui lòng nhập đầy đủ thông tin khách hàng và nơi giao hàng");
            //TODO: Thay bởi ID cảu nhân viên
            int employeeID = int.Parse(User.GetUserData()!.UserId);

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCart)
            {
                orderDetails.Add(new OrderDetail()
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    SalePrice = item.SalePrice

                });
            }
            int orderID = OrderDataService.InitOrder(employeeID, customerID, deliveryProvince, deliveryAddress, orderDetails);
            ClearCart();
            return Json(orderID);
        }
    }
}