using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyCalendar.Core.Models;
using MyCalendar.Persistence;
using MyCalendar.Persistence.Repositories;
using MyCalendar.Tests.Extensions;
using System.Collections.Generic;
using System.Data.Entity;

namespace MyCalendar.Tests.Persistence.Repositories
{
    [TestClass]
    public class CycleEventRepositoryTests
    {
        private CycleEventRepository _repository;
        private Mock<DbSet<Event>> _mockCycleEvents;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockCycleEvents = new Mock<DbSet<Event>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Events).Returns(_mockCycleEvents.Object);

            _repository = new CycleEventRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetCycleEvents_EventIsCanceled_ShouldNotBeReturned()
        {
            var cycleEvent = new Event {UserId = "1"};
            cycleEvent.Cancel();

            _mockCycleEvents.SetSource(new[] { cycleEvent });

            var cycleEvents = _repository.GetCycleEvents("1");

            cycleEvents.Should().BeEmpty();
        }

        [TestMethod]
        public void GetCycleEvents_EventIsForADifferentUser_ShouldNotBeReturned()
        {
            var cycleEvent = new Event { UserId = "1" };

            _mockCycleEvents.SetSource(new[] { cycleEvent });

            var cycleEvents = _repository.GetCycleEvents(cycleEvent.UserId + "-");

            cycleEvents.Should().BeEmpty();
        }

        [TestMethod]
        public void GetCycleEvents_EventIsForTheGivenUserAndIsNotCanceled_ShouldBeReturned()
        {
            var cycleEvent = new Event { UserId = "1" };

            _mockCycleEvents.SetSource(new[] { cycleEvent });

            var cycleEvents = _repository.GetCycleEvents(cycleEvent.UserId);

            cycleEvents.Should().Contain(cycleEvent);
        }

        [TestMethod]
        public void GetPeriodEvents_EventIsADifferentType_ShouldNotBeReturned()
        {
            var periodEvent = new Event { UserId = "1", TypeId = 2 };

            _mockCycleEvents.SetSource(new[] { periodEvent });

            var periodEvents = _repository.GetPeriodEvents("1");

            periodEvents.Should().BeEmpty();
        }

        [TestMethod]
        public void GetPeriodEvents_EventIsCanceled_ShouldNotBeReturned()
        {
            var periodEvent = new Event { UserId = "1", TypeId = 1 };
            periodEvent.Cancel();

            _mockCycleEvents.SetSource(new[] { periodEvent });

            var periodEvents = _repository.GetPeriodEvents("1");

            periodEvents.Should().BeEmpty();
        }

        [TestMethod]
        public void GetPeriodEvents_EventIsForADifferentUser_ShouldNotBeReturned()
        {
            var periodEvent = new Event { UserId = "1", TypeId = 1 };

            _mockCycleEvents.SetSource(new[] { periodEvent });

            var periodEvents = _repository.GetPeriodEvents(periodEvent.UserId + "-");

            periodEvents.Should().BeEmpty();
        }

        [TestMethod]
        public void GetPeriodEvents_EventIsForTheGivenUserAndIsNotCanceled_ShouldBeReturned()
        {
            var periodEvent = new Event { UserId = "1", TypeId = 1 };

            _mockCycleEvents.SetSource(new[] { periodEvent });

            var periodEvents = _repository.GetPeriodEvents(periodEvent.UserId);

            periodEvents.Should().BeOfType<List<Event>>();
            periodEvents.Should().Contain(periodEvent);
        }
    }
}
