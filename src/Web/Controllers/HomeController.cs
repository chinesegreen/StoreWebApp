using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly CatalogContext _context;

        public HomeController(
            CatalogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel model = new IndexViewModel();

            var trending = await _context.Products.Where(p => p.IsTrending && !p.IsDeleted).ToListAsync();

            if (trending.Count <= 4)
            {
                model.Trendings = trending;
            }
            else
            {
                model.Trendings = trending.GetRange(trending.Count - 4, trending.Count);
            }

            var all = await _context.Products.Where(p => !p.IsDeleted).ToListAsync();

            if (all.Count <= 8)
            {
                model.RecentArrivals = all;
            }
            else
            {
                model.RecentArrivals = all.GetRange(all.Count - 8, all.Count);
            }

            return View(model);
        }
    }
}