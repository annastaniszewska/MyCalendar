using Microsoft.AspNet.Identity;
using MyCalendar.Core;
using MyCalendar.Core.ViewModels;
using MyCalendar.Helpers;
using System;
using System.Web.Mvc;

namespace MyCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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