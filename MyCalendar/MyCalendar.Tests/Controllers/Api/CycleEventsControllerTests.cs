using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyCalendar.Controllers.Api;
using MyCalendar.Core;
using MyCalendar.Core.Models;
using MyCalendar.Core.Repositories;
using MyCalendar.Tests.Extensions;
using System.Web.Http.Results;

namespace MyCalendar.Tests.Controllers.Api
{
    [TestClass]
    public class CycleEventsControllerTests
    {
        private CycleEventsController _controller;
        private Mock<ICycleEventRepository> _mockRepository;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<ICycleEventRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.CycleEvents).Returns(_mockRepository.Object);

            _controller = new CycleEventsController(mockUnitOfWork.Object);
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user1@domain.com");
        }

        [TestMethod]
        public void Cancel_NoEventWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_EventIsCanceled_ShouldReturnNotFound()
        {
            var cycleEvent = new Event();
            cycleEvent.Cancel();

            _mockRepository.Setup(r => r.GetCycleEvent(1)).Returns(cycleEvent);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancelingAnotherUsersEvent_ShouldReturnUnauthorized()
        {
            var cycleEvent = new Event { UserId = _userId + "-" };

            _mockRepository.Setup(r => r.GetCycleEvent(1)).Returns(cycleEvent);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var cycleEvent = new Event { UserId = _userId };

            _mockRepository.Setup(r => r.GetCycleEvent(1)).Returns(cycleEvent);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}
