using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;
using ShowLogger.Web.Models;
using System.Diagnostics;

namespace ShowLogger.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            ILogger<BaseController> logger,
            IHttpContextAccessor httpContextAccessor
                ) : base(userManager, logger, httpContextAccessor)
        {
            try
            {
                int userId = GetLoggedInUserId();

            }
            catch
            {
                int x = 0;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}