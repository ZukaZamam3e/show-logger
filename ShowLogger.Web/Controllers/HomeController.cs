using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;
using ShowLogger.Web.Models;
using System.Diagnostics;

namespace ShowLogger.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFriendRepository _friendRepository;
        public HomeController(
            UserManager<ApplicationUser> userManager,
            IFriendRepository friendRepository,
            ILogger<BaseController> logger,
            IHttpContextAccessor httpContextAccessor
                ) : base(userManager, logger, httpContextAccessor)
        {
            _friendRepository = friendRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Thanks()
        {
            return View();
        }

        public IActionResult Friends()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult Friends_Read()
        {

            IEnumerable<FriendModel> model = new List<FriendModel>();

            try
            {
                model = _friendRepository.GetFriends(GetLoggedInUserId()).OrderByDescending(m => m.IsPending);
                model = from m in model
                        join u in _userManager.Users on m.FriendUserId equals u.UserId
                        select new FriendModel
                        {
                            FriendUserId = m.FriendUserId,
                            Id = m.Id,
                            IsPending = m.IsPending,
                            FriendEmail = u.Email,
                            CreatedDate = m.CreatedDate,
                        };
            }
            catch (Exception ex)
            {
                HandleException(ex, "Could not load friends.");
            }

            // Only grid query values will be available here.
            return PartialView("~/Views/Home/PartialViews/_FriendsGrid.cshtml", model);
        }

        [HttpPost]
        public IActionResult Friends_LoadCreator()
        {
            AddFriendModel model = new AddFriendModel();
            return PartialView("~/Views/Home/Editor/_AddFriend.cshtml", model);
        }

        [HttpPost]
        public IActionResult Friends_Create(AddFriendModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser? user = _userManager.Users.FirstOrDefault(m => m.Email.ToLower() == model.Email.ToLower() && m.UserId != GetLoggedInUserId());

                    if (user != null)
                    {
                        _friendRepository.SendFriendRequest(GetLoggedInUserId(), user.UserId);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Could not find user by email");
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Could not add friend.");
            }

            return Json(new { data = model, errors = GetErrorsFromModelState() });
        }

        [HttpPost]
        public IActionResult Friend_AcceptFriendRequest(int friendRequestId)
        {
            bool successful = false;
            try
            {
                if (ModelState.IsValid)
                {
                    successful = _friendRepository.AcceptFriendRequest(GetLoggedInUserId(), friendRequestId);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Could not accept friend request.");
            }

            return Json(new { data = successful, errors = GetErrorsFromModelState() });
        }

        [HttpPost]
        public IActionResult Friend_DenyFriendRequest(int friendRequestId)
        {
            bool successful = false;
            try
            {
                if (ModelState.IsValid)
                {
                    successful = _friendRepository.DenyFriendRequest(GetLoggedInUserId(), friendRequestId);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Could not deny friend request.");
            }

            return Json(new { data = successful, errors = GetErrorsFromModelState() });
        }

        [HttpPost]
        public IActionResult Friend_DeleteFriend(int friendId)
        {
            bool successful = false;
            try
            {
                if (ModelState.IsValid)
                {
                    successful = _friendRepository.DeleteFriend(GetLoggedInUserId(), friendId);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Could not delete friend.");
            }

            return Json(new { data = successful, errors = GetErrorsFromModelState() });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}