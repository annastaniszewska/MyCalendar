using MyCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCalendar.Helpers
{
    public static class DateCalculations
    {
        public static int GetDaysToCalculate(List<Event> periodEvents)
        {
            var totalDays = periodEvents.Sum(c => (c.EndDate - c.StartDate).TotalDays);
            var days = (int)(totalDays / periodEvents.Count);

            return days;
        }

        public static DateTime GetFuturePeriodDate(Event latestOvulation, List<Event> periodEvents)
        {
            const int lutealPhaseDuration = 14;
            var cycleLengths = new List<int>();

            DateTime expectedPeriodDate;

            if (latestOvulation?.StartDate > periodEvents[0].StartDate)
            {
                expectedPeriodDate = latestOvulation.StartDate.Date.AddDays(lutealPhaseDuration);
            }
            else
            {
                for (var periodEvent = 0; periodEvent < periodEvents.Count - 1; periodEvent++)
                {
                    var cycleLength = (periodEvents[periodEvent].StartDate - periodEvents[periodEvent + 1].StartDate)
                        .Days;
                    cycleLengths.Add(cycleLength);
                }

                expectedPeriodDate = periodEvents[0].StartDate.Date.AddDays(cycleLengths.Average());
            }

            return expectedPeriodDate;
        }

        public static int GetAverageCycleLength(List<Event> periodEvents)
        {
            var cycleLengths = new List<int>();

            for (var periodEvent = 0; periodEvent < periodEvents.Count - 1; periodEvent++)
            {
                var cycleLength = (periodEvents[periodEvent].StartDate - periodEvents[periodEvent + 1].StartDate)
                    .Days;
                cycleLengths.Add(cycleLength);
            }

            return (int)cycleLengths.Average();
        }
    }
}