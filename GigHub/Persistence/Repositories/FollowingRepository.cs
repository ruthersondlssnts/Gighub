using System.Linq;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public Following GetFollowing(string artistId, string userId)
        {
            return _context.Followings
                    .SingleOrDefault(x => x.FolloweeId == artistId && x.FollowerId == userId);
        }
        public bool IsFollowAlreadyExist(FollowingDto dto, string userFollowerId)
        {
            return _context.Followings.Any(a => a.FolloweeId == dto.FolloweeId && a.FollowerId == userFollowerId);
        }
        public void Remove(Following following)
        {
            _context.Followings.Remove(following);
        }
        public void Add(Following following)
        {
            _context.Followings.Add(following);
        }
    }
}