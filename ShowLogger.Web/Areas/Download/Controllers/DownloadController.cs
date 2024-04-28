using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Models;
using ShowLogger.Store.Repositories;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Books.Views.Book.ViewModels;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;
using System.IO.Compression;

namespace ShowLogger.Web.Areas.Download.Controllers;

[Area("Download")]
public class DownloadController : BaseController
{
    private readonly IDownloadRepository _downloadRepository;

    public DownloadController(
        UserManager<ApplicationUser> userManager,
        ILogger<BaseController> logger,
        IHttpContextAccessor httpContextAccessor,
        IDownloadRepository downloadRepository
    ) : base(userManager, logger, httpContextAccessor)
    {
        _downloadRepository = downloadRepository;
    }

    public IActionResult Index()
    {
        BookPageModel model = new BookPageModel();

        try
        {
            //model.HasBookAreaAsDefault = _userRepository.GetDefaultArea(GetLoggedInUserId()) == "Books";
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not index page.");
        }

        return View(model);
    }

    public IActionResult Export()
    {

        

        ExportModel model;
        byte[] fileBytes = null;

        try
        {
            int userId = GetLoggedInUserId();

            if (userId == 1000)
            {
                model = _downloadRepository.ExportData();

                Tuple<string, string>[] exports = new Tuple<string, string>[]
                {
                    new Tuple<string, string>("sl_show.json", model.SL_SHOW),
                    new Tuple<string, string>("sl_book.json", model.SL_BOOK),
                    new Tuple<string, string>("sl_friend.json", model.SL_FRIEND),
                    new Tuple<string, string>("sl_friend_request.json", model.SL_FRIEND_REQUEST),
                    new Tuple<string, string>("sl_watchlist.json", model.SL_WATCHLIST),
                    new Tuple<string, string>("sl_transaction.json", model.SL_TRANSACTION),
                    new Tuple<string, string>("sl_user_pref.json", model.SL_USER_PREF),
                    new Tuple<string, string>("oa_users.json", model.OA_USERS),
                    new Tuple<string, string>("sl_tv_info.json", model.SL_TV_INFO),
                    new Tuple<string, string>("sl_tv_episode_info.json", model.SL_TV_EPISODE_INFO),
                    new Tuple<string, string>("sl_movie_info.json", model.SL_MOVIE_INFO),
                    new Tuple<string, string>("sl_tv_episode_order.json", model.SL_TV_EPISODE_ORDER),
                };

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (Tuple<string, string> item in exports)
                        {
                            ZipArchiveEntry file = archive.CreateEntry(item.Item1);

                            using (var entryStream = file.Open())
                            using (var streamWriter = new StreamWriter(entryStream))
                            {
                                streamWriter.Write(item.Item2);
                            }
                        }
                    }

                    fileBytes = memoryStream.ToArray();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not export data.");
        }
        
        return File(fileBytes, "application/zip", $"export_{DateTime.Now:yyyyMMddhhmmss}.zip");
    }
}
