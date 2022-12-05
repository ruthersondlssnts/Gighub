using System.Data.Entity.ModelConfiguration;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public UserConfiguration()
        {
            Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            HasMany(g => g.UserNotifications)
                .WithRequired(a => a.User)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Followers)
                .WithRequired(f => f.Followee)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Followees)
                .WithRequired(f => f.Follower)
                .WillCascadeOnDelete(false);
        }
    }
}