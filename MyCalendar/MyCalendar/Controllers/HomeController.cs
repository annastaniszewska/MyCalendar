using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using MyCalendar.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MyCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var periodEvents = _context.Events
                .Where(p => p.UserId == userId && !p.IsCanceled && p.TypeId == 1)
                .OrderByDescending(p => p.StartDate)
                .Take(2)
                .ToList();

            var ovulationEvent = _context.Events
                .SingleOrDefault(o => o.UserId == userId && !o.IsCanceled && o.TypeId == 2);

            var cycleModel = new CycleEvent()
            {
                StartDate = periodEvents.Count <= 2 && periodEvents.Count != 0 ? periodEvents[0].StartDate : DateTime.MinValue,
                EndDate = periodEvents.Count <= 2 && periodEvents.Count != 0 ? periodEvents[0].EndDate : DateTime.MinValue,
                StartDateOfPreviousEvent = periodEvents.Count == 2 ? periodEvents[1].StartDate : DateTime.MinValue,
                OvulationDate = ovulationEvent?.StartDate ?? DateTime.MinValue
            };

            return View("Index", cycleModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}