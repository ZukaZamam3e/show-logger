using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;

namespace ShowLogger.Areas.Shows.Controllers;

public class WatchlistController : BaseController
{
    IWatchedShowsRepository _watchedShowsRepository;

    public WatchlistController(
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

        IEnumerable<WatchlistModel> model = new List<WatchlistModel>();

        try
        {
            model = _watchedShowsRepository.GetWatchList(m => m.UserId == GetLoggedInUserId()).OrderByDescending(m => m.ShowName);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load watchlist.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Shows/Views/Watchlist/PartialViews/_WatchlistGrid.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadCreator()
    {
        WatchlistModel model = new WatchlistModel
        {
            DateAdded = DateTime.Now.GetEST().Date,
        };
        return PartialView("~/Areas/Shows/Views/Watchlist/Editor/_EditWatchlist.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadEditor(WatchlistModel model)
    {
        return PartialView("~/Areas/Shows/Views/Watchlist/Editor/_EditWatchlist.cshtml", model);
    }

    [HttpPost]
    public IActionResult Create(WatchlistModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                long id = _watchedShowsRepository.CreateWatchlist(GetLoggedInUserId(), model);
                model = _watchedShowsRepository.GetWatchList(m => m.WatchlistId == id).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not create watchlist.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult Update(WatchlistModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _watchedShowsRepository.UpdateWatchlist(GetLoggedInUserId(), model);
                model = _watchedShowsRepository.GetWatchList(m => m.WatchlistId == model.WatchlistId).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not update watchlist.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult Delete(int watchlistId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowsRepository.DeleteWatchlist(GetLoggedInUserId(), watchlistId);

                if (!successful)
                {
                    ModelState.AddModelError("Delete", "Could not delete watchlist.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not delete watchlist.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult MoveToShows(int watchlistId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowsRepository.MoveWatchlistToShow(GetLoggedInUserId(), watchlistId);

                if (!successful)
                {
                    ModelState.AddModelError("Delete", "Could not move watchlist to shows.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not move watchlist to shows.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }
}
