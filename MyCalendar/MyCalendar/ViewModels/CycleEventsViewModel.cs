using MyCalendar.Models;
using System.Collections.Generic;

namespace MyCalendar.ViewModels
{
    public class CycleEventsViewModel
    {
        public IEnumerable<Event> RecentCycleEvents { get; set; }

        public IEnumerable<int> MenstrualCycles { get; set; }
    }
}