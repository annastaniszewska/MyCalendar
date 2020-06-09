using MyCalendar.Models;
using System.Collections.Generic;

namespace MyCalendar.ViewModels
{
    public class CycleEventFormViewModel
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public byte Type { get; set; }

        public IEnumerable<Type> Types { get; set; }

        public string Time { get; set; }
    }
}