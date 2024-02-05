using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Models.Api;
public class ApisConfig
{
    public string TMDbURL { get; set; }
    public string TMDbAPIKey { get; set; }

    public string OMDbURL { get; set; }
    public string OMDbAPIKey { get; set; }
}

public static class TMDBApiPaths
{
    public const string TV = "tv/";
    public const string Movie = "movie/";
    public const string Image = "t/p/original/";
}

public static class OMDBApiPaths
{
    public const string Title = "title/";
}
