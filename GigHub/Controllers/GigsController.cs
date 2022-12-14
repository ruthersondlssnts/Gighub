using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using GigHub.Persistence;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        public ViewResult Mine()
        {
            var userId = User.Identity.GetUserId();
            IEnumerable<Gig> gigs = _unitOfWork.Gigs.GetUpcomingGigsByArtist(userId);
            return View(gigs);
        }


        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Attending",
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }



        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            var viewModel = new GigFormViewModel
            {
                Id = gig.Id,
                Genres = _unitOfWork.Genres.GetGenre(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Venue = gig.Venue,
                Genre = gig.GenreId,
                Heading = "Edit a Gig"
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _unitOfWork.Genres.GetGenre();
                viewModel.Heading = "Create a Gig";
                return View("GigForm", viewModel);
            }
            var userId = User.Identity.GetUserId();

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _unitOfWork.Complete();
            return RedirectToAction("Mine", "Gigs");
        }


        public ActionResult Details(int id)
        {
            Gig gig = _unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                viewModel.IsAttending =
                    _unitOfWork.Attendances.GetAttendance(gig.Id, userId) != null;

                viewModel.IsFollowing =
                    _unitOfWork.Followings.GetFollowing(gig.ArtistId, userId) != null;
            }

            return View("Details", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _unitOfWork.Genres.GetGenre(),
                Heading = "Create a Gig"
            };

            return View("GigForm", viewModel);
        }


        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _unitOfWork.Genres.GetGenre();
                viewModel.Heading = "Create a Gig";
                return View("GigForm", viewModel);
            }
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };


            _unitOfWork.Gigs.Add(gig);
            gig.Create();
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }
    }
}