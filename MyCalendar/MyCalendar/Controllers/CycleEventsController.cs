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
    public class CycleEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CycleEventsController()
        {
            _context = new ApplicationDbContext();
        }
        
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CycleEventFormViewModel
            {
                Types = _context.Types.ToList(),
                Heading = "Add an Event"
            };

            return View("CycleEventForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var cycleEvent = _context.Events.Single(c => c.Id == id && c.UserId == userId);

            var viewModel = new CycleEventFormViewModel
            {
                Id = cycleEvent.Id,
                Types = _context.Types.ToList(),
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
            var userId = User.Identity.GetUserId();

            var cycleEvents = _context.Events
                .Where(e => e.UserId == userId && !e.IsCanceled)
                .Include(e => e.Type)
                .OrderByDescending(e => e.StartDate)
                .ToList();

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
            
            if (!ModelState.IsValid)
            {
                viewModel.Types = _context.Types.ToList();
                return View("CycleEventForm", viewModel);
            }

            var days = GetDaysToCalculate(viewModel.Type);

            var cycleEvent = new Event()
            {
                StartDate = viewModel.GetStartDate(),
                EndDate = viewModel.GetEndDate(days),
                TypeId = viewModel.Type,
                UserId = User.Identity.GetUserId()
            };

            _context.Events.Add(cycleEvent);
            _context.SaveChanges();

            return RedirectToAction("GetRecentEvents", "CycleEvents");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CycleEventFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Types = _context.Types.ToList();
                return View("CycleEventForm", viewModel);
            }
            
            var userId = User.Identity.GetUserId();
            var cycleEvent = _context.Events.Single(c => c.Id == viewModel.Id && c.UserId == userId);

            var days = GetDaysToCalculate(viewModel.Type);
            cycleEvent.Modify(viewModel.GetStartDate(), viewModel.GetEndDate(days), viewModel.Type);

            _context.SaveChanges();

            return RedirectToAction("GetRecentEvents", "CycleEvents");
        }

        private int GetDaysToCalculate(int type)
        {
            var userId = User.Identity.GetUserId();
            var days = 0;

            if (type != 1) return days;

            var periodEvents = _context.Events
                .Where(c => c.UserId == userId && !c.IsCanceled && c.TypeId == 1)
                .ToList();

            var totalDays = periodEvents.Sum(c => (c.EndDate - c.StartDate).TotalDays);
            days = (int) (totalDays / periodEvents.Count);

            return days;
        }
    }
}