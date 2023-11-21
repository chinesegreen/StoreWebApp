using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly CatalogContext _context;

        public HomeController(
            CatalogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel model = new IndexViewModel();

            var all = await _context.Products.Where(p => !p.IsDeleted).ToListAsync();

            var trending = all.Where(p => p.IsTrending).ToList();

            if (all.Count <= 8)
            {
                model.RecentArrivals = all;
            }
            else
            {
                model.RecentArrivals = all.GetRange(all.Count - 8, all.Count);
            }

            if (trending.Count <= 4)
            {
                model.Trendings = trending;
            }
            else
            {
                model.Trendings = trending.GetRange(trending.Count - 4, trending.Count);
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}