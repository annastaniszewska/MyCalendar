using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using MyCalendar.ViewModels;
using System;
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
                .OrderByDescending(o => o.StartDate)
                .FirstOrDefault(o => o.UserId == userId && !o.IsCanceled && o.TypeId == 2);

            var futurePeriodDate = GetFuturePeriodDate();

            var cycleModel = new CycleEvent()
            {
                StartDate = periodEvents.Count <= 2 && periodEvents.Count != 0 ? periodEvents[0].StartDate : DateTime.MinValue,
                EndDate = periodEvents.Count <= 2 && periodEvents.Count != 0 ? periodEvents[0].EndDate : DateTime.MinValue,
                StartDateOfPreviousEvent = periodEvents.Count == 2 ? periodEvents[1].StartDate : DateTime.MinValue,
                OvulationDate = ovulationEvent?.StartDate ?? DateTime.MinValue,
                FuturePeriodDate = futurePeriodDate
            };

            return View("Index", cycleModel);
        }

        private DateTime GetFuturePeriodDate()
        {
            const int lutealPhaseDuration = 14;
            var cycleLengths = new List<int>();

            var latestOvulation = _context.Events.FirstOrDefault(o => o.TypeId == 2 && !o.IsCanceled);
            var periodEvents = _context.Events
                .Where(p => p.TypeId == 1 && !p.IsCanceled)
                .OrderByDescending(p => p.StartDate)
                .ToList();

            DateTime expectedPeriodDate;

            if (latestOvulation?.StartDate > periodEvents[0].StartDate)
            {
                expectedPeriodDate = latestOvulation.StartDate.Date.AddDays(lutealPhaseDuration);
            }
            else
            {
                for (var periodEvent = 0; periodEvent < periodEvents.Count - 1; periodEvent++)
                {
                    var cycleLength = (periodEvents[periodEvent].StartDate - periodEvents[periodEvent + 1].StartDate)
                        .Days;
                    cycleLengths.Add(cycleLength);
                }

                expectedPeriodDate = periodEvents[0].StartDate.Date.AddDays(cycleLengths.Average());
            }

            return expectedPeriodDate;
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