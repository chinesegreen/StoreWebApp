using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels;

namespace Web.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly CatalogContext _context;

        public CatalogController(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Where(p => !p.IsDeleted).ToListAsync();
            
            var model = new CatalogViewModel()
            {
                Products = products,
                NumberOfPages = products.Count() / 10
            };

            return View(model);
        }

        [Route("[controller]/[action]/{searchString}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var products = from p in _context.Products
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    p.NormalizedName!.Contains(searchString.ToUpper()) && !p.IsDeleted);
            }

            var viewProducts = await products.ToListAsync();

            var model = new CatalogViewModel()
            {
                Products = viewProducts,
                NumberOfPages = viewProducts.Count / 10
            };

            return View("Index", model);
        }

        public IActionResult Product(int id)
        {
            var product = _context.Find<Product>(id);
            
            if (product == null)
            {
                return BadRequest();
            }

            if (product.Tags == null)
            {
                product.Tags = new List<Tag>() { new Tag() { Name = "No categories", Link = "" } };
            }

            var model = new ProductViewModel()
            {
                Product = product
            };

            return View(product);
        }

        [HttpGet("[controller]/{category}")]
        public IActionResult Category(string category)
        {
            var products = new List<Product>();

            products.AddRange(_context.Products.Where(p => !p.IsDeleted));

            return View(products);
        }
    }
}
