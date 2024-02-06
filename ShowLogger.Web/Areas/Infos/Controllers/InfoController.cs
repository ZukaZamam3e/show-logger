using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Models;
using ShowLogger.Models.Api;
using ShowLogger.Store.Repositories;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Books.Views.Book.ViewModels;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;

namespace ShowLogger.Web.Areas.Infos.Controllers;

[Area("Infos")]
public class InfoController : BaseController
{
    private readonly IInfoRepository _infoRepository;
    private readonly IWatchedShowsRepository _watchedShowRepository;
    //private readonly IUserRepository _userRepository;

    public InfoController(
        UserManager<ApplicationUser> userManager,
        ILogger<BaseController> logger,
        IHttpContextAccessor httpContextAccessor,
        IWatchedShowsRepository watchedShowRepository,
        IInfoRepository infoRepository,
        ApisConfig apiKeysConfig
    ) : base(userManager, logger, httpContextAccessor)
    {
        _infoRepository = infoRepository;
        _watchedShowRepository = watchedShowRepository;
        string email = GetLoggedInUserEmail();

        //if(email != "ffalex5678@gmail.com" ) { throw new Exception("Access Denied"); };
    }

    public async Task<IActionResult> Index()
    {
        //ApiResultModel<IEnumerable<ApiSearchResultModel>> model = new ApiResultModel<IEnumerable<ApiSearchResultModel>>();
        try
        {
            //model = await _infoRepository.Search(GetLoggedInUserId(), new InfoApiSearchModel
            //{
            //    API = INFO_API.TMDB_API,
            //    Type = INFO_TYPE.TV,
            //    Name = "The Walking Dead"
            //});

            //int x = 0;

            //model.HasBookAreaAsDefault = _userRepository.GetDefaultArea(GetLoggedInUserId()) == "Books";
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not index page.");
        }

        return View("Index");
    }


