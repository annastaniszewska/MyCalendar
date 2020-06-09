using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using MyCalendar.ViewModels;
using System;
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
                StartDate = DateTime.Parse($"{viewModel.StartDate} {viewModel.Time}"),
                EndDate = DateTime.Parse($"{viewModel.EndDate}"),
                TypeId = viewModel.Type,
                UserId = User.Identity.GetUserId()
            };

            _context.Events.Add(cycleEvent);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}