using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ShowLogger.Models;
public class UnlinkedShowsModel
{
    [Display(Name = "Show Name")]
    public string ShowName { get; set; }

    [Display(Name = "Show Type")]
    public int ShowTypeId { get; set; }

    [Display(Name = "Show Type")]
    public string? ShowTypeIdZ { get; set; }

    [Display(Name = "Watch Count")]
    public int WatchCount { get; set; }

    [Display(Name = "Info Id")]
    public int InfoId { get; set; }

    [Display(Name = "In Show Logger?")]
    public bool InShowLoggerIndc { get; set; }
}
