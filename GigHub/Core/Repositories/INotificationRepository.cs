using System.Collections.Generic;
using System.Linq;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface INotificationRepository
    {
        List<Notification> GetNotifications(string userId);
        IQueryable<UserNotification> GetUserNotifications(string userId);
    }
}