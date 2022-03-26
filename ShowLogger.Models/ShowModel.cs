using System.ComponentModel.DataAnnotations;

namespace ShowLogger.Models;

public class ShowModel
{
    public int UserId { get; set; }

    public int ShowId { get; set; }

    [Display(Name = "Show Name")]
    public string ShowName { get; set; }

    [Display(Name = "Show Type")]
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

public class GroupedShowModel
{
    public int UserId { get; set; }

    public int ShowId { get; set; }

    [Display(Name = "Show Name")]
    public string ShowName { get; set; }

    [Display(Name = "Episodes Watched")]
    public int EpisodesWatched { get; set; }

    [Display(Name = "First Watched")]
    public DateTime FirstWatched { get; set; }

    [Display(Name = "Last Watched")]
    public DateTime LastWatched { get; set; }

    [Display(Name = "Episodes Per Day")]
    public decimal EpisodesPerDay => EpisodesWatched / (decimal)DaysSinceStarting;

    [Display(Name = "Days Since Starting")]
    public int DaysSinceStarting => Math.Max(Convert.ToInt32((LastWatched.Date - FirstWatched.Date).TotalDays), 1);

    public string MobileView => $"{ShowName}<br>{FirstWatched.ToShortDateString()} - {LastWatched.ToShortDateString()}<br>{EpisodesWatched} total - {EpisodesPerDay}/day - {DaysSinceStarting} days";
}

public class MovieModel
{
    public int UserId { get; set; }

    [Display(Name = "Movie Name")]
    public string MovieName { get; set; }

    [Display(Name = "Date Watched")]
    public DateTime DateWatched { get; set; }

    public string MobileView => $"{MovieName} - {DateWatched.ToShortDateString()}";

}

public class ShowNameModel
{
    public string ShowName { get; set; }

    public int? SeasonNumber { get; set; }

    public int? EpisodeNumber { get; set; }

    public int ShowTypeId { get; set; }
}

public static class DateTimeExtensions
{
    public static DateTime GetEST(this DateTime date)
    {
        var timeUtc = DateTime.UtcNow;
        TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

        return easternTime;
    }
}
