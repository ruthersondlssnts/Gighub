using System.Data.Entity.ModelConfiguration;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class AttendanceConfiguration : EntityTypeConfiguration<Attendance>
    {
        public AttendanceConfiguration()
        {
            HasKey(a => new { a.GigId, a.AttendeeId });

            Property(a => a.GigId)
                .HasColumnOrder(1);

            Property(a => a.AttendeeId)
                .HasColumnOrder(2);
        }
    }
}