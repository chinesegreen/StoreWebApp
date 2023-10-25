using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {

        }

        [HttpGet]
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            return View(model);
        }
    }
}