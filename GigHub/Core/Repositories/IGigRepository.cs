using System.Collections.Generic;
using System.Linq;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        Gig GetGig(int id);
        IEnumerable<Gig> GetUpcomingGigsByArtist(string userId);
        Gig GetGigWithAttendees(int gigId);
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        IQueryable<Gig> SearchUpcomingGigs(string query, IQueryable<Gig> upcomingGigs);
        IQueryable<Gig> UpcomingGigs();
        void Add(Gig gig);

    }
}