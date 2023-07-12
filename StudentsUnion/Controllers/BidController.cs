using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentsUnion.Data;
using StudentsUnion.Models;
using StudentsUnion.ViewModels;

namespace StudentsUnion.Controllers
{
    [Authorize(Roles = "Admin, Volunteer")]
    public class BidController : Controller
    {
        private readonly DataContext db;
        public BidController(DataContext db)
        {
            this.db = db;
        }
        public IActionResult NewBids()
        {
            ViewBag.Title = "Новые заявки";

            IEnumerable<BidViewModel> model = db.Bids.Where(b => b.Status == "New").Select(b => new BidViewModel()
            {
                Id = b.Id,
                CreationTime = b.CreationTime,
                Phone = b.Phone,
                Description = b.Description,
                FIO = b.FIO,
                Status = b.Status,
                Type = b.Type,
            }).OrderByDescending(b => b.CreationTime);

            return View("Index", model);
        }
        public IActionResult WorkingBids()
        {
            ViewBag.Title = "Заявки в работе";

            IEnumerable<BidViewModel> model = db.Bids.Where(b => b.Status == "Working").Select(b => new BidViewModel()
            {
                Id = b.Id,
                CreationTime = b.CreationTime,
                Phone = b.Phone,
                Description = b.Description,
                FIO = b.FIO,
                Status = b.Status,
                Type = b.Type,
            }).OrderByDescending(b => b.CreationTime);

            return View("Index", model);
        }
        public IActionResult DoneBids()
        {
            ViewBag.Title = "Завершенные заявки";

            IEnumerable<BidViewModel> model = db.Bids.Where(b => b.Status == "Done").Select(b => new BidViewModel()
            {
                Id = b.Id,
                CreationTime = b.CreationTime,
                Phone = b.Phone,
                Description = b.Description,
                FIO = b.FIO,
                Status = b.Status,
                Type = b.Type,
            }).OrderByDescending(b => b.CreationTime);

            return View("Index", model);
        }
        public IActionResult CancelBids()
        {
            ViewBag.Title = "Отмененные заявки";

            IEnumerable<BidViewModel> model = db.Bids.Where(b => b.Status == "Cancel").Select(b => new BidViewModel()
            {
                Id = b.Id,
                CreationTime = b.CreationTime,
                Phone = b.Phone,
                Description = b.Description,
                FIO = b.FIO,
                Status = b.Status,
                Type = b.Type,
            }).OrderByDescending(b => b.CreationTime);

            return View("Index", model);
        }

        public async Task<IActionResult> EditBid(int? id)
        {
            ViewBag.Statuses = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Новый", Value = "New"},
                new SelectListItem() { Text = "В работе", Value = "Working"},
                new SelectListItem() { Text = "Завершено", Value = "Done"},
                new SelectListItem() { Text = "Отменено", Value = "Cancel"},
            };

            ViewBag.Types = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Адресная", Value = "Адресная"},
                new SelectListItem() { Text = "Психологическая", Value = "Психологическая"},
                new SelectListItem() { Text = "Гуманитарная", Value = "Гуманитарная"},
                new SelectListItem() { Text = "Иная", Value = "Иная"},
            };

            if (id != null)
            {
                Bid? bid = await db.Bids.FindAsync(id);
                if (bid != null)
                {

                    var model = new BidViewModel()
                    {
                        Id = bid.Id,
                        Description = bid.Description,
                        FIO = bid.FIO,
                        Phone = bid.Phone,
                        CreationTime = bid.CreationTime,
                        Status = bid.Status,
                        Type = bid.Type,
                    };

                    ViewBag.IsEditable = model.Status != "Done" && model.Status != "Cancel";

                    return View(model);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditBid(BidViewModel model)
        {
            var bid = new Bid()
            {
                Id = model.Id,
                Description = model.Description,
                Phone = model.Phone,
                FIO= model.FIO,
                Type = model.Type,
                Status = model.Status,
                CreationTime = DateTime.Now,
            };

            db.Bids.Update(bid);
            await db.SaveChangesAsync();

            return RedirectToAction($"{bid.Status}Bids");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteBid(int? id)
        {
            var bid = await db.Bids.FindAsync(id);
            if (bid != null && bid.Status != "Working")
            {
                db.Bids.Remove(bid);
                return RedirectToAction("NewBids");
            }
            return NotFound();
        }
    }
}
