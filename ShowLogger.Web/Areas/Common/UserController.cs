using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;
using ShowLogger.Web.Models;
using System.Diagnostics;

[Area("Common")]
public class UserController : BaseController
{
    private readonly IUserRepository _userRepository;
    public UserController(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        ILogger<BaseController> logger,
        IHttpContextAccessor httpContextAccessor
            ) : base(userManager, logger, httpContextAccessor)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public IActionResult SetDefaultArea(string area)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _userRepository.SetDefaultArea(GetLoggedInUserId(), area);
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not set default area.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }
}
