using System;
using System.Collections.Generic;
using Type = MyCalendar.Models.Type;

namespace MyCalendar.ViewModels
{
    public class CycleEventFormViewModel
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public byte Type { get; set; }

        public IEnumerable<Type> Types { get; set; }

        public string Time { get; set; }

        public DateTime StartDateParse => DateTime.Parse($"{StartDate} {Time}");

        public DateTime EndDateParse => DateTime.Parse($"{EndDate}");
    }
}