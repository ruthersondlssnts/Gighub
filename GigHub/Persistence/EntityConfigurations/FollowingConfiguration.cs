using System.Data.Entity.ModelConfiguration;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class FollowingConfiguration : EntityTypeConfiguration<Following>
    {
        public FollowingConfiguration()
        {
            HasKey(a => new { a.FollowerId, a.FolloweeId });

            Property(a => a.FollowerId)
                .HasColumnOrder(1);

            Property(a => a.FolloweeId)
                .HasColumnOrder(2);
        }
    }
}