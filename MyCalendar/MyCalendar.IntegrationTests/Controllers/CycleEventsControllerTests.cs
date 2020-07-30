using FluentAssertions;
using MyCalendar.Core.Models;
using MyCalendar.Core.ViewModels;
using MyCalendar.IntegrationTests.Extensions;
using MyCalendar.Persistence;
using NUnit.Framework;
using System;
using System.Linq;

namespace MyCalendar.IntegrationTests.Controllers
{
    [TestFixture]
    public class CycleEventsControllerTests
    {
        private MyCalendar.Controllers.CycleEventsController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ApplicationDbContext();
            _controller = new MyCalendar.Controllers.CycleEventsController(new UnitOfWork(_context));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, Isolated]
        public void Update_WhenCalled_ShouldUpdateTheGivenEvent()
        {
            // Arrange
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            var type = _context.Types.Single(t => t.Id == 1);
            var cycleEvent = new Event { User = user, StartDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(-2), TypeId = type.Id };
            _context.Events.Add(cycleEvent);
            _context.SaveChanges();

            // Act
            var result = _controller.Update(new CycleEventFormViewModel
            {
                Id = cycleEvent.Id,
                StartDate = DateTime.Today.AddMonths(-1).ToString("d MMM yyyy"),
                Type = 2
            });

            // Assert
            _context.Entry(cycleEvent).Reload();
            cycleEvent.StartDate.Should().Be(DateTime.Today.AddMonths(-1));
            cycleEvent.TypeId.Should().Be(2);
        }
    }
}
