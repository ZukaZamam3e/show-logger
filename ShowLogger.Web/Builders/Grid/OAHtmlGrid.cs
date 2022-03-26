using NonFactors.Mvc.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowLogger.Web.Builders.Grid;

public class OAHtmlGrid<T>
    where T : class
{
    public bool Header => HeaderButtons.Count() > 0;

    public string? Read { get; set; }

    public string Name { get; set; }

    public string Init { get; set; }

    public bool ServerFilter { get; set; }

    public List<OAHtmlGridButton> HeaderButtons { get; set; }

    public OAHtmlGrid()
    {
        HeaderButtons = new List<OAHtmlGridButton>();
    }
}
