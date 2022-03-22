using Microsoft.AspNetCore.Mvc;

namespace ShowLogger.Areas.Common.Controllers;

[Area("Common")]
public class GridController : Controller
{
    public IActionResult AjaxGrid(string actionUrl)
    {
        return PartialView("~/Views/Shared/Grid/_AjaxGrid.cshtml", actionUrl);
    }
}
