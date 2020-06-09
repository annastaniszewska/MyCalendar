using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using MyCalendar.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace MyCalendar.Controllers
{
    public class CycleEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CycleEventsController()
        {
            _context = new ApplicationDbContext();
        }
        
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CycleEventFormViewModel
            {
                Types = _context.Types.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CycleEventFormViewModel viewModel)
        {
            var cycleEvent = new Event()
            {
                StartDate = viewModel.StartDateParse,
                EndDate = viewModel.EndDateParse,
                TypeId = viewModel.Type,
                UserId = User.Identity.GetUserId()
            };

            _context.Events.Add(cycleEvent);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}