using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Books.Views.Book.ViewModels;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;

namespace BookLogger.Web.Areas.Books.Controllers;
[Area("Books")]
public class BookController : BaseController
{
    IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;

    public BookController(
        UserManager<ApplicationUser> userManager,
        ILogger<BaseController> logger,
        IHttpContextAccessor httpContextAccessor,
        IBookRepository bookRepository,
        IUserRepository userRepository
    ) : base(userManager, logger, httpContextAccessor)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        BookPageModel model = new BookPageModel();

        try
        {
            model.HasBookAreaAsDefault = _userRepository.GetDefaultArea(GetLoggedInUserId()) == "Books";
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

        IEnumerable<BookModel> model = new List<BookModel>();

        try
        {
            model = _bookRepository.GetBooks(m => m.UserId == GetLoggedInUserId()).OrderByDescending(m => m.EndDate == null).ThenByDescending(m => m.EndDate).ThenByDescending(m => m.StartDate);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load Books.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Books/Views/Book/PartialViews/_BooksGrid.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadCreator()
    {
        BookModel model = new BookModel();
        return PartialView("~/Areas/Books/Views/Book/Editor/_EditBook.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadEditor(BookModel model)
    {
        return PartialView("~/Areas/Books/Views/Book/Editor/_EditBook.cshtml", model);
    }

    [HttpPost]
    public IActionResult Create(BookModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                long id = _bookRepository.CreateBook(GetLoggedInUserId(), model);
                model = _bookRepository.GetBooks(m => m.BookId == id).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not create Book.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult Update(BookModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _bookRepository.UpdateBook(GetLoggedInUserId(), model);
                model = _bookRepository.GetBooks(m => m.BookId == model.BookId).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not update Book.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult Delete(int bookId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _bookRepository.DeleteBook(GetLoggedInUserId(), bookId);

                if (!successful)
                {
                    ModelState.AddModelError("Delete", "Could not delete Book.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not delete Book.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    [HttpGet]
    public PartialViewResult BookYearStats_Read()
    {

        IEnumerable<YearStatsBookModel> model = new List<YearStatsBookModel>();

        try
        {
            model = _bookRepository.GetYearStats(GetLoggedInUserId()).OrderByDescending(m => m.Year).ThenBy(m => m.Name);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load year stats.");
        }

        // Only grid query values will be available here.
        return PartialView("~/Areas/Books/Views/Book/PartialViews/_YearStatsBookGrid.cshtml", model);
    }
}
