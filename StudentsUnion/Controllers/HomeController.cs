using Microsoft.AspNetCore.Mvc;
using StudentsUnion.Data;
using StudentsUnion.Models;
using StudentsUnion.ViewModels;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace StudentsUnion.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext db;
        public HomeController(DataContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateBid(BidViewModel model)
        {
            var bid = new Bid()
            {
                CreationTime = DateTime.Now,
                FIO = model.FIO,
                Description = model.Description,
                Phone = model.Phone,
                Status = "New",
                Type = "Иная"
            };

            db.Bids.AddAsync(bid);
            db.SaveChangesAsync();

            TempData["Alert"] = "Спасибо! В ближайшее время с вами свяжется наш волонтер.";

            return RedirectToAction("Index");
        }
    }
}