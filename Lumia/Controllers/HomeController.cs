using Lumia.DataContext;
using Lumia.Models;
using Lumia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lumia.Controllers
{
    public class HomeController : Controller
    {
        private readonly LumiaDbContext _context;

        public HomeController(LumiaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.Include(c=>c.Jobs).ToListAsync();
            HomeVM vm =new HomeVM()
            {
                Teams= teams
            };
            return View(vm);
        }
    }
}