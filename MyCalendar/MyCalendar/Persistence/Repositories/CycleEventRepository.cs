using MyCalendar.Core.Models;
using MyCalendar.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MyCalendar.Persistence.Repositories
{
    public class CycleEventRepository : ICycleEventRepository
    {
        private readonly ApplicationDbContext _context;

        public CycleEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Event GetCycleEvent(int id)
        {
            return _context.Events.SingleOrDefault(c => c.Id == id);
        }

        public Event GetLatestOvulationEvent(string userId)
        {
            return _context.Events
                .OrderByDescending(o => o.StartDate)
                .FirstOrDefault(o => o.UserId == userId && !o.IsCanceled && o.TypeId == 2);
        }

        public List<Event> GetCycleEvents(string userId)
        {
            return _context.Events
                .Where(e => e.UserId == userId && !e.IsCanceled)
                .Include(e => e.Type)
                .OrderByDescending(e => e.StartDate)
                .ToList();
        }

        public List<Event> GetPeriodEvents(string userId)
        {
            return _context.Events
                .Where(c => c.UserId == userId && !c.IsCanceled && c.TypeId == 1)
                .OrderByDescending(p => p.StartDate)
                .ToList();
        }

        public List<Event> GetTwoLatestPeriodEvents(string userId)
        {
            return _context.Events
                .Where(p => p.UserId == userId && !p.IsCanceled && p.TypeId == 1)
                .OrderByDescending(p => p.StartDate)
                .Take(2)
                .ToList();
        }

        public void Add(Event cycleEvent)
        {
            _context.Events.Add(cycleEvent);
        }
    }
}