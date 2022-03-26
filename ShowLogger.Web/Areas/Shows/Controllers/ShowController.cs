﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;

namespace ShowLogger.Areas.Shows.Controllers;

public class ShowController : BaseController
{
    IWatchedShowsRepository _watchedShowsRepository;

    public ShowController(
        UserManager<ApplicationUser> userManager,
        ILogger<BaseController> logger,
        IHttpContextAccessor httpContextAccessor,
        IWatchedShowsRepository watchedShowsRepository
    ) : base(userManager, logger, httpContextAccessor)
    {
        _watchedShowsRepository = watchedShowsRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public PartialViewResult Read()
    {

        IEnumerable<ShowModel> model = new List<ShowModel>();

        try
        {
            model = _watchedShowsRepository.GetShows(m => m.UserId == GetLoggedInUserId()).OrderByDescending(m => m.DateWatched).ThenByDescending(m => m.ShowId);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load shows.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Shows/Views/Show/PartialViews/_ShowsGrid.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadCreator()
    {
        ShowModel model = new ShowModel
        {
            DateWatched = DateTime.Now.GetEST().Date,
        };
        return PartialView("~/Areas/Shows/Views/Show/Editor/_EditShow.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadEditor(ShowModel model)
    {
        return PartialView("~/Areas/Shows/Views/Show/Editor/_EditShow.cshtml", model);
    }

    [HttpPost]
    public IActionResult Create(ShowModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                long id = _watchedShowsRepository.CreateShow(GetLoggedInUserId(), model);
                model = _watchedShowsRepository.GetShows(m => m.ShowId == id).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not create show.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult Update(ShowModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _watchedShowsRepository.UpdateShow(GetLoggedInUserId(), model);
                model = _watchedShowsRepository.GetShows(m => m.ShowId == model.ShowId).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not update show.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult AddNextEpisode(int showId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowsRepository.AddNextEpisode(GetLoggedInUserId(), showId);

                if (!successful)
                {
                    ModelState.AddModelError("AddNextEpisode", "Could not add next episode.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not delete show.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult Delete(int showId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowsRepository.DeleteShow(GetLoggedInUserId(), showId);

                if (!successful)
                {
                    ModelState.AddModelError("Delete", "Could not delete show.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not delete show.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    [HttpGet]
    public JsonResult GetShowNamesDDL()
    {
        IEnumerable<ShowNameModel> model = new List<ShowNameModel>();

        try
        {
            model = _watchedShowsRepository.GetShows(m => m.UserId == GetLoggedInUserId() && m.ShowTypeId == 1000).Select(m => m.ShowName).Distinct().Select(m => new ShowNameModel { ShowName = m }).OrderBy(m => m.ShowName);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load show names.");
        }

        return new JsonResult(model);
    }

    [HttpGet]
    public JsonResult LoadShow(string showName)
    {
        ShowNameModel? model = new ShowNameModel();

        try
        {
            model = _watchedShowsRepository.GetShows(m => m.UserId == GetLoggedInUserId() && m.ShowName == showName)
                .OrderByDescending(m => m.SeasonNumber)
                .ThenByDescending(m => m.EpisodeNumber)
                .Select(m => new ShowNameModel 
                { 
                    ShowName = m.ShowName,
                    SeasonNumber = m.SeasonNumber,
                    EpisodeNumber = m.EpisodeNumber != null ? m.EpisodeNumber + 1 : null,
                    ShowTypeId = m.ShowTypeId
                }).FirstOrDefault();
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load show names.");
        }

        return new JsonResult(model);
    }

    [HttpGet]
    public PartialViewResult TVStats_Read()
    {

        IEnumerable<GroupedShowModel> model = new List<GroupedShowModel>();

        try
        {
            model = _watchedShowsRepository.GetTVStats(GetLoggedInUserId()).OrderByDescending(m => m.EpisodesWatched).ThenByDescending(m => m.ShowName);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load tv stats.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Shows/Views/Show/PartialViews/_TVStatsGrid.cshtml", model);
    }

    [HttpGet]
    public PartialViewResult MovieStats_Read()
    {

        IEnumerable<MovieModel> model = new List<MovieModel>();

        try
        {
            model = _watchedShowsRepository.GetMovieStats(GetLoggedInUserId()).OrderByDescending(m => m.DateWatched).ThenByDescending(m => m.MovieName);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load tv stats.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Shows/Views/Show/PartialViews/_MovieStatsGrid.cshtml", model);
    }

    [HttpGet]
    public PartialViewResult FriendsWatchHistory_Read()
    {

        IEnumerable<FriendWatchHistoryModel> model = new List<FriendWatchHistoryModel>();

        try
        {
            model = _watchedShowsRepository.GetFriendsWatchHistory(GetLoggedInUserId()).OrderByDescending(m => m.DateWatched).ThenByDescending(m => m.ShowId);

            model = from m in model
                    join u in _userManager.Users on m.UserId equals u.UserId
                    select new FriendWatchHistoryModel
                    {
                        UserId = m.UserId,
                        Email = u.Email,
                        DateWatched = m.DateWatched,
                        EpisodeNumber = m.EpisodeNumber,
                        SeasonNumber = m.SeasonNumber,
                        ShowId = m.ShowId,
                        ShowName = m.ShowName,
                        ShowNotes = m.ShowNotes,
                        ShowTypeId = m.ShowTypeId,
                        ShowTypeIdZ = m.ShowTypeIdZ,
                    };
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load friends watch history.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Shows/Views/Show/PartialViews/_FriendsWatchHistoryGrid.cshtml", model);
    }
}