    [HttpPost]
    public async Task<PartialViewResult> SearchApi(InfoApiSearchModel searchModel)
    {

        List<ApiSearchResultModel> model = new List<ApiSearchResultModel>();

        try
        {
            ApiResultModel<IEnumerable<ApiSearchResultModel>> query = await _infoRepository.Search(GetLoggedInUserId(), searchModel);

            model = query.ApiResultContents.Where(m => m.Name.Equals(searchModel.Name, StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => m.AirDate).ToList();
            model.AddRange(query.ApiResultContents.Where(m => !m.Name.Equals(searchModel.Name, StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => m.AirDate).ToList());

            //model = query.ApiResultContents.OrderBy(m => m.Name.ToLower().Equals(searchModel.Name.ToLower()));
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load api results.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Infos/Views/Info/PartialViews/_SearchCards.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> DownloadInfo(InfoApiDownloadModel downloadModel)
    {
        DownloadResultModel model = new DownloadResultModel();

        try
        {
            model = await _infoRepository.Download(GetLoggedInUserId(), downloadModel);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not download info.");
        }

        // Only grid query values will be available here.
        return Json(new { data = model, errors = GetErrorsFromModelState() }); ;
    }

    [HttpPost]
    public async Task<IActionResult> RefreshTVInfo(int id)
    {
        DownloadResultModel model = new DownloadResultModel();

        try
        {
            TvInfoModel tv = _infoRepository.GetTvInfos(m => m.TvInfoId == id).FirstOrDefault();

            model = await _infoRepository.Download(GetLoggedInUserId(), new InfoApiDownloadModel
            {
                //API = tv.TMDbId != -1 ? INFO_API.TMDB_API : INFO_API.OMDB_API,
                API = (INFO_API)tv.ApiType,
                Type = INFO_TYPE.TV,
                //Id = tv.TMDbId != -1 ? tv.TMDbId.ToString() : tv.OMDbId,
                Id = tv.ApiId,
            });
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not download info.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() }); ;
    }

    [HttpPost]
    public async Task<IActionResult> RefreshMovieInfo(int id)
    {
        DownloadResultModel model = new DownloadResultModel();

        try
        {
            MovieInfoModel tv = _infoRepository.GetMovieInfos(m => m.MovieInfoId == id).FirstOrDefault();

            model = await _infoRepository.Download(GetLoggedInUserId(), new InfoApiDownloadModel
            {
                //API = tv.TMDbId != -1 ? INFO_API.TMDB_API : INFO_API.OMDB_API,
                API = (INFO_API)tv.ApiType,
                Type = INFO_TYPE.MOVIE,
                //Id = tv.TMDbId != -1 ? tv.TMDbId.ToString() : tv.OMDbId,
                Id = tv.ApiId,
            });
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not download info.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() }); ;
    }

    [HttpGet]
    public PartialViewResult ReadTVInfo()
    {

        IEnumerable<TvInfoModel> model = new List<TvInfoModel>();

        try
        {
            model = _infoRepository.GetTvInfos().OrderByDescending(m => m.ShowName);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load shows.");
        }

        return PartialView("~/Areas/Infos/Views/Info/PartialViews/_TVInfosGrid.cshtml", model);
    }

    [HttpGet]
    public PartialViewResult ReadMovieInfo()
    {

        IEnumerable<MovieInfoModel> model = new List<MovieInfoModel>();

        try
        {
            model = _infoRepository.GetMovieInfos().OrderByDescending(m => m.MovieName);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load movies.");
        }

        return PartialView("~/Areas/Infos/Views/Info/PartialViews/_MovieInfosGrid.cshtml", model);
    }

    [HttpGet]
    public IActionResult TVInfo(int id)
    {

        TvInfoModel model = new TvInfoModel();

        try
        {
            model = _infoRepository.GetTvInfos(m => m.TvInfoId == id).FirstOrDefault();
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load tv info.");
        }

        return View("~/Areas/Infos/Views/Info/PartialViews/_TVInfo.cshtml", model);
    }

    [HttpGet]
    public PartialViewResult ReadTVEpisodesInfo(int tvInfoId)
    {

        IEnumerable<TvEpisodeInfoModel> model = new List<TvEpisodeInfoModel>();

        try
        {
            model = _infoRepository.GetTvEpisodeInfos(m => m.TvInfoId == tvInfoId)
                .OrderByDescending(m => m.SeasonNumber > 0)
                .ThenBy(m => m.SeasonNumber)
                .ThenBy(m => m.EpisodeNumber)
                .ToList()
                .Select((m,i) =>
                {
                    m.OverallEpisodeNumber = i + 1;
                    return m;
                });
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load episodes.");
        }

        return PartialView("~/Areas/Infos/Views/Info/PartialViews/_TVEpisodeInfosGrid.cshtml", model);
    }

    [HttpGet]
    public IActionResult MovieInfo(int id)
    {

        MovieInfoModel model = new MovieInfoModel();

        try
        {
            model = _infoRepository.GetMovieInfos(m => m.MovieInfoId == id).FirstOrDefault();
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load movie info.");
        }

        return View("~/Areas/Infos/Views/Info/PartialViews/_MovieInfo.cshtml", model);
    }

    [HttpGet]
    public PartialViewResult ReadUnlinkedShows()
    {

        IEnumerable<UnlinkedShowsModel> model = new List<UnlinkedShowsModel>();

        try
        {
            model = _infoRepository.GetUnlinkedShows().OrderByDescending(m => m.InShowLoggerIndc).ThenBy(m => m.ShowName);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load unlinked shows.");
        }

        return PartialView("~/Areas/Infos/Views/Info/PartialViews/_UnlinkedShowsGrid.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadUpdateUnlinkedShowNameWindow()
    {
        UpdateUnlinkedShowNameModel model = new UpdateUnlinkedShowNameModel();
        return PartialView("~/Areas/Infos/Views/Info/Editor/_UpdateUnlinkedShowName.cshtml", model);
    }

    [HttpPost]
    public IActionResult UpdateUnlinkedShowName(UpdateUnlinkedShowNameModel model)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowRepository.UpdateShowNames(model);

                if (!successful)
                {
                    ModelState.AddModelError("UpdateUnlinkedShowName", "Could not update show names.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not update show names.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult LinkShows(LinkShowsModel model)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowRepository.LinkShows(model);

                if (!successful)
                {
                    ModelState.AddModelError("LinkShows", "Could not link shows.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not link shows.");
            ModelState.AddModelError("LinkShows", ex.Message);
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }
}
