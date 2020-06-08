using System;
using System.ComponentModel.DataAnnotations;

namespace MyCalendar.Models
{
    public class Event
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public Type Type { get; set; }
    }
}