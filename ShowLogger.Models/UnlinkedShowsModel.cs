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

    [Display(Name = "Last Watched")]
    public DateTime LastWatched { get; set; }

    [Display(Name = "Watch Count")]
    public int WatchCount { get; set; }

    [Display(Name = "Air Date")]
    public DateTime? AirDate { get; set; }

    [Display(Name = "Last Data Refresh")]
    public DateTime? LastDataRefresh { get; set; }

    [Display(Name = "Info Id")]
    public int InfoId { get; set; }

    [Display(Name = "In Show Logger?")]
    public bool InShowLoggerIndc { get; set; }

    public string MobileView =>
        $"{ShowName}<br>" +
        $"{ShowTypeIdZ}<br>" +
        $"{LastWatched.ToShortDateString()}<br>" +
        $"{WatchCount}<br>" +
        (AirDate != null ? $"{AirDate:MM/dd/yyyy}<br>": "") +
        $"{InShowLoggerIndc}<br>" +
        (LastDataRefresh != null ? $"{LastDataRefresh:MM/dd/yyyy}<br>": "") +
        $"";
}
