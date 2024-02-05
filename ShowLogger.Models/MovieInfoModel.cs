using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Models;
public class MovieInfoModel
{
    [Display(Name = "Movie Info Id")]
    public int MovieInfoId { get; set; }

    [Display(Name = "Movie Name")]
    public string MovieName { get; set; }

    [Display(Name = "Movie Overview")]
    public string? MovieOverview { get; set; }

    [Display(Name = "API Type")]
    public int? ApiType { get; set; }

    [Display(Name = "API Id")]
    public string? ApiId { get; set; }

    [Display(Name = "Runtime")]
    public int? Runtime { get; set; }

    [Display(Name = "Air Date")]
    public DateTime? AirDate { get; set; }

    [Display(Name = "Other Names")]
    public string? OtherNames { get; set; }

    [Display(Name = "Last Data Refresh")]
    public DateTime LastDataRefresh { get; set; }

    [Display(Name = "Last Updated")]
    public DateTime LastUpdated { get; set; }

    [Display(Name = "Image URL")]
    public string? ImageURL { get; set; }

}
