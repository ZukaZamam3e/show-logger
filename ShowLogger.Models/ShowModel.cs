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

    [Display(Name = "Watch Date")]
    public DateTime DateWatched { get; set; }

    [Display(Name = "Show Notes")]
    public string? ShowNotes { get; set; }

    public virtual string MobileView => $"{ShowName}<br>{ShowTypeIdZ}{(SeasonNumber != null ? $" - s{SeasonNumber.Value.ToString().PadLeft(2, '0')}e{EpisodeNumber.Value.ToString().PadLeft(2, '0')}" :"")}<br>{DateWatched.ToString("MM/dd/yyyy")}<br>{ShowNotes}";
}

public class GroupedShowModel
{
    public int UserId { get; set; }

    public int ShowId { get; set; }

    [Display(Name = "Show Name")]
    public string ShowName { get; set; }

    [Display(Name = "Episodes Watched")]
    public int EpisodesWatched { get; set; }

    [Display(Name = "Lastest Season Watched")]
    public int? SeasonNumber { get; set; }

    [Display(Name = "Lastest Episode Watched")]
    public int? EpisodeNumber { get; set; }

    [Display(Name = "Latest Watched")]
    public string LatestWatched => SeasonNumber != null && EpisodeNumber != null ? $"s{SeasonNumber.Value.ToString().PadLeft(2, '0')}e{EpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "";

    [Display(Name = "First Watched")]
    public DateTime FirstWatched { get; set; }

    [Display(Name = "Last Watched")]
    public DateTime LastWatched { get; set; }

    [Display(Name = "Episodes Per Day")]
    public decimal EpisodesPerDay => Math.Max(Math.Round(EpisodesWatched / (decimal)DaysSinceStarting, 2), 1);

    [Display(Name = "Days Since Starting")]
    public int DaysSinceStarting => Math.Max(Convert.ToInt32((LastWatched.Date - FirstWatched.Date).TotalDays), 1);

    public string MobileView => $"{ShowName}<br>{FirstWatched.ToShortDateString()} - {LastWatched.ToShortDateString()}<br>{(!string.IsNullOrEmpty(LatestWatched) ? $"{LatestWatched}<br>" : "")}{EpisodesWatched} total - {DaysSinceStarting} days";
}

public class MovieModel
{
    public int UserId { get; set; }

    [Display(Name = "Movie Name")]
    public string MovieName { get; set; }

    [Display(Name = "Show Type")]
    public string? ShowTypeIdZ { get; set; }

    [Display(Name = "Date Watched")]
    public DateTime DateWatched { get; set; }

    public string MobileView => $"{MovieName}<br>{ShowTypeIdZ}<br>{DateWatched.ToShortDateString()}";

}

public class AddRangeModel 
{
    [Display(Name = "Show Name")]
    public string AddRangeShowName { get; set; }

    [Display(Name = "Season")]
    public int? AddRangeSeasonNumber { get; set; }

    [Display(Name = "Start Episode")]
    public int AddRangeStartEpisode { get; set; }

    [Display(Name = "End Episode")]
    public int? AddRangeEndEpisode { get; set; }
}

public class ShowNameModel
{
    public string ShowName { get; set; }

    public int? SeasonNumber { get; set; }

    public int? EpisodeNumber { get; set; }

    public int ShowTypeId { get; set; }
}

public class FriendWatchHistoryModel : ShowModel
{
    [Display(Name = "Email")]
    public string Email { get; set; }

    public override string MobileView => $"{Email}<br>{ShowName}<br>{ShowTypeIdZ}{(SeasonNumber != null ? $" - s{SeasonNumber.Value.ToString().PadLeft(2, '0')}e{EpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "")}<br>{DateWatched.ToString("MM/dd/yyyy")}<br>{ShowNotes}";
}

public class WatchlistModel
{
    public int UserId { get; set; }

    public int WatchlistId { get; set; }

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

    [Display(Name = "Date Added")]
    public DateTime DateAdded { get; set; }

    [Display(Name = "Show Notes")]
    public string? ShowNotes { get; set; }

    public virtual string MobileView => $"{ShowName}<br>{ShowTypeIdZ}{(SeasonNumber != null ? $" - s{SeasonNumber.Value.ToString().PadLeft(2, '0')}e{EpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "")}<br>{DateAdded.ToString("MM/dd/yyyy")}<br>{ShowNotes}";
}

public class TransactionModel
{
    public int UserId { get; set; }
    public int TransactionId { get; set; }

    [Display(Name = "Transaction Type")]
    public int TransactionTypeId { get; set; }

    [Display(Name = "Transaction Type")]
    public string? TransactionTypeIdZ { get; set; }

    [Display(Name = "Movie")]
    public int? ShowId { get; set; }

    [Display(Name = "Movie")]
    public string? ShowIdZ { get; set; }

    [Display(Name = "Item")]
    public string Item { get; set; }

    [Display(Name = "Cost")]
    public decimal CostAmt { get; set; }

    [Display(Name = "Discount")]
    public decimal? DiscountAmt { get; set; }

    [Display(Name = "Transaction Notes")]
    public string? TransactionNotes { get; set; }

    [Display(Name = "Transaction Date")]
    public DateTime TransactionDate { get; set; }

    public virtual string MobileView => $"{TransactionTypeIdZ}{(ShowId != null ? $"<br>{ShowIdZ}" : "")}<br>{Item}<br>{string.Format("{0:C}", CostAmt)}{(DiscountAmt != null ? $"<br>{string.Format("{0:C}", DiscountAmt)}" : "")}{(!string.IsNullOrEmpty(TransactionNotes) ? $"<br>{TransactionNotes}" : "")}<br>{TransactionDate.ToString("MM/dd/yyyy")}";
}

public class ItemModel
{
    public int TransactionTypeId { get; set; }

    public string Item { get; set; }

    public decimal CostAmt { get; set; }
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
