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

            var cycleEvents = _context.Events
                .Where(e => e.UserId == userId)
                .Include(e => e.Type)
                .OrderByDescending(e => e.StartDate)
                .Take(3)
                .ToList();

            CycleEvent cycleModel;

            if (cycleEvents[1].TypeId == 2)
            {
                cycleModel = new CycleEvent
                {
                    StartDate = cycleEvents[0].StartDate,
                    EndDate = cycleEvents[0].EndDate,
                    StartDateOfPreviousEvent = cycleEvents[2].StartDate,
                    OvulationDate = cycleEvents[1].StartDate
                };
            }
            else
            {
                cycleModel = new CycleEvent
                {
                    StartDate = cycleEvents[0].StartDate,
                    EndDate = cycleEvents[0].EndDate,
                    StartDateOfPreviousEvent = cycleEvents[1].StartDate,
                    OvulationDate = DateTime.MinValue
                };
            }
            
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