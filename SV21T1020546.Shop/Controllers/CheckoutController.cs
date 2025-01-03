using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020546.Shop.Models;

namespace SV21T1020546.Shop.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";
        public IActionResult Index()
        {
            return View(GetShoppingCart());
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
    }
}
