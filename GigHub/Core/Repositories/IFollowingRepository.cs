using GigHub.Core.Dtos;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        Following GetFollowing(string artistId, string userId);
        bool IsFollowAlreadyExist(FollowingDto dto, string userFollowerId);
        void Remove(Following following);
        void Add(Following following);
    }
}