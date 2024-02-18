using System.ComponentModel.DataAnnotations;

namespace ShowLogger.Models;
public class TvInfoModel
{
    [Display(Name = "Tv Info Id")]
    public int TvInfoId { get; set; }

    [Display(Name = "Show Name")]
    public string ShowName { get; set; }

    [Display(Name = "Show Overview")]
    public string ShowOverview { get; set; }

    [Display(Name = "API Type")]
    public int? ApiType { get; set; }

    [Display(Name = "API Id")]
    public string? ApiId { get; set; }

    [Display(Name = "Other Names")]
    public string? OtherNames { get; set; }

    [Display(Name = "Last Data Refresh")]
    public DateTime LastDataRefresh { get; set; }

    [Display(Name = "Last Updated")]
    public DateTime LastUpdated { get; set; }

    [Display(Name = "Image")]
    public string ImageUrl { get; set; }

    public IEnumerable<TvInfoSeasonModel> Seasons { get; set; }

    [Display(Name = "Episodes")]
    public IEnumerable<TvEpisodeInfoModel> Episodes { get; set; }
}

