using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.BindingModels;

namespace Web.Controllers
{
    //[Authorize("Admin")]
    //[ValidateAntiForgeryToken]
    public class AdminController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CatalogContext _context;

        public AdminController(
            IWebHostEnvironment hostingEnvironment,
            CatalogContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return View("/Error");
            }

            var product = new Product()
            {
                Name = cmd.Name,
                Description = cmd.Description,
                Price = cmd.Price,
                Tags = cmd.Tags.Select(t =>
                new Tag
                {
                    Name = t
                }).ToList()
            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product.Id);
        }

        [HttpPost]
        public async Task<IActionResult> AddImages(AddImagesCommand cmd)
        {
            // TODO Additional images must be not required

            if (!ModelState.IsValid)
            {
                return View("/Error");
            }

            string filePath = await ImageLinkProvider.SaveFile(cmd.Image);

            var product = new Product()
            {
                Id = cmd.ProductId,
                ImageLink = filePath
            };

            _context.Update(product);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCatalog(int productId)
        {
            return Ok();
        }
    }
}
