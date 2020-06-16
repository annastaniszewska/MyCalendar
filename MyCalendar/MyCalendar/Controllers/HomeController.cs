using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using MyCalendar.ViewModels;
using System.Collections.Generic;
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

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var latestEvent = _context.Events
                .Include(e => e.Type)
                .Where(e => e.UserId == userId && e.TypeId == 1)
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefault();

            var latestOvulationEvent = _context.Events
                .Include(e => e.Type)
                .Where(e => e.UserId == userId && e.TypeId == 2)
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefault();

            var periodEvents = _context.Events
                .Where(e => e.UserId == userId && e.TypeId == 1)
                .OrderByDescending(e => e.StartDate)
                .Take(2)
                .ToList();

            var cycleLengths = new List<int>();

            for (var i = 0; i < periodEvents.Count - 1; i++)
            {
                var cycleLength = (periodEvents[i].StartDate - periodEvents[i+1].StartDate).Days;
                cycleLengths.Add(cycleLength);
            }

            var mostRecentEvents = new List<Event>
            {
                latestEvent,
                latestOvulationEvent
            };

            var viewModel = new CycleEventsViewModel()
            {
                RecentCycleEvents = mostRecentEvents,
                MenstrualCycles = cycleLengths
            };

            return View("Index", viewModel);
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