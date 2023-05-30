using Lumia.DataContext;
using Lumia.Models;
using Lumia.ViewModels.TeamVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Lumia.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]

	public class TeamsController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public TeamsController(LumiaDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.ToListAsync();
            return View(teams);
        }
        public async Task<IActionResult> Details(int id)
        {
            Team? teams= await _context.Teams.FindAsync(id);
            if (teams == null)
            {
                return NotFound();
            }
            return View(teams);
        }
        public async Task<IActionResult> Create()
        {
            CreateTeamVM createTeamVM = new CreateTeamVM()
            {
                Jobs= await _context.Jobs.ToListAsync(),
            };
            return View(createTeamVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamVM createTeam)
        {
            if(!ModelState.IsValid)
            {
                createTeam.Jobs = await _context.Jobs.ToListAsync();
                return View(createTeam);
            }
            Team team =new Team()
            {
                Name= createTeam.Name,
                Surname=createTeam.Surname,
				Description=createTeam.Description,
                JobId=createTeam.JobId,
			}; 
            if (!createTeam.Image.ContentType.Contains("image/") && createTeam.Image.Length / 1024 > 4096)
			{
				createTeam.Jobs = await _context.Jobs.ToListAsync();
				ModelState.AddModelError("Image", "Yerimiz Yoxdu");
				return View(createTeam);
			}
			string newFileName = Guid.NewGuid().ToString() + createTeam.Image.FileName;
			string path = Path.Combine(_environment.WebRootPath, "assets", "img", "team" , newFileName);
			using (FileStream fs = new FileStream(path, FileMode.CreateNew))
			{
				await createTeam.Image.CopyToAsync(fs);
			}
			team.ImageName = newFileName;
			await _context.Teams.AddAsync(team);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			Team? team = await _context.Teams.FindAsync(id);
			if (team == null) { return NotFound(); }
			string path = Path.Combine(_environment.WebRootPath, "assets", "img", "team" , team.ImageName);

			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}
			_context.Teams.Remove(team);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
        public async Task<IActionResult> Edit(int id)
        {
            Team? team = await _context.Teams.FindAsync(id);
            if (team == null) { return NotFound(); }
            EditTeamVM vm = new EditTeamVM()
            {
                Name = team.Name,
                Surname = team.Surname,
                Description = team.Description,
                JobId = team.JobId,
                Jobs = await _context.Jobs.ToListAsync()
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(EditTeamVM editTeam, int id)
        {
            Team team = await _context.Teams.FindAsync(id);
            if (team == null) { return NotFound(); }
            if (!ModelState.IsValid)
            {
                editTeam.Jobs = await _context.Jobs.ToListAsync();
                return View(editTeam);
            }
            if (editTeam.Image != null)
            {
                string path = Path.Combine(_environment.WebRootPath, "assets", "img", "team" , team.ImageName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                string newFileName = Guid.NewGuid().ToString() + editTeam.Image.FileName;
                string pathCreate = Path.Combine(_environment.WebRootPath, "assets", "img", "team" , newFileName);

                using (FileStream fs = new FileStream(pathCreate, FileMode.CreateNew))
                {
                    await editTeam.Image.CopyToAsync(fs);
                }
                team.ImageName = newFileName;
            }
            team.Name=editTeam.Name;
            team.Surname = editTeam.Surname;
            team.Description = editTeam.Description;
            team.JobId= editTeam.JobId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
