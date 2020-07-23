using Microsoft.AspNet.Identity;
using MyCalendar.Helpers;
using MyCalendar.Models;
using MyCalendar.Persistence;
using MyCalendar.ViewModels;
using System;
using System.Web.Mvc;

namespace MyCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var periodEvents = _unitOfWork.CycleEvents.GetTwoLatestPeriodEvents(userId);
            var ovulationEvent = _unitOfWork.CycleEvents.GetLatestOvulationEvent(userId);

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