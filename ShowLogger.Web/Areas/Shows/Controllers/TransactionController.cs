using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowLogger.Data.Entities;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Areas.Common;
using ShowLogger.Web.Data;

public class TransactionController : BaseController
{
    IWatchedShowsRepository _watchedShowsRepository;

    public TransactionController(
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

        IEnumerable<TransactionModel> model = new List<TransactionModel>();

        try
        {
            int userId = GetLoggedInUserId();
            model = _watchedShowsRepository.GetTransactions(userId, m => m.UserId == userId).OrderByDescending(m => m.TransactionId);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load transactions.");
        }

        return PartialView("~/Areas/Shows/Views/Transaction/PartialViews/_TransactionGrid.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadCreator()
    {
        TransactionModel model = new TransactionModel
        {
            TransactionDate = DateTime.Now.GetEST().Date,
            TransactionTypeId = (int)CodeValueIds.PURCHASE
        };
        return PartialView("~/Areas/Shows/Views/Transaction/Editor/_EditTransaction.cshtml", model);
    }

    [HttpPost]
    public IActionResult LoadEditor(TransactionModel model)
    {
        return PartialView("~/Areas/Shows/Views/Transaction/Editor/_EditTransaction.cshtml", model);
    }

    [HttpPost]
    public IActionResult Create(TransactionModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                long id = _watchedShowsRepository.CreateTransaction(GetLoggedInUserId(), model);
                model = _watchedShowsRepository.GetTransactions(GetLoggedInUserId(), m => m.TransactionId == model.TransactionId).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not create transaction.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult Update(TransactionModel? model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _watchedShowsRepository.UpdateTransaction(GetLoggedInUserId(), model);
                model = _watchedShowsRepository.GetTransactions(GetLoggedInUserId(), m => m.TransactionId == model.TransactionId).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not update transaction.");
        }

        return Json(new { data = model, errors = GetErrorsFromModelState() });
    }

    [HttpPost]
    public IActionResult Delete(int transactionId)
    {
        bool successful = false;
        try
        {
            if (ModelState.IsValid)
            {
                successful = _watchedShowsRepository.DeleteTransaction(GetLoggedInUserId(), transactionId);

                if (!successful)
                {
                    ModelState.AddModelError("Delete", "Could not delete transaction.");
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not delete transaction.");
        }

        return Json(new { data = successful, errors = GetErrorsFromModelState() });
    }

    #region Drop Down Lists

    [HttpGet]
    public JsonResult GetItemsDDL()
    {
        IEnumerable<ItemModel> model = new List<ItemModel>();

        try
        {
            int userId = GetLoggedInUserId();
            model = _watchedShowsRepository.GetTransactions(userId, m => m.UserId == GetLoggedInUserId())
                .GroupBy(m => new { m.Item, m.TransactionTypeId })
                .Select(m => new ItemModel 
                { 
                    TransactionTypeId = m.Key.TransactionTypeId,
                    Item = m.Key.Item,
                    CostAmt = m.Last().CostAmt
                }).OrderBy(m => m.TransactionTypeId).ThenBy(m => m.Item);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load items.");
        }

        return new JsonResult(model);
    }

    [HttpGet]
    public JsonResult GetMovies()
    {
        IEnumerable<CodeValueModel> model = new List<CodeValueModel>();

        try
        {
            int userId = GetLoggedInUserId();
            model = _watchedShowsRepository.GetShows(m => m.UserId == GetLoggedInUserId() && m.ShowTypeId == 1002)
                .Select(m => new CodeValueModel
                {
                    CodeValueId = m.ShowId,
                    DecodeTxt = m.ShowName,
                }).OrderByDescending(m => m.CodeValueId);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load items.");
        }

        return new JsonResult(model);
    }

    [HttpGet]
    public JsonResult GetTransactionTypes()
    {
        IEnumerable<CodeValueModel> model = new List<CodeValueModel>();

        try
        {
            model = _watchedShowsRepository.GetCodeValues(m => m.CodeTableId == (int)CodeTableIds.TRANSACTION_TYPE_ID);
        }
        catch (Exception ex)
        {
            HandleException(ex, "Could not load transaction types.");
        }

        return new JsonResult(model);
    }


    #endregion
}
