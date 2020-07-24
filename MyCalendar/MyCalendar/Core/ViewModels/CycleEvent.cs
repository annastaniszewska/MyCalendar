using System;

namespace MyCalendar.Core.ViewModels
{
    public class CycleEvent
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDateOfPreviousEvent { get; set; }

        public DateTime OvulationDate { get; set; }

        public int TypeId { get; set; }

        public DateTime FuturePeriodDate { get; set; }

        public int AverageCycleLength { get; set; }

        public int CycleLength
        {
            get
            {
                return (StartDate - StartDateOfPreviousEvent).Days;
            }
        }

    }
}