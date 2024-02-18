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

            object routeValues;

            if(partialUrl.StartsWith("/Infos/Info/LoadTvSeasonTab"))
            {
                routeValues = new { area = a, tvInfoId = Convert.ToInt32(url[4]), seasonNumber = Convert.ToInt32(url[5]) };
            }
            else
            {
                routeValues = new { area = a };
            }
            //string e = url[4];
            //string i = url[5];

            return RedirectToAction(act, c, routeValues);
        }
        else
        {
            return PartialView(partialUrl);
        }
    }
}
