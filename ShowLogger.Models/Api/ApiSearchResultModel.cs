using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShowLogger.Models.Api;
public class ApiSearchResultModel : IComparable<ApiSearchResultModel>
{
    [Display(Name = "API")]
    public INFO_API Api { get; set; }

    [Display(Name = "Type")]
    public INFO_TYPE Type { get; set; }

    [Display(Name = "Id")]
    public string Id { get; set; }

    [Display(Name = "Name")]
    public string Name { get; set; }

    [Display(Name = "Air Date")]
    public DateTime? AirDate { get; set; }

    [Display(Name = "Air Year")]
    public string AirYear { get; set; }

    public string AirDateZ => AirDate.HasValue ? AirDate.Value.ToShortDateString() : !string.IsNullOrEmpty(AirYear) ? AirYear : "No Air Date Available";

    [Display(Name = "Link")]
    public string Link { get; set; }

    [Display(Name = "Image URL")]
    public string ImageUrl { get; set; }

    public int CompareTo(ApiSearchResultModel? other)
    {
        return 1;
    }

    //public string Link => Api switch
    //{
    //    INFO_API.TMDB_API => $"",
    //    INFO_API.OMDB_API => $"",
    //}
}
