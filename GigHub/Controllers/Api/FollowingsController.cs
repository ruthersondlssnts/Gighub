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
    public class FollowingsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userFollowerId = User.Identity.GetUserId();


            if (_unitOfWork.Followings.IsFollowAlreadyExist(dto, userFollowerId))
                return BadRequest("Following already exist");

            var following = new Following
            {
                FolloweeId = dto.FolloweeId,
                FollowerId = userFollowerId
            };

            _unitOfWork.Followings.Add(following);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Unfollow(string id)
        {

            var following = _unitOfWork.Followings.GetFollowing(id, User.Identity.GetUserId());

            if (following == null)
                return NotFound();

            _unitOfWork.Followings.Remove(following);
            _unitOfWork.Complete();

            return Ok(id);
        }


    }
}
