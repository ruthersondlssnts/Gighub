using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;

namespace GigHub.Controllers.Api
{

    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttendancesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();

            var attendance = _unitOfWork.Attendances.GetAttendance(dto.GigId, userId);

            if (attendance != null)
                return BadRequest("The attendance already exists.");

            attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _unitOfWork.Attendances.Add(attendance);
            _unitOfWork.Complete();
            return Ok();
        }



        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            var attendance = _unitOfWork.Attendances.GetAttendance(id, User.Identity.GetUserId());

            if (attendance == null)
                return NotFound();

            _unitOfWork.Attendances.Remove(attendance);
            _unitOfWork.Complete();

            return Ok(id);
        }


    }
}
