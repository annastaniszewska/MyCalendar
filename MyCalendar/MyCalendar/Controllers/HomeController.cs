using Microsoft.AspNet.Identity;
using MyCalendar.Helpers;
using MyCalendar.Models;
using MyCalendar.Repositories;
using MyCalendar.ViewModels;
using System;
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

            var futurePeriodDate = DateCalculations.GetFuturePeriodDate(ovulationEvent, periodEvents);
            var averageCycleLength = DateCalculations.GetAverageCycleLength(periodEvents);

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