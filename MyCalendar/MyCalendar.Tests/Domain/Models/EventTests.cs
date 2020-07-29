using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCalendar.Core.Models;

namespace MyCalendar.Tests.Domain.Models
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void Cancel_WhenCalled_ShouldSetIsCanceledToTrue()
        {
            var cycleEvent = new Event();

            cycleEvent.Cancel();

            cycleEvent.IsCanceled.Should().BeTrue();
        }
    }
}
