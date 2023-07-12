using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentsUnion.Data;
using StudentsUnion.Models;
using StudentsUnion.ViewModels;
using System.Data;

namespace StudentsUnion.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly DataContext db;
        public UserController(DataContext db)
        {
            this.db = db;
        }
        public IActionResult Users()
        {
            IEnumerable<UserViewModel> model = db.Users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                FIO = u.FIO,
                Address = u.Address,
                Email = u.Email,
                Phone = u.Phone,
                CreationDate = u.CreationDate,
                Position = u.Position,
                Role = u.Role,
            });

            return View(model);
        }

        public IActionResult CreateUser()
        {
            ViewBag.Roles = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Админ", Value = "Admin"},
                new SelectListItem() { Text = "Волонтер", Value = "Volunteer"},
            };

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            var user = new User()
            {
                Email = model.Email,
                Role = model.Role,
                Position = model.Position,
                FIO= model.FIO,
                CreationDate = DateTime.Now,
                Address = model.Address,
                Phone = model.Phone,
                Password = model.Password
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> EditUser(int? id)
        {
            ViewBag.Roles = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Админ", Value = "Admin"},
                new SelectListItem() { Text = "Волонтер", Value = "Volunteer"},
            };

            if (id != null)
            {
                User? user = await db.Users.FindAsync(id);
                if (user != null)
                {

                    var model = new UserViewModel()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Role = user.Role,
                        Position = user.Position,
                        Address = user.Address,
                        CreationDate = user.CreationDate,
                        FIO = user.FIO,
                        Phone = user.Phone,
                        Password = user.Password,
                    };

                    return View(model);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            var user = new User()
            {
                Id = model.Id,
                Email = model.Email,
                Role = model.Role,
                Position = model.Position,
                FIO = model.FIO,
                CreationDate = model.CreationDate,
                Address = model.Address,
                Phone = model.Phone,
                Password = model.Password
            };

            db.Users.Update(user);
            await db.SaveChangesAsync();

            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            var user = await db.Users.FindAsync(id);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Users");
            }
            return NotFound();
        }
    }
}
