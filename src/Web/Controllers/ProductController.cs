using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.BindingModels;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Authorize("Admin")]
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
        public async Task<IActionResult> Add([FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string unique = Guid.NewGuid().ToString();

            string name = model.Image.FileName + unique;
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "img/products");

            string filePath = Path.Combine(uploads, name);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(fileStream);
            }

            return Ok();
        }
    }
}
