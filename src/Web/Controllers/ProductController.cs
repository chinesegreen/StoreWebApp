using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.BindingModels;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class ProductController : BaseController
    {
        private readonly CatalogContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(
            CatalogContext context,
            IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult AddToBasket(int productId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            return Ok();
        }
    }
}
