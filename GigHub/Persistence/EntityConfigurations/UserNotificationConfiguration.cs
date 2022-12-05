using System.Data.Entity.ModelConfiguration;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class UserNotificationConfiguration : EntityTypeConfiguration<UserNotification>
    {
        public UserNotificationConfiguration()
        {
            HasKey(a => new { a.UserId, a.NotificationId });

            Property(a => a.UserId)
                .HasColumnOrder(1);

            Property(a => a.NotificationId)
                .HasColumnOrder(2);
        }
    }
}