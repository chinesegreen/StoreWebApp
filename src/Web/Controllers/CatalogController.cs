using Ardalis.GuardClauses;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web.Configuration;
using Web.Features.Category;
using Web.ViewModels;

namespace Web.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly CatalogContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(CatalogContext context,
            IMediator mediator,
            ILogger<CatalogController> logger)    
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("[Controller]")]
        [HttpGet("[Controller]/{pageId}")]
        public async Task<IActionResult> Catalog(int pageId = 1)
        {
            var products = await _context.Products.Where(p => !p.IsDeleted).ToListAsync();

            List<Product> page = new List<Product>();

            try
            {
                if (products != null && products.Any())
                {
                    page = GetPage(products, pageId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("/Error");
            }

            var model = new CatalogViewModel()
            {
                Products = page,
                CurrentPage = pageId
            };

            return View(model);
        }

        [Route("[Controller]/[Action]/{searchString}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var products = from p in _context.Products
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    (p.NormalizedName!.Contains(searchString.ToUpper()) || p.NormalizedVendorCode!.Contains(searchString.ToUpper()))
                    && !p.IsDeleted);
            }

            var viewProducts = await products.ToListAsync();

            var model = new CatalogViewModel()
            {
                Products = viewProducts
            };

            return View("Index", model);
        }

        public async Task<IActionResult> Product(int id)
        {
            var product = _context.Find<Product>(id);

            if (product != null)
            {
                // TODO
                //product.Categories = await _context.Categories
                //    .Where(c => c.)
                var model = new ProductViewModel()
                {
                    Product = product
                };

                return View(model);
            }

            return BadRequest();
        }

        [HttpGet("[controller]/[Action]/{category}")]
        public async Task<IActionResult> Category(string category)
        {
            var model = await _mediator.Send(new GetCategory(category.ToUpper()));

            return View("Index", model);
        }

        public static List<Product> GetPage(List<Product> products, int pageId)
        {
            int index = (pageId - 1) * 9;

            if (index < products.Count)
            {
                for (int i = 9; i > 0; i--)
                {
                    if (index + i <= products.Count)
                    {
                        var page = products.GetRange(index, i);
                        return page;
                    }
                }
            }

            throw new ArgumentException("Page not found");
        }
    }
}
