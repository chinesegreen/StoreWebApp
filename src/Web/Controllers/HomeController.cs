using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web.ViewModels;

namespace Web.Controllers
{
    //[Authorize("Admin")]
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CatalogContext _context;

        public HomeController(
            IWebHostEnvironment hostingEnvironment,
            CatalogContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            model.Trendings = new List<Product>();
            model.RecentArrivals = new List<Product>();
            
            //for (int i = 0; i < 4; i++)
            //{
            //    _context.Find<Product>(1);
            //}

            return View(model);
        }

        public IActionResult Product(int productId)
        {
            var product = _context.Find<Product>(productId);

            return View(product);
        }
    }
}