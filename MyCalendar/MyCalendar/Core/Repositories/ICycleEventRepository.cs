using System.Collections.Generic;
using MyCalendar.Core.Models;

namespace MyCalendar.Core.Repositories
{
    public interface ICycleEventRepository
    {
        Event GetCycleEvent(int id);
        Event GetLatestOvulationEvent(string userId);
        List<Event> GetCycleEvents(string userId);
        List<Event> GetPeriodEvents(string userId);
        List<Event> GetTwoLatestPeriodEvents(string userId);
        void Add(Event cycleEvent);
    }
}