﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Data.Entities;
using ShowLogger.Models;
using ShowLogger.Models.Api;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Areas.Shows.Views.Show.ViewModels;
using ShowLogger.Web.Data;

namespace ShowLogger.Areas.Shows.Controllers;

[Area("Shows")]
public class ShowController : BaseController
{
    private readonly IWatchedShowsRepository _watchedShowsRepository;
    private readonly IUserRepository _userRepository;
    private readonly IInfoRepository _infoRepository;

    public ShowController(
        UserManager<ApplicationUser> userManager,
        ILogger<BaseController> logger,
        IHttpContextAccessor httpContextAccessor,
        IWatchedShowsRepository watchedShowsRepository,
        IInfoRepository infoRepository,
        IUserRepository userRepository
    ) : base(userManager, logger, httpContextAccessor)
    {
        _watchedShowsRepository = watchedShowsRepository;
        _userRepository = userRepository;
        _infoRepository = infoRepository;
    }

    public IActionResult Index()
    {
        ShowPageModel model = new ShowPageModel();

        try
        {
            model.HasShowAreaAsDefault = _userRepository.GetDefaultArea(GetLoggedInUserId()) == "Shows";
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not index page.");
        }

        return View(model);
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
    public async Task<IActionResult> Update(ShowModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _watchedShowsRepository.UpdateShow(GetLoggedInUserId(), model);

                if (model.InfoId != null)
                {
                    TvEpisodeInfoModel episodeInfo = _infoRepository.GetTvEpisodeInfos(m => m.TvEpisodeInfoId == model.InfoId).First();
                    TvInfoModel tvInfo = _infoRepository.GetTvInfos(m => m.TvInfoId == episodeInfo.TvInfoId).First();

                    if (episodeInfo.Runtime == null || string.IsNullOrEmpty(episodeInfo.EpisodeOverview))
                    {
                        await _infoRepository.Download(GetLoggedInUserId(), new InfoApiDownloadModel
                        {
                            API = (INFO_API)episodeInfo.ApiType,
                            Type = INFO_TYPE.TV,
                            Id = tvInfo.ApiId
                        });
                    }
                }

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
    public async Task<IActionResult> AddNextEpisode(int showId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                int newShowId = _watchedShowsRepository.AddNextEpisode(GetLoggedInUserId(), showId);

                successful = newShowId != -1;

                if (!successful)
                {
                    ModelState.AddModelError("AddNextEpisode", "Could not add next episode.");
                }
                else
                {
                    ShowModel show = _watchedShowsRepository.GetShows(m => m.ShowId == newShowId).First();

                    if (show.InfoId != null)
                    { 
                        TvEpisodeInfoModel episodeInfo = _infoRepository.GetTvEpisodeInfos(m => m.TvEpisodeInfoId == show.InfoId).First();
                        TvInfoModel tvInfo = _infoRepository.GetTvInfos(m => m.TvInfoId == episodeInfo.TvInfoId).First();

                        if (episodeInfo.Runtime == null 
                            || string.IsNullOrEmpty(episodeInfo.EpisodeOverview) 
                            || string.IsNullOrEmpty(episodeInfo.ImageUrl)
                            || episodeInfo.EpisodeName == $"Episode {episodeInfo.EpisodeNumber}")
                        {
                            await _infoRepository.Download(GetLoggedInUserId(), new InfoApiDownloadModel
                            {
                                API = (INFO_API)episodeInfo.ApiType, 
                                Type = INFO_TYPE.TV,
                                Id = tvInfo.ApiId
                            });
                        }
                    } 
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not add next episode.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult AddOneDay(int showId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowsRepository.AddOneDay(GetLoggedInUserId(), showId);

                if (!successful)
                {
                    ModelState.AddModelError("AddOneDay", "Could not subtract one day to episode.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not add one day to episode.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult SubtractOneDay(int showId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowsRepository.SubtractOneDay(GetLoggedInUserId(), showId);

                if (!successful)
                {
                    ModelState.AddModelError("SubtractOneDay", "Could not subtract one day to episode.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not add one day to episode.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult LoadAddRangeWindow()
    {
        AddRangeModel model = new AddRangeModel
        {
            AddRangeDateWatched = DateTime.Now.GetEST().Date,
        };
        return PartialView("~/Areas/Shows/Views/Show/Editor/_AddRange.cshtml", model);
    }

    [HttpPost]
    public IActionResult AddRange(AddRangeModel model)
    {
        bool successful = false;
        try
        {
            if(model.AddRangeStartEpisode >= model.AddRangeEndEpisode)
            {
                ModelState.AddModelError("AddRangeStartEpisode", "Start cannot be greater than End.");
            }

            if (model.AddRangeDateWatched == null)
            {
                ModelState.AddModelError("AddRangeDateWatched", "Please select a date you watched the episodes on.");
            }

            if (ModelState.IsValid)
            {
                successful = _watchedShowsRepository.AddRange(GetLoggedInUserId(), model);

                if (!successful)
                {
                    ModelState.AddModelError("AddRange", "Could not add range.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not add range.");
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
                .OrderByDescending(m => m.ShowId)
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
            model = _watchedShowsRepository.GetTVStats(GetLoggedInUserId()).OrderByDescending(m => m.LastWatched).ThenByDescending(m => m.ShowId).ThenByDescending(m => m.ShowName);
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
            model = _watchedShowsRepository.GetMovieStats(GetLoggedInUserId()).OrderByDescending(m => m.DateWatched).ThenByDescending(m => m.MovieName).ToList();
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
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load friends watch history.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Shows/Views/Show/PartialViews/_FriendsWatchHistoryGrid.cshtml", model);
    }

    [HttpGet]
    public PartialViewResult YearStats_Read()
    {

        IEnumerable<YearStatsModel> model = new List<YearStatsModel>();

        try
        {
            int userId = GetLoggedInUserId();
            model = _watchedShowsRepository.GetYearStats(GetLoggedInUserId()).OrderByDescending(m => m.Year).ThenBy(m => m.UserId == userId ? 0 : 1).ThenBy(m => m.Name);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load year stats.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Shows/Views/Show/PartialViews/_YearStatsGrid.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> SearchApi(InfoApiSearchModel searchModel)
    {

        IEnumerable<ApiSearchResultModel> model = new List<ApiSearchResultModel>();

        try
        {
            ApiResultModel<IEnumerable<ApiSearchResultModel>> query = await _infoRepository.Search(GetLoggedInUserId(), searchModel);

            if(query.Result != ApiResults.Success)
            {
                return Json(new { data = query.Result, errors = GetErrorsFromModelState() }); ;
            }

            model = query.ApiResultContents.OrderByDescending(m => m.AirDate);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load api results.");
        }


        return PartialView("~/Areas/Shows/Views/Show/PartialViews/_SearchCards.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> WatchFromSearch(LoadWatchFromSearchModel downloadModel)
    {
        WatchFromSearchModel model = new WatchFromSearchModel
        {
            DateWatched = DateTime.Now.GetEST().Date,
            ShowName = downloadModel.Name,
            Id = downloadModel.Id,
            API = downloadModel.API,
            Type = downloadModel.Type,
        };

        DateTime airDate;

        if (downloadModel.Type == INFO_TYPE.TV)
        {
            model.ShowTypeId = (int)CodeValueIds.TV;
        }
        else
        {
            if (DateTime.TryParse(downloadModel.AirDate, out airDate))
            {

                model.ShowTypeId = (airDate >= DateTime.Now.AddDays(-45) ? (int)CodeValueIds.AMC : (int)CodeValueIds.MOVIE);
            }
        }
        return PartialView("~/Areas/Shows/Views/Show/Editor/_WatchFromSearch.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> AddWatchFromSearch(WatchFromSearchModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                DownloadResultModel info = await _infoRepository.Download(GetLoggedInUserId(), new InfoApiDownloadModel
                {
                    API = model.API,
                    Type = model.Type,
                    Id = model.Id,
                });

                if(info.Type == INFO_TYPE.TV)
                {
                    TvEpisodeInfoModel episode = _infoRepository.GetTvEpisodeInfos(m => m.TvInfoId == info.Id).FirstOrDefault(m => m.SeasonNumber == model.SeasonNumber && m.EpisodeNumber == model.EpisodeNumber);
                    
                    if(episode != null)
                    {
                        model.InfoId = episode.TvEpisodeInfoId;
                    }
                }
                else
                {
                    model.InfoId = (int)info.Id;
                }

                long id = _watchedShowsRepository.CreateShow(GetLoggedInUserId(), model);
                ShowModel show = _watchedShowsRepository.GetShows(m => m.ShowId == id).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not create show.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }
}
