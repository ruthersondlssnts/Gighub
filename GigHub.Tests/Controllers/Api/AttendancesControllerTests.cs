using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class AttendancesControllerTests
    {
        private AttendancesController _controller;
        private Mock<IAttendanceRepository> _mockRepository;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IAttendanceRepository>();

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.SetupGet(u => u.Attendances).Returns(_mockRepository.Object);

            _controller = new AttendancesController(mockUow.Object);

            _userId = "1";
            _controller.MockCurrentUser(_userId, "user1@domain.com");
        }

        [TestMethod]
        public void Attend_AttendanceAlreadyExist_ShouldReturnBadRequest()
        {
            var attendance = new Attendance();
            AttendanceDto dto = new AttendanceDto { GigId = 1 };
            _mockRepository.Setup(r => r.GetAttendance(dto.GigId, _userId)).Returns(attendance);
            var result = _controller.Attend(dto);
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }



        [TestMethod]
        public void Attend_ValidRequest_ShouldReturnOk()
        {
            AttendanceDto dto = new AttendanceDto { GigId = 1 };
            var result = _controller.Attend(dto);
            result.Should().BeOfType<OkResult>();
        }


        [TestMethod]
        public void DeleteAttendance_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.DeleteAttendance(1);
            result.Should().BeOfType<NotFoundResult>();
        }
        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnOk()
        {
            Attendance a = new Attendance();
            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(a);
            var result = _controller.DeleteAttendance(1);
            result.Should().BeOfType<OkNegotiatedContentResult<int>>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnIdOfDeleteAttendance()
        {
            var attendance = new Attendance();

            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);

            var result = _controller.DeleteAttendance(1) as OkNegotiatedContentResult<int>;

            result.Content.Should().Be(1);
        }
    }
}
