using Microsoft.AspNet.Identity;
using MyCalendar.Core;
using MyCalendar.Core.ViewModels;
using MyCalendar.Helpers;
using System;
using System.Linq;
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

            var periodEvents = _unitOfWork.CycleEvents.GetPeriodEvents(userId);
            var twoLatestPeriodEvents = periodEvents.Take(2).ToList();
            var ovulationEvent = _unitOfWork.CycleEvents.GetLatestOvulationEvent(userId);
            var futurePeriodDate = DateTime.MinValue;

            if (ovulationEvent != null || periodEvents.Count >= 2)
            {
                futurePeriodDate = DateCalculations.GetFuturePeriodDate(ovulationEvent, periodEvents);
            }

            var averageCycleLength = periodEvents.Count < 2 ? 0 : DateCalculations.GetAverageCycleLength(periodEvents);

            var cycleModel = new CycleEvent()
            {
                StartDate = twoLatestPeriodEvents.Count <= 2 && twoLatestPeriodEvents.Count != 0 ? twoLatestPeriodEvents[0].StartDate : DateTime.MinValue,
                EndDate = twoLatestPeriodEvents.Count <= 2 && twoLatestPeriodEvents.Count != 0 ? twoLatestPeriodEvents[0].EndDate : DateTime.MinValue,
                StartDateOfPreviousEvent = twoLatestPeriodEvents.Count == 2 ? twoLatestPeriodEvents[1].StartDate : DateTime.MinValue,
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