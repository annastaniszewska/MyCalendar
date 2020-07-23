using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using MyCalendar.Repositories;
using MyCalendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CycleEventRepository _cycleEventRepository;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _cycleEventRepository = new CycleEventRepository(_context);
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var periodEvents = _cycleEventRepository.GetTwoLatestPeriodEvents(userId);
            var ovulationEvent = _cycleEventRepository.GetLatestOvulationEvent(userId);

            var futurePeriodDate = GetFuturePeriodDate(ovulationEvent, periodEvents);
            var averageCycleLength = GetAverageCycleLength(periodEvents);

            var cycleModel = new CycleEvent()
            {
                StartDate = periodEvents.Count <= 2 && periodEvents.Count != 0 ? periodEvents[0].StartDate : DateTime.MinValue,
                EndDate = periodEvents.Count <= 2 && periodEvents.Count != 0 ? periodEvents[0].EndDate : DateTime.MinValue,
                StartDateOfPreviousEvent = periodEvents.Count == 2 ? periodEvents[1].StartDate : DateTime.MinValue,
                OvulationDate = ovulationEvent?.StartDate ?? DateTime.MinValue,
                FuturePeriodDate = futurePeriodDate,
                AverageCycleLength = averageCycleLength
            };

            return View("Index", cycleModel);
        }
        
        private static DateTime GetFuturePeriodDate(Event latestOvulation, List<Event> periodEvents)
        {
            const int lutealPhaseDuration = 14;
            var cycleLengths = new List<int>();
            
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

        private static int GetAverageCycleLength(List<Event> periodEvents)
        {
            var cycleLengths = new List<int>();

            for (var periodEvent = 0; periodEvent < periodEvents.Count - 1; periodEvent++)
            {
                var cycleLength = (periodEvents[periodEvent].StartDate - periodEvents[periodEvent + 1].StartDate)
                    .Days;
                cycleLengths.Add(cycleLength);
            }

            return (int)cycleLengths.Average();
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