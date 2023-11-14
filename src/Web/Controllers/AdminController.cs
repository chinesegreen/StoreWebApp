using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;
using Web.BindingModels;

namespace Web.Controllers
{
    //[Authorize("Admin")]
    //[ValidateAntiForgeryToken]
    [ApiController]
    [Route("api")]
    public class AdminController : BaseController
    {
        private readonly CatalogContext _context;

        public AdminController(
            CatalogContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Product/[Action]")]
        public async Task<IActionResult> Add([FromForm] CreateProductCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return Problem(cmd.ToJson());
            }

            string filePath = await ImageLinkProvider.SaveFile(cmd.Image);

            var product = new Product()
            {
                Name = cmd.Name,
                Description = cmd.Description,
                Price = cmd.Price,
                ImageLink = filePath
            };

            if (cmd.Tags != null)
            {
                product.Tags = cmd.Tags.Select(t =>
                new Tag
                {
                    Name = t,
                    Link = t
                }).ToList();
            }

            if (cmd.Additionals != null)
            {
                var filePaths = new List<string>();

                foreach (var item in cmd.Additionals)
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
        public async Task<IActionResult> Delete(int id)
        {
            var product = _context.Products
                .Where(p => p.Id.Equals(id))
                .FirstOrDefault();

            if (product == null)
            {
                return BadRequest();
            }

            product.IsDeleted = true;
            
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
