using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Web.Configuration;
using Web.ViewModels;

namespace Web.Controllers
{
    public class CartController : BaseController
    {
        const string SessionKey = "_Cart";

        private readonly CatalogContext _context;
        public CartController(CatalogContext context)
        {
            _context = context;
        }

        public IActionResult Index(string returnUrl)
        {
            return View(new CartViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public IActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = _context.Products
                .FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = _context.Products
                .FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Cart GetCart()
        {
            Cart cart = HttpContext.Session.Get<Cart>(SessionKey);

            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Session.Set<Cart>(SessionKey, cart);
            }
            return cart;
        }
    }
}
