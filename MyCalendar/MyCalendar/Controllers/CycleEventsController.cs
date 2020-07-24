using Microsoft.AspNet.Identity;
using MyCalendar.Core;
using MyCalendar.Core.Models;
using MyCalendar.Core.ViewModels;
using MyCalendar.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyCalendar.Controllers
{
    public class CycleEventsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CycleEventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CycleEventFormViewModel
            {
                Types = _unitOfWork.Types.GetEventTypes(),
                Heading = "Add an Event"
            };

            return View("CycleEventForm", viewModel);
        }
        
        [Authorize]
        public ActionResult Edit(int id)
        {
            var cycleEvent = _unitOfWork.CycleEvents.GetCycleEvent(id);

            if (cycleEvent == null)
            {
                return HttpNotFound();
            }

            if (cycleEvent.UserId != User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new CycleEventFormViewModel
            {
                Id = cycleEvent.Id,
                Types = _unitOfWork.Types.GetEventTypes(),
                StartDate = cycleEvent.StartDate.ToString("d MMM yyyy"),
                EndDate = cycleEvent.EndDate.ToString("d MMM yyyy"),
                Type = cycleEvent.TypeId,
                Time = cycleEvent.StartDate.ToString("HH:mm"),
                Heading = "Edit an Event"
            };

            return View("CycleEventForm",viewModel);
        }

        [Authorize]
        public ActionResult GetRecentEvents()
        {
            var cycleEvents = _unitOfWork.CycleEvents.GetCycleEvents(User.Identity.GetUserId());

            var recentCycleEvents = new List<CycleEvent>();
            
            for (var cycleEvent = 0; cycleEvent < cycleEvents.Count; cycleEvent++)
            {
                CycleEvent recentCycleEvent;
                if (cycleEvent < cycleEvents.Count - 2)
                {
                    recentCycleEvent = new CycleEvent
                    {
                        Id = cycleEvents[cycleEvent].Id,
                        StartDate = cycleEvents[cycleEvent].StartDate,
                        EndDate = cycleEvents[cycleEvent].EndDate,
                        StartDateOfPreviousEvent = cycleEvents[cycleEvent + 1].TypeId == 2
                            ? cycleEvents[cycleEvent + 2].StartDate
                            : cycleEvents[cycleEvent + 1].StartDate,
                        OvulationDate = cycleEvents[cycleEvent].TypeId == 2
                            ? cycleEvents[cycleEvent].StartDate
                            : DateTime.MinValue,
                        TypeId = cycleEvents[cycleEvent].TypeId
                    };
                }
                else if (cycleEvent < cycleEvents.Count - 1)
                {
                    recentCycleEvent = new CycleEvent
                    {
                        Id = cycleEvents[cycleEvent].Id,
                        StartDate = cycleEvents[cycleEvent].StartDate,
                        EndDate = cycleEvents[cycleEvent].EndDate,
                        StartDateOfPreviousEvent = cycleEvents[cycleEvent + 1].TypeId == 2 || cycleEvents[cycleEvent].TypeId == 2
                            ? DateTime.MinValue
                            : cycleEvents[cycleEvent + 1].StartDate,
                        OvulationDate = cycleEvents[cycleEvent].TypeId == 2
                            ? cycleEvents[cycleEvent].StartDate
                            : DateTime.MinValue,
                        TypeId = cycleEvents[cycleEvent].TypeId
                    };
                }
                else
                {
                    recentCycleEvent = new CycleEvent
                    {
                        Id = cycleEvents[cycleEvent].Id,
                        StartDate = cycleEvents[cycleEvent].StartDate,
                        EndDate = cycleEvents[cycleEvent].EndDate,
                        StartDateOfPreviousEvent = DateTime.MinValue,
                        OvulationDate = cycleEvents[cycleEvent].TypeId == 2
                            ? cycleEvents[cycleEvent].StartDate
                            : DateTime.MinValue,
                        TypeId = cycleEvents[cycleEvent].TypeId
                    };
                }

                recentCycleEvents.Add(recentCycleEvent);
            }

            var viewModel = new CycleEventsViewModel()
            {
                RecentCycleEvents = recentCycleEvents
            };

            return View("MyRecentEvents", viewModel);
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CycleEventFormViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                viewModel.Types = _unitOfWork.Types.GetEventTypes();
                return View("CycleEventForm", viewModel);
            }

            var periodEvents = _unitOfWork.CycleEvents.GetPeriodEvents(userId);
            var days = viewModel.Type == 1 ? DateCalculations.GetDaysToCalculate(periodEvents) : 0; ;

            var cycleEvent = new Event()
            {
                StartDate = viewModel.GetStartDate(),
                EndDate = viewModel.GetEndDate(days),
                TypeId = viewModel.Type,
                UserId = userId
            };

            _unitOfWork.CycleEvents.Add(cycleEvent);
            _unitOfWork.Complete();

            return RedirectToAction("GetRecentEvents", "CycleEvents");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CycleEventFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Types = _unitOfWork.Types.GetEventTypes();
                return View("CycleEventForm", viewModel);
            }
            
            var userId = User.Identity.GetUserId();
            var cycleEvent = _unitOfWork.CycleEvents.GetCycleEvent(viewModel.Id);

            if (cycleEvent == null)
            {
                return HttpNotFound();
            }

            if (cycleEvent.UserId != userId)
            {
                return new HttpUnauthorizedResult();
            }

            var periodEvents = _unitOfWork.CycleEvents.GetPeriodEvents(userId);
            var days = viewModel.Type == 1 ? DateCalculations.GetDaysToCalculate(periodEvents) : 0;

            cycleEvent.Modify(viewModel.GetStartDate(), viewModel.GetEndDate(days), viewModel.Type);

            _unitOfWork.Complete();

            return RedirectToAction("GetRecentEvents", "CycleEvents");
        }
    }
}