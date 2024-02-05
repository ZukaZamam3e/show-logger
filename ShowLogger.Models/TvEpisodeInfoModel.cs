using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Models;
public class TvEpisodeInfoModel
{
    [Display(Name = "Tv Episode Info Id")]
    public int TvEpisodeInfoId { get; set; }

    [Display(Name = "Tv Info Id")]
    public int TvInfoId { get; set; }

    [Display(Name = "API Type")]
    public int? ApiType { get; set; }

    [Display(Name = "API Id")]
    public string? ApiId { get; set; }

    [Display(Name = "Season Name")]
    public string? SeasonName { get; set; }

    [Display(Name = "Episode Name")]
    public string? EpisodeName { get; set; }

    [Display(Name = "Season Number")]
    public int? SeasonNumber { get; set; }

    [Display(Name = "Episode Number")]
    public int? EpisodeNumber { get; set; }

    [Display(Name = "Season Episode")]
    public string SeasonEpisode => SeasonNumber != null && EpisodeNumber != null ? $"s{SeasonNumber.Value.ToString().PadLeft(2, '0')}e{EpisodeNumber.Value.ToString().PadLeft(2, '0')}" : "";

    [Display(Name = "Episode Overview")]
    public string EpisodeOverview { get; set; }

    [Display(Name = "Runtime")]
    public int? Runtime { get; set; }

    [Display(Name = "Air Date")]
    public DateTime? AirDate { get; set; }

    [Display(Name = "Image URL")]
    public string ImageUrl { get; set; }

    [Display(Name = "Overall Episode Number")]
    public int OverallEpisodeNumber { get; set; }
}
