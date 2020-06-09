using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Type = MyCalendar.Models.Type;

namespace MyCalendar.ViewModels
{
    public class CycleEventFormViewModel
    {
        [Required]
        [ValidDate]
        public string StartDate { get; set; }

        [Required]
        [ValidDate]
        public string EndDate { get; set; }

        [Required]
        public byte Type { get; set; }

        public IEnumerable<Type> Types { get; set; }
        
        public string Time { get; set; }

        public DateTime GetStartDate()
        {
            return DateTime.Parse($"{StartDate} {Time}");
        }

        public DateTime GetEndDate()
        {
            return DateTime.Parse($"{EndDate}");
        }
    }
}