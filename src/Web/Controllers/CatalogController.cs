using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly CatalogContext _context;

        public CatalogController(CatalogContext context)
        {
            _context = context;
        }

        public IActionResult Product(int id)
        {
            var product = _context.Find<Product>(id);

            if (product == null)
            {

            }

            return View(product);
        }
    }
}
