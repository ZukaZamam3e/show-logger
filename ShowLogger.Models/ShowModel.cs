using System.ComponentModel.DataAnnotations;

namespace ShowLogger.Models;

public class ShowModel
{
    public int UserId { get; set; }

    public int ShowId { get; set; }

    [Display(Name = "Show Name")]
    public string ShowName { get; set; }

    public int ShowTypeId { get; set; }

    [Display(Name = "Show Type")]
    public string? ShowTypeIdZ { get; set; }

    [Display(Name = "Season")]
    public int? SeasonNumber { get; set; }

    [Display(Name = "Episode")]
    public int? EpisodeNumber { get; set; }

    [Display(Name = "Date Watch")]
    public DateTime DateWatched { get; set; }

    public string MobileView => $"{ShowName}<br>{ShowTypeIdZ}{(SeasonNumber != null ? $" - s{SeasonNumber.ToString().PadLeft(2, '0')}e{EpisodeNumber.ToString().PadLeft(2, '0')}" :"")}<br>{DateWatched.ToString("MM/dd/yyyy")}";
}

public class ShowNameModel
{
    public string ShowName { get; set; }

    public int? SeasonNumber { get; set; }

    public int? EpisodeNumber { get; set; }

    public int ShowTypeId { get; set; }
}
