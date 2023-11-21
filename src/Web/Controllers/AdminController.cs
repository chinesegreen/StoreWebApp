using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Claims;
using Web.BindingModels;
using Web.ViewModels;

namespace Web.Controllers
{
    //[Authorize("Admin")]
    //[ValidateAntiForgeryToken]
    public class AdminController : BaseController
    {
        private readonly CatalogContext _context;

        public AdminController(
            CatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Catalog));
        }

        public async Task<IActionResult> Catalog()
        {
            var products = await _context.Products.ToListAsync();

            var model = new AdminProductsViewModel()
            {
                Products = products
            };

            return View("Products", model);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("[Controller]/Product/[Action]")]
        public async Task<IActionResult> Edit([FromForm] EditProductCommand cmd)
        {
            return Ok();
        }

        [HttpPost]
        [Route("[Controller]/Product/[Action]")]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return View("/Error");
            }
            
            var profilePic = cmd.Images[0];
            cmd.Images.RemoveAt(0);
            string filePath = await ImageLinkProvider.SaveFile(profilePic);

            var product = new Product()
            {
                Name = cmd.Name,
                NormalizedName = cmd.Name.ToUpper(),
                Description = cmd.Description,
                ValueTax = cmd.ValueTax,
                VendorCode = cmd.VendorCode,
                NormalizedVendorCode = cmd.VendorCode.ToUpper(),
                Price = cmd.Price,
                ImageLink = filePath,
                Dimensions = cmd.Dimensions
            };

            // Try without null check like dimensions

            if (cmd.PriceWithoutDiscount != null)
            {
                product.PriceWithoutDiscount = cmd.PriceWithoutDiscount;
            }

            if (cmd.Tags != null)
            {
                product.Tags = cmd.Tags.Select(t =>
                new Tag
                {
                    Name = t,
                    NormalizedName = t.ToUpper(),
                    Link = t
                }).ToList();
            }

            if (cmd.Images != null)
            {
                var filePaths = new List<string>();

                foreach (var item in cmd.Images)
                {
                    filePaths.Add(await ImageLinkProvider.SaveFile(item));
                }

                product.Additionals = filePaths.Select(p => new Additional
                {
                    ImageLink = p
                }).ToList();
            }

            _context.Add(product);

            await _context.SaveChangesAsync();

            return Ok(product.Id);
        }

        [HttpPost]
        public async Task<IActionResult> MakeTrending(int Id)
        {
            if (!ModelState.IsValid)
            {
                return View("/Error");
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] List<int> ids)
        {
            foreach (var id in ids)
            {
                var product = _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();

                if (product == null)
                {
                    return BadRequest();
                }

                _context.Remove(product);

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromSale([FromBody] ProductsModel model)
        {
            Console.WriteLine(model.ids);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            foreach (var id in model.ids)
            {
                var product = _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();

                if (product == null)
                {
                    return BadRequest();
                }

                product.IsDeleted = true;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [Route("[controller]/[action]/{searchString}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var products = from p in _context.Products
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => (p.NormalizedName!
                .Contains(searchString.ToUpper()) || p.NormalizedVendorCode!.Contains(searchString.ToUpper()))
                && !p.IsDeleted);
            }

            var viewProducts = await products.ToListAsync();

            var model = new AdminProductsViewModel()
            {
                Products = viewProducts,
                NumberOfPages = viewProducts.Count / 10
            };

            return View("Index", model);
        }
    }
}
