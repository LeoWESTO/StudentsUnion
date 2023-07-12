using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StudentsUnion.Data;
using Microsoft.EntityFrameworkCore;
using StudentsUnion.ViewModels;

namespace StudentsUnion.Controllers
{
    public class AuthController : Controller
    {
        private readonly DataContext db;
        public AuthController(DataContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = await db.Users.FirstOrDefaultAsync(a =>
                    a.Email == model.Email &&
                    a.Password == model.Password &&
                    a.Role == "Admin"
                );
                if (admin != null)
                {
                    await Authenticate(admin.Id.ToString(), "Admin"); // аутентификация
                    return RedirectToAction("NewBids", "Bid");
                }

                var volunteer = await db.Users.FirstOrDefaultAsync(a =>
                    a.Email == model.Email &&
                    a.Password == model.Password &&
                    a.Role == "Volunteer"
                );
                if (volunteer != null)
                {
                    await Authenticate(volunteer.Id.ToString(), "Volunteer"); // аутентификация
                    return RedirectToAction("NewBids", "Bid");
                }

                ModelState.AddModelError("", "Некорректные ФИО и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(string userName, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
