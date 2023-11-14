using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CatalogContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            CatalogContext context,
            AppIdentityDbContext idContext)
        {
            _userManager = userManager;
            _context = context;
        }
    }
}
