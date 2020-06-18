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
                Types = _context.Types.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult GetRecentEvents()
        {
            var userId = User.Identity.GetUserId();

            var cycleEvents = _context.Events
                .Where(e => e.UserId == userId)
                .Include(e => e.Type)
                .OrderByDescending(e => e.StartDate)
                .ToList();

            var recentCycleEvents = new List<CycleEvent>();

            for (var cycleEvent = 0; cycleEvent < cycleEvents.Count; cycleEvent++)
            {
                CycleEvent recentCycleEvent;

                if (cycleEvents[cycleEvent].TypeId == 2)
                {
                    recentCycleEvent = new CycleEvent
                    {
                        OvulationDate = cycleEvents[cycleEvent].StartDate,
                        TypeId = cycleEvents[cycleEvent].TypeId
                    };
                }
                else if (cycleEvent < cycleEvents.Count - 1)
                {
                    recentCycleEvent = new CycleEvent
                    {
                        StartDate = cycleEvents[cycleEvent].StartDate,
                        EndDate = cycleEvents[cycleEvent].EndDate,
                        StartDateOfPreviousEvent = cycleEvents[cycleEvent + 1].TypeId == 2 ? cycleEvents[cycleEvent + 2].StartDate : cycleEvents[cycleEvent + 1].StartDate,
                        TypeId = cycleEvents[cycleEvent].TypeId
                    };
                }
                else
                {
                    recentCycleEvent = new CycleEvent
                    {
                        StartDate = cycleEvents[cycleEvent].StartDate,
                        EndDate = cycleEvents[cycleEvent].EndDate,
                        StartDateOfPreviousEvent = DateTime.MinValue,
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
                return View("Create", viewModel);
            }

            var cycleEvent = new Event()
            {
                StartDate = viewModel.GetStartDate(),
                EndDate = viewModel.GetEndDate(),
                TypeId = viewModel.Type,
                UserId = User.Identity.GetUserId()
            };

            _context.Events.Add(cycleEvent);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}