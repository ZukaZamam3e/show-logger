﻿using ShowLogger.Models.Api;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

    [Display(Name = "Restart Binge")]
    public bool RestartBinge { get; set; }

    [Display(Name = "Info Id")]
    public int? InfoId { get; set; }

    [Display(Name = "Episode Name")]
    public string? EpisodeName { get; set; }

    [Display(Name = "Runtime")]
    public int? Runtime { get; set; }

    [Display(Name = "Runtime")]
    public string RuntimeZ => Runtime != null ? $"{Runtime} minutes" : "";

    public string? ImageUrl { get; set; }

    public string? InfoUrl { get; set; }

    [Display(Name = "Season Episode")]
    public string SeasonEpisode => SeasonNumber != null && EpisodeNumber != null ? $"s{SeasonNumber.Value.ToString().PadLeft(2, '0')}e{EpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "";

    [Display(Name = "Season Episode / Runtime")]
    public string SeasonEpisodeRuntime => $"{SeasonEpisode}{(Runtime != null ? $"{(ShowTypeId == 1000 ? " - " : "")}{RuntimeZ}" : "")}";

    //public virtual string MobileView => $"{ShowName}<br>" +
    //    $"{ShowTypeIdZ}{(ShowTypeId == 1000 ? $" - {SeasonEpisode}" : "")}<br>" +
    //    $"{DateWatched:MM/dd/yyyy}<br>" +
    //    $"{(!string.IsNullOrEmpty(EpisodeName) ? $"{$"<a href =\"{InfoUrl}\" target=\"blank\">{EpisodeName}</a>"}<br>" : "")}" +
    //    $"{(Runtime != null ? $"{RuntimeZ}<br>" : "")}" +
    //    $"{ShowNotes}";

    [Display(Name = "Show Name")]
    public string ShowNameZ => $"{(ShowTypeId == 1000 ? GetShowShowNameZ : GetMovieShowNameZ)}";

    private string GetShowShowNameZ => $"{ShowName}{$"{(!string.IsNullOrEmpty(EpisodeName) ? $"{$" - <a href =\"{InfoUrl}\" target=\"_blank\">{EpisodeName}</a>"}<br>" : "")}"}";
    private string GetMovieShowNameZ => $"{(InfoId != null ? $"<a href =\"{InfoUrl}\" target=\"_blank\">{ShowName}</a>" : ShowName)}";

    public virtual string MobileView => $"{(ShowTypeId == 1000 ? GetShowMobileView : GetMovieMobileView)}";

    private string GetShowMobileView => $"{ShowName}<br>" +
        $"{(!string.IsNullOrEmpty(EpisodeName) ? $"{$"<a href =\"{InfoUrl}\" target=\"_blank\">{EpisodeName}</a>"}<br>" : "")}" +
        $"{ShowTypeIdZ} - {SeasonEpisode}<br>" +
        $"{DateWatched:MM/dd/yyyy}<br>" +
        $"{(Runtime != null ? $"{RuntimeZ}<br>" : "")}" +
        $"{ShowNotes}";

    private string GetMovieMobileView => $"{(InfoId != null ? $"<a href =\"{InfoUrl}\" target=\"_blank\">{ShowName}</a>" : ShowName)}<br>" +
        $"{ShowTypeIdZ}<br>" +
        $"{DateWatched:MM/dd/yyyy}<br>" +
        $"{(Runtime != null ? $"{RuntimeZ}<br>" : "")}" +
        $"{ShowNotes}";


}

public class ShowInfoModel : ShowModel
{
    public int? InfoSeasonNumber { get; set; }
    public int? InfoEpisodeNumber { get; set; }
    public int? InfoApiType { get; set; }
    public string? InfoApiId { get; set; }
    public string? InfoImageUrl { get; set; }

}

public class GroupedShowModel
{
    public int UserId { get; set; }

    public int ShowId { get; set; }

    [Display(Name = "Show Name")]
    public string ShowName { get; set; }

    [Display(Name = "Episodes Watched")]
    public int EpisodesWatched { get; set; }

    [Display(Name = "First Season Watched")]
    public int? StartingSeasonNumber { get; set; }

    [Display(Name = "First Episode Watched")]
    public int? StartingEpisodeNumber { get; set; }

    [Display(Name = "First Watched")]
    public string StartingWatched => StartingSeasonNumber != null && StartingEpisodeNumber != null ? $"s{StartingSeasonNumber.Value.ToString().PadLeft(2, '0')}e{StartingEpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "";

    [Display(Name = "Lastest Season Watched")]
    public int? LatestSeasonNumber { get; set; }

    [Display(Name = "Lastest Episode Watched")]
    public int? LatestEpisodeNumber { get; set; }

    [Display(Name = "Latest Watched")]
    public string LatestWatched => LatestSeasonNumber != null && LatestEpisodeNumber != null ? $"s{LatestSeasonNumber.Value.ToString().PadLeft(2, '0')}e{LatestEpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "";

    [Display(Name = "Watched")]
    public string Watched => $"{StartingWatched} - {LatestWatched}";

    [Display(Name = "First Watched")]
    public DateTime FirstWatched { get; set; }

    [Display(Name = "Last Watched")]
    public DateTime LastWatched { get; set; }

    [Display(Name = "Episodes Per Day")]
    public decimal EpisodesPerDay => Math.Max(Math.Round(EpisodesWatched / (decimal)DaysSinceStarting, 2), 1);

    [Display(Name = "Days Since Starting")]
    public int DaysSinceStarting => Math.Max(Convert.ToInt32((LastWatched.Date - FirstWatched.Date.AddDays(-1)).TotalDays), 1);

    public int? InfoId { get; set; }

    public int? NextEpisodeInfoId { get; set; }

    public string? NextEpisodeName { get; set; }

    public int? NextSeasonNumber { get; set; }

    public int? NextEpisodeNumber { get; set; }

    public string? NextInfoUrl { get; set; }

    public DateTime? NextAirDate { get; set; }

    public int? EpisodesLeft { get; set; }

    public string NextWatched => NextSeasonNumber != null && NextSeasonNumber != null ? $"s{NextSeasonNumber.Value.ToString().PadLeft(2, '0')}e{NextEpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "";

    [Display(Name = "Next Episode")]
    public string NextEpisodeInfo =>
        $"{(!string.IsNullOrEmpty(NextEpisodeName) ? $"{$"<a href =\"{NextInfoUrl}\" target=\"_blank\">{NextEpisodeName}</a>"}<br>" : "")}" +
        $"{(!string.IsNullOrEmpty(NextWatched) ? $"{NextWatched}{(NextAirDate != null && NextAirDate > DateTime.Now.Date ? $" - {NextAirDate.Value.ToShortDateString()}" : "")}<br>" : "")}" +
        $"{(EpisodesLeft != null && EpisodesLeft > 0 ? $"{EpisodesLeft} episode{(EpisodesLeft > 1 ? "s" : "")} left" : "")}";

    public string MobileView => $"{ShowName}" +
        $"<br>{FirstWatched.ToShortDateString()} - {LastWatched.ToShortDateString()}" +
        $"<br>{(!string.IsNullOrEmpty(Watched) ? $"{Watched}<br>" : "")}" +
        $"{EpisodesWatched} total - {DaysSinceStarting} days" +
        $"{(NextEpisodeInfoId != null ? $"<br/>{NextEpisodeInfo}" : "")}";
}

public class MovieModel
{
    public int UserId { get; set; }

    [Display(Name = "Movie Name")]
    public string MovieName { get; set; }

    [Display(Name = "Show Id")]
    public int ShowId { get; set; }

    [Display(Name = "Show Type")]
    public int ShowTypeId { get; set; }

    [Display(Name = "Show Type")]
    public string? ShowTypeIdZ { get; set; }

    [Display(Name = "Date Watched")]
    public DateTime DateWatched { get; set; }

    [Display(Name = "A-List")]
    public decimal? AlistTicketAmt { get; set; }

    [Display(Name = "Ticket")]
    public decimal? TicketAmt { get; set; }

    [Display(Name = "Purchase")]
    public decimal? PurchaseAmt { get; set; }

    public string MobileView => $"{MovieName}<br>{ShowTypeIdZ}<br>{DateWatched.ToShortDateString()}{(ShowTypeId == 1002 ? $"<br>A-List: {string.Format("{0:C}", AlistTicketAmt)}<br>Ticket: {string.Format("{0:C}", TicketAmt)}<br>Purchase: {string.Format("{0:C}", PurchaseAmt)}" : "")}";

}

public class AddRangeModel 
{
    public AddRangeModel()
    {
        AddRangeDateWatched = DateTime.Now.Date;
    }

    [Display(Name = "Show Name")]
    public string AddRangeShowName { get; set; }

    [Display(Name = "Season")]
    public int? AddRangeSeasonNumber { get; set; }

    [Display(Name = "Start")]
    public int AddRangeStartEpisode { get; set; }

    [Display(Name = "End")]
    public int? AddRangeEndEpisode { get; set; }

    [Display(Name = "Date")]
    public DateTime AddRangeDateWatched { get; set; }
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
    [Display(Name = "Name")]
    public string Name { get; set; }

    //public override string MobileView => $"{Name}<br>{ShowName}<br>{ShowTypeIdZ}{(SeasonNumber != null ? $" - s{SeasonNumber.Value.ToString().PadLeft(2, '0')}e{EpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "")}<br>{DateWatched.ToString("MM/dd/yyyy")}<br>{ShowNotes}";

    public override string MobileView => $"{Name}<br>{base.MobileView}";

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

    [Display(Name = "Benefits")]
    public decimal? BenefitAmt { get; set; }

    [Display(Name = "Discount")]
    public decimal? DiscountAmtZ => (DiscountAmt ?? 0) + (BenefitAmt ?? 0);

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

public class YearStatsModel
{
    [Display(Name = "User Id")]
    public int UserId { get; set; }

    [Display(Name = "Name")]
    public string Name { get; set; }

    [Display(Name = "Year")]
    public int Year { get; set; }

    [Display(Name = "TV")]
    public int TvCnt { get; set; }

    [Display(Name = "TV (Not Tracked)")]
    public int TvNotTrackedCnt { get; set; }

    [Display(Name = "TV Runtime")]
    public int? TvRuntime { get; set; }

    [Display(Name = "TV Runtime")]
    public string TvRuntimeZ => ConvertRuntime(TvRuntime);

    [Display(Name = "TV Stats")]
    public string TvStats => $"{TvCnt} ({TvNotTrackedCnt}){TvRuntimeZ}";

    [Display(Name = "Movies")]
    public int MoviesCnt { get; set; }

    [Display(Name = "Movies (Not Tracked)")]
    public int MoviesNotTrackedCnt { get; set; }

    [Display(Name = "Movies Runtime")]
    public int? MoviesRuntime { get; set; }

    [Display(Name = "Movies Runtime")]
    public string MoviesRuntimeZ => ConvertRuntime(MoviesRuntime);

    [Display(Name = "Movies Stats")]
    public string MovieStats => $"{MoviesCnt} ({MoviesNotTrackedCnt}){MoviesRuntimeZ}";

    [Display(Name = "AMC")]
    public int AmcCnt { get; set; }

    [Display(Name = "AMC (Not Tracked)")]
    public int AmcNotTrackedCnt { get; set; }

    [Display(Name = "AMC Runtime")]
    public int? AmcRuntime { get; set; }

    [Display(Name = "AMC Runtime")]
    public string AmcRuntimeZ => ConvertRuntime(AmcRuntime);

    [Display(Name = "AMC Stats")]
    public string AmcStats => $"{AmcCnt} ({AmcNotTrackedCnt}){AmcRuntimeZ}";

    [Display(Name = "A-List Membership")]
    public decimal AListMembership { get; set; }

    [Display(Name = "A-List Tickets")]
    public decimal AListTickets { get; set; }

    [Display(Name = "AMC Purchases")]
    public decimal AmcPurchases { get; set; }

    public virtual string MobileView => $"{Year}<br>{Name}<br>TV: {TvStats}<br>Movies: {MovieStats}<br>AMC: {AmcStats}" +
        $"<br>A-List Membership: {string.Format("{0:C}", AListMembership)}<br>A-List Tickets: {string.Format("{0:C}", AListTickets)}" +
        $"<br>AMC Purchases: {string.Format("{0:C}", AmcPurchases)}";

    private string ConvertRuntime(int? minutes)
    {
        if (minutes == null)
            return "";

        TimeSpan timespan = TimeSpan.FromMinutes(minutes.Value);

        StringBuilder result = new StringBuilder("<br>");

        if(timespan.Days > 0)
        {
            result.Append($"{timespan.Days} day");

            if(timespan.Days > 1)
            {
                result.Append("s");
            }

            result.Append(" ");
        }

        if (timespan.Hours > 0)
        {
            result.Append($"{timespan.Hours} hour");

            if (timespan.Hours > 1)
            {
                result.Append("s");
            }

            result.Append(" ");
        }

        if (timespan.Minutes > 0)
        {
            result.Append($"{timespan.Minutes} minute");

            if (timespan.Minutes > 1)
            {
                result.Append("s");
            }

            result.Append(" ");
        }

        return result.ToString();
    }
}

public class LoadWatchFromSearchModel
{
    public INFO_API API { get; set; }

    public INFO_TYPE Type { get; set; }

    public string Id { get; set; }

    public string Name { get; set; }

    public string AirDate { get; set; }
}

public class WatchFromSearchModel : ShowModel
{
    public INFO_API API { get; set; }

    public INFO_TYPE Type { get; set; }

    public string Id { get; set; }
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
