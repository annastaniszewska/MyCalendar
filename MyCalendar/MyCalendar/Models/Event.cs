using System;
using System.ComponentModel.DataAnnotations;

namespace MyCalendar.Models
{
    public class Event
    {
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Type Type { get; set; }

        [Required]
        public byte TypeId { get; set; }

        public void Cancel()
        {
            IsCanceled = true;
        }

        public void Modify(DateTime startDate, DateTime endDate, byte type)
        {
            StartDate = startDate;
            EndDate = endDate;
            TypeId = type;
        }
    }
}