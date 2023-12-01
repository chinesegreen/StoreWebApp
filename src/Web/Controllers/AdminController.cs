using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using SixLabors.ImageSharp.Memory;
using System.Diagnostics;
using System.Security.Claims;
using Web.BindingModels;
using Web.Configuration;
using Web.ViewModels;

namespace Web.Controllers
{
    //[Authorize("Admin")]
    //[ValidateAntiForgeryToken]
    public class AdminController : BaseController
    {
        private readonly CatalogContext _context;
        private readonly ILocalStorageService _storage;
        private readonly ILogger<AdminController> _logger;

        public AdminController(CatalogContext context,
            ILocalStorageService storage,
            ILogger<AdminController> logger)
        {
            _context = context;
            _storage = storage;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Products));
        }

        [HttpGet("[Controller]/[Action]")]
        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.OrderBy(p => p.Date).ToListAsync();

            var model = new CatalogViewModel()
            {
                Products = products
            };

            return View(model);
        }

        [HttpGet("[Controller]/Product/[Action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("[Controller]/Product/[Action]/{productId}")]
        public async Task<IActionResult> Edit(int productId)
        {
            var product = _context.Find<Product>(productId);

            if (product == null)
            {
                throw new ArgumentOutOfRangeException("Артем мовсесян артем артем мовсесян");
            }

            product.Images = await _context.Images
                    .Where(i => i.ProductId == product.Id).ToListAsync();

            product.Dimensions = _context.Find<Dimensions>(productId);

            var model = new ProductViewModel()
            {
                Product = product
            };

            return View(model);
        }

        [HttpPost]
        [Route("[Controller]/Product/[Action]")]
        public async Task<IActionResult> Edit([FromForm] EditProductCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return View("/Error");
            }

            var product = _context.Products.Where(p => p.Id == cmd.Id)
                    .FirstOrDefault();

            if (product != null)
            {
                product.SetQuantity(cmd.QuantityInStock);

                product.Name = cmd.Name;
                product.NormalizedName = cmd.Name.ToUpper();
                product.Price = cmd.Price;
                product.Description = cmd.Description;
                product.Manufacturer = cmd.Manufacturer;
                product.VendorCode = cmd.VendorCode;
                product.NormalizedVendorCode = cmd.VendorCode.ToUpper();
                product.ValueTax = cmd.ValueTax;
                product.IsTrending = cmd.IsTrending ?? false;
                product.Dimensions = new Dimensions()
                {
                    Width = cmd.Width,
                    Height = cmd.Height,
                    Length = cmd.Length,
                    Weight = cmd.Weight
                };
                product.Categories = (cmd.Categories ?? new List<string>() { "Empty" })
                    .Select(c => new Category()
                    {
                        Name = c,
                        NormalizedName = c.ToUpper()
                    }).ToList();
            }
            else
            {
                return BadRequest();
            }

            await _context.SaveChangesAsync();

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        [Route("[Controller]/Product/[Action]")]
        public async Task<IActionResult> EditImage([FromBody] EditImageCommand cmd)
        {
            var product = _context.Products.Where(p => p.Id == cmd.Id)
                    .FirstOrDefault();

            if (product != null)
            {
                if (cmd.Position == 0)
                {
                    product.Picture = await _storage.SaveFile(cmd.Replacer, "products");
                }
                else
                {
                    var image = await _context.Images
                        .Where(i => i.ProductId == cmd.Id && i.Position == cmd.Position)
                        .FirstOrDefaultAsync();

                    if (image != null)
                    {
                        image!.Image = await _storage.SaveFile(cmd.Replacer, "products");
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        [Route("[Controller]/Product/[Action]")]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return View("/Error");
            }

            var product = new Product()
            {
                Name = cmd.Name,
                NormalizedName = cmd.Name.ToUpper(),
                Price = cmd.Price,
                Description = cmd.Description,
                Manufacturer = cmd.Manufacturer,
                VendorCode = cmd.VendorCode,
                NormalizedVendorCode = cmd.VendorCode.ToUpper(),
                ValueTax = cmd.ValueTax,
                IsTrending = cmd.IsTrending ?? false,
                QuantityInStock = 0,
                Dimensions = new Dimensions()
                {
                    Width = cmd.Width,
                    Height = cmd.Height,
                    Length = cmd.Length,
                    Weight = cmd.Weight
                },
                Picture = await _storage.SaveFile(cmd.Picture, "products"),
                Categories = (cmd.Categories ?? new List<string>() { "Empty" })
                    .Select(c => new Category()
                    {
                        Name = c,
                        NormalizedName = c.ToUpper()
                    }).ToList()
            };

            if (cmd.Images != null && cmd.Images.Any())
            {
                var images = new List<ImageObj>();

                foreach (var image in cmd.Images)
                {
                    var imageObj = new ImageObj()
                    {
                        Image = await _storage.SaveFile(image, "products")
                    };

                    images.Add(imageObj);
                }

                product.Images = images;
            }

            _context.Add(product);

            await _context.SaveChangesAsync();

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] ProductsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            foreach (var id in model.Ids)
            {
                var product = _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();

                if (product != null)
                {
                    // TODO: Raises IOException: the client reset the request stream
                    //System.IO.File.Delete($"wwwroot/{product.Picture}");

                    var images = await _context.Images.Where(i => i.ProductId == product.Id).ToListAsync();

                    if (images != null && images.Any())
                    {
                        foreach (var image in images)
                        {
                            _context.Remove(image);
                            //System.IO.File.Delete($"wwwroot{image.Image}");
                        }
                    }

                    _context.Remove(product);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest();
                }
            }

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] ProductsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            foreach (var id in model.Ids)
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

            return Redirect("/Admin/Products");
        }

        [HttpPost]
        public async Task<IActionResult> Restore([FromBody] ProductsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            foreach (var id in model.Ids)
            {
                var product = _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();

                if (product == null)
                {
                    return BadRequest();
                }

                product.IsDeleted = false;

                await _context.SaveChangesAsync();
            }

            return Redirect("/Admin/Products");
        }

        [HttpGet("[controller]/[action]/{searchString}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var products = from p in _context.Products
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.NormalizedVendorCode!
                    .Contains(searchString.ToUpper()));
            }

            var viewProducts = await products.ToListAsync();

            var model = new CatalogViewModel()
            {
                Products = viewProducts
            };

            return View(nameof(Products), model);
        }
    }
}
