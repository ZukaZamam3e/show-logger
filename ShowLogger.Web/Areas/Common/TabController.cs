using Microsoft.AspNetCore.Mvc;

namespace ShowLogger.Areas.Common.Controllers;

[Area("Common")]
public class TabController : Controller
{
    public IActionResult Partial(string partialUrl)
    {
        if (!partialUrl.EndsWith(".cshtml"))
        {
            if (!string.IsNullOrEmpty(Request.PathBase))
            {
                partialUrl = partialUrl.Replace(Request.PathBase, "");
            }

            string[] url = partialUrl.Split("/");
            string a = url[1];
            string c = url[2];
            string act = url[3];
            string e = url[4];
            string i = url[5];

            return RedirectToAction(act, c, new { area = a, eventId = Convert.ToInt32(e), id = Convert.ToInt32(i) });
        }
        else
        {
            return PartialView(partialUrl);
        }
    }
}
