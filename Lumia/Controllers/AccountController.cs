using Lumia.Models;
using Lumia.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lumia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register )
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                Name=register.Name,
                Email=register.Email,
                Surname=register.Surname,
                UserName=register.UserName,
            };
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded) 
            {
                foreach(IdentityError? error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
			IdentityResult resultRole = await _userManager.AddToRoleAsync(user, "Admin");
			if (!resultRole.Succeeded)
			{
				foreach (IdentityError? item in resultRole.Errors)
				{
					ModelState.AddModelError("", "Duzgun Yaz!!!");
				}
				return View();
			}
			return RedirectToAction(nameof(Login));
        } 
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
			if (!ModelState.IsValid)
			{
				return View();
			}
            AppUser user=await _userManager.FindByEmailAsync(login.Email);
            if(user==null)
            {
				ModelState.AddModelError("", "Duzgun Yaz");
                return View();
			}
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);

			if (!result.Succeeded)
			{
                ModelState.AddModelError("", "Duzgun Yaz");
                return View();
			}
            return RedirectToAction("Index", "Home");
		}
        [HttpPost]
        public async  Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        } 
        public async Task<IActionResult> AddRole()
        {
            IdentityRole role = new IdentityRole("Admin");
            await _roleManager.CreateAsync(role);
			return Json("ok");
        }
    }
}
