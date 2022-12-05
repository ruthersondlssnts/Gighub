using System;
using System.Collections.Generic;
using System.Linq;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _context.Attendances
                            .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                            .ToList();
        }

        public Attendance GetAttendance(int gigId, string userId)
        {
            return _context.Attendances
                    .SingleOrDefault(x => x.GigId == gigId && x.AttendeeId == userId);

        }


        public void Remove(Attendance attendance)
        {
            _context.Attendances.Remove(attendance);
        }
        public void Add(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
        }

    }
}