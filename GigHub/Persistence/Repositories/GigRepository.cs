﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class GigRepository : IGigRepository
    {
        private readonly IApplicationDbContext _context;

        public GigRepository(IApplicationDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Gig> SearchUpcomingGigs(string query, IQueryable<Gig> upcomingGigs)
        {
            return upcomingGigs
                .Where(g =>
                    g.Artist.Name.Contains(query) ||
                    g.Genre.Name.Contains(query) ||
                    g.Venue.Contains(query));
        }

        public IQueryable<Gig> UpcomingGigs()
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g =>
                    g.DateTime > DateTime.Now);
        }

        public Gig GetGig(int id)
        {
            return _context.Gigs
               .Include(x => x.Artist)
               .Include(x => x.Genre)
               .SingleOrDefault(x => x.Id == id);

        }

        public IEnumerable<Gig> GetUpcomingGigsByArtist(string userId)
        {
            return _context.Gigs
                .Where(g =>
                    g.ArtistId == userId &&
                    g.DateTime > DateTime.Now &&
                    !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();
        }

        public Gig GetGigWithAttendees(int gigId)
        {

            return _context.Gigs
                 .Include(g => g.Attendances.Select(a => a.Attendee))
                 .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _context.Attendances
                 .Where(a => a.AttendeeId == userId)
                 .Select(a => a.Gig)
                 .Include(a => a.Artist)
                 .Include(a => a.Genre)
                 .Where(g => g.DateTime > DateTime.Now)
                 .ToList();
        }


        public void Add(Gig gig)
        {
            _context.Gigs.Add(gig);
        }
    }
}