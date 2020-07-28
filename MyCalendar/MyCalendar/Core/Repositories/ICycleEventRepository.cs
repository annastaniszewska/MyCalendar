using MyCalendar.Core.Models;
using System.Collections.Generic;

namespace MyCalendar.Core.Repositories
{
    public interface ICycleEventRepository
    {
        Event GetCycleEvent(int id);
        Event GetLatestOvulationEvent(string userId);
        List<Event> GetCycleEvents(string userId);
        List<Event> GetPeriodEvents(string userId);
        void Add(Event cycleEvent);
    }
}