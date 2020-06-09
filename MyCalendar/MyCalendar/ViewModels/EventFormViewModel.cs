using MyCalendar.Models;
using System.Collections.Generic;

namespace MyCalendar.ViewModels
{
    public class EventFormViewModel
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int Type { get; set; }

        public IEnumerable<Type> Types { get; set; }

        public string Time { get; set; }
    }
}