using OMDB_API_Wrapper;
using OMDB_API_Wrapper.Models.API_Requests;
using OMDB_API_Wrapper.Models;
using ShowLogger.Data.Context;
using ShowLogger.Data.Entities;
using ShowLogger.Models;
using ShowLogger.Models.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using OMDB_API_Wrapper.Models.API_Responses;

namespace ShowLogger.Store.Repositories.Interfaces;
public class InfoRepository : IInfoRepository
{
    private readonly ShowLoggerDbContext _context;
    private readonly ApisConfig _apisConfig;

    public InfoRepository(
        ShowLoggerDbContext context,
        ApisConfig apisConfig
        )
    {
        _context = context;
        _apisConfig = apisConfig;
    }

    public async Task<DownloadResultModel> Download(int userId, InfoApiDownloadModel downloadInfo)
    {
        DownloadResultModel result = downloadInfo.API switch
        {
            INFO_API.TMDB_API => await DownloadFromTMDb(downloadInfo),
            INFO_API.OMDB_API => await DownloadFromOMDb(downloadInfo)
        };

        return result;
    }

    public async Task<ApiResultModel<IEnumerable<ApiSearchResultModel>>> Search(int userId, InfoApiSearchModel searchInfo)
    {
        ApiResultModel<IEnumerable<ApiSearchResultModel>> result = searchInfo.API switch
        {
            INFO_API.TMDB_API => await SearchFromTMDb(searchInfo),
            INFO_API.OMDB_API => await SearchFromOMDb(searchInfo),
            _ => throw new NotImplementedException()
        };

        return result;
    }

    public async Task<ApiResultModel<IEnumerable<ApiSearchResultModel>>> SearchFromTMDb(InfoApiSearchModel searchInfo)
    {
        ApiResultModel<IEnumerable<ApiSearchResultModel>> model = new ApiResultModel<IEnumerable<ApiSearchResultModel>>
        {
            ApiResultContents = new List<ApiSearchResultModel>(),
            Result = ApiResults.Success
        };

        IEnumerable<ApiSearchResultModel> query = new List<ApiSearchResultModel>();   

        if(string.IsNullOrEmpty(searchInfo.Name))
        {
            model.Result = ApiResults.SearchNameMissing;
            return model;
        }
        else if(searchInfo.Name.Length < 3)
        {
            model.Result = ApiResults.SearchNameTooShort;
            return model;
        }


        if (!string.IsNullOrEmpty(_apisConfig.TMDbAPIKey))
        {
            TMDbClient client = new TMDbClient(_apisConfig.TMDbAPIKey);

            switch (searchInfo.Type)
            {
                case INFO_TYPE.TV:
                    {
                        SearchContainer<SearchTv> searchContainer = await client.SearchTvShowAsync(searchInfo.Name);

                        model.ApiResultContents = searchContainer.Results.Select(m => new ApiSearchResultModel
                        {
                            Api = INFO_API.TMDB_API,
                            Type = INFO_TYPE.TV,
                            Id = m.Id.ToString(),
                            Name = m.Name,
                            AirDate = m.FirstAirDate,
                            Link = $"{_apisConfig.TMDbURL}{TMDBApiPaths.TV}{m.Id}",
                            ImageUrl = !string.IsNullOrEmpty(m.PosterPath) ? $"{_apisConfig.TMDbURL}{TMDBApiPaths.Image}{m.PosterPath}" : ""
                        });

                        break;
                    }

                case INFO_TYPE.MOVIE:
                    {
                        SearchContainer<SearchMovie> searchContainer = await client.SearchMovieAsync(searchInfo.Name);

                        model.ApiResultContents = searchContainer.Results.Select(m => new ApiSearchResultModel
                        {
                            Api = INFO_API.TMDB_API,
                            Type = INFO_TYPE.MOVIE,
                            Id = m.Id.ToString(),
                            Name = m.Title,
                            AirDate = m.ReleaseDate,
                            Link = $"{_apisConfig.TMDbURL}{TMDBApiPaths.Movie}{m.Id}",
                            ImageUrl = !string.IsNullOrEmpty(m.PosterPath) ? $"{_apisConfig.TMDbURL}{TMDBApiPaths.Image}{m.PosterPath}" : ""
                        });

                        break;
                    }
            }
        }
        else
        {
            model.Result = ApiResults.TMDBApiKeyMissing;
        }

        return model;
    }

    private async Task<ApiResultModel<IEnumerable<ApiSearchResultModel>>> SearchFromOMDb(InfoApiSearchModel searchInfo)
    {
        ApiResultModel<IEnumerable<ApiSearchResultModel>> model = new ApiResultModel<IEnumerable<ApiSearchResultModel>>
        {
            ApiResultContents = new List<ApiSearchResultModel>(),
            Result = ApiResults.Success
        };

        if (!string.IsNullOrEmpty(_apisConfig.OMDbAPIKey))
        {
            OmdbClient client = new OmdbClient(_apisConfig.OMDbAPIKey);

            switch (searchInfo.Type)
            {
                case INFO_TYPE.TV:
                    {
                        BySearchRequest request = new BySearchRequest(searchInfo.Name, VideoType.Series);
                        BySearchResponse response = await client.BySearchRequestAsync(request);

                        model.ApiResultContents = response.SearchResults.Select(m => new ApiSearchResultModel
                        {
                            Api = INFO_API.OMDB_API,
                            Type = INFO_TYPE.TV,
                            Id = m.IMDB_ID,
                            Name = m.Title,
                            AirYear = m.Year,
                            Link = $"{_apisConfig.OMDbURL}{OMDBApiPaths.Title}{m.IMDB_ID}",
                            ImageUrl = m.Poster_URI
                        });

                        break;
                    }

                case INFO_TYPE.MOVIE:
                    {
                        BySearchRequest request = new BySearchRequest(searchInfo.Name, VideoType.Movie);
                        BySearchResponse response = await client.BySearchRequestAsync(request);

                        model.ApiResultContents = response.SearchResults.Select(m => new ApiSearchResultModel
                        {
                            Api = INFO_API.OMDB_API,
                            Type = INFO_TYPE.TV,
                            Id = m.IMDB_ID,
                            Name = m.Title,
                            AirYear = m.Year,
                            Link = $"{_apisConfig.OMDbURL}{OMDBApiPaths.Title}{m.IMDB_ID}",
                            ImageUrl = m.Poster_URI
                        });

                        break;
                    }
            }
        }
        else
        {
            model.Result = ApiResults.OMDBApiKeyMissing;
        }

        return model;
    }

    private async Task<DownloadResultModel> DownloadFromTMDb(InfoApiDownloadModel downloadInfo)
    {
        DownloadResultModel downloadResult = new DownloadResultModel
        {
            Result = ApiResults.Success,
            API = INFO_API.TMDB_API,
            Type = downloadInfo.Type,
            Id = -1
        };

        if (!string.IsNullOrEmpty(_apisConfig.TMDbAPIKey))
        {
            TMDbClient client = new TMDbClient(_apisConfig.TMDbAPIKey);

            switch (downloadInfo.Type)
            {
                case INFO_TYPE.TV:
                    {
                        TvShow show = await client.GetTvShowAsync(int.Parse(downloadInfo.Id));
                        TvInfoModel info = new TvInfoModel();

                        info.ShowName = show.Name;
                        info.ShowOverview = show.Overview;
                        info.ApiType = (int)INFO_API.TMDB_API;
                        info.ApiId = show.Id.ToString();
                        info.ImageUrl = show.PosterPath;

                        List<TvEpisodeInfoModel> episodes = new List<TvEpisodeInfoModel>();
                        
                        int requestCount = 1;
                        Stopwatch requestTimer = Stopwatch.StartNew();

                        foreach (var season in show.Seasons)
                        {
                            requestCount++;
                            TvSeason? seasonData = await client.GetTvSeasonAsync(show.Id, season.SeasonNumber);

                            if (seasonData != null)
                            {
                                episodes.AddRange(seasonData.Episodes.Select(m => new TvEpisodeInfoModel
                                {
                                    ApiType = (int)INFO_API.TMDB_API,
                                    ApiId = m.Id.ToString(),
                                    SeasonName = season.Name,
                                    EpisodeName = m.Name,
                                    SeasonNumber = season.SeasonNumber,
                                    EpisodeNumber = m.EpisodeNumber,
                                    EpisodeOverview = m.Overview,
                                    Runtime = m.Runtime,
                                    AirDate = m.AirDate,
                                    ImageUrl = m.StillPath
                                }));
                            }

                            if (requestCount >= 30)
                            {
                                if (requestTimer.Elapsed.Seconds < 1)
                                {
                                    Thread.Sleep(1 - (int)requestTimer.ElapsedMilliseconds);
                                }

                                requestCount = 0;
                                requestTimer.Restart();
                            }
                        }

                        info.Episodes = episodes;

                        downloadResult.Id = RefreshTvInfo(info);
                        break;

                    }

                case INFO_TYPE.MOVIE:
                    {
                        Movie movie = await client.GetMovieAsync(int.Parse(downloadInfo.Id));
                        MovieInfoModel info = new MovieInfoModel
                        {
                            MovieName = movie.Title,
                            MovieOverview = movie.Overview,
                            ApiType = (int)INFO_API.TMDB_API,
                            ApiId = movie.Id.ToString(),
                            Runtime = movie.Runtime,
                            AirDate = movie.ReleaseDate,
                            ImageURL = movie.PosterPath
                        };

                        downloadResult.Id = RefreshMovieInfo(info);

                        break;
                    }
            }
        }
        else
        {
            downloadResult.Result = ApiResults.TMDBApiKeyMissing;
        }

        return downloadResult;
    }

    private async Task<DownloadResultModel> DownloadFromOMDb(InfoApiDownloadModel downloadInfo)
    {
        DownloadResultModel downloadResult = new DownloadResultModel
        {
            Result = ApiResults.Success,
            API = INFO_API.OMDB_API,
            Id = -1
        };

        if (!string.IsNullOrEmpty(_apisConfig.OMDbAPIKey))
        {
            OmdbClient client = new OmdbClient(_apisConfig.OMDbAPIKey);

            switch (downloadInfo.Type)
            {
                case INFO_TYPE.TV:
                    {
                        ByIDRequest request = new ByIDRequest(downloadInfo.Id, PlotSize.Full);
                        ByTitleResponse response = await client.ByIDRequestAsync(request);

                        TvInfoModel info = new TvInfoModel();

                        info.ShowName = response.Title;
                        info.ShowOverview = response.Plot;
                        info.ApiId = response.IMDB_ID;

                        //List<TvEpisodeInfoModel> episodes = new List<TvEpisodeInfoModel>();

                        //int requestCount = 1;
                        //Stopwatch requestTimer = Stopwatch.StartNew();

                        //foreach (var season in show.Seasons)
                        //{
                        //    requestCount++;
                        //    TvSeason? seasonData = await client.GetTvSeasonAsync(show.Id, season.SeasonNumber);

                        //    if (seasonData != null)
                        //    {
                        //        episodes.AddRange(seasonData.Episodes.Select(m => new TvEpisodeInfoModel
                        //        {
                        //            ApiType = (int)INFO_API.TMDB_API,
                        //            ApiId = m.Id.ToString(),
                        //            SeasonName = season.Name,
                        //            EpisodeName = m.Name,
                        //            SeasonNumber = season.SeasonNumber,
                        //            EpisodeNumber = m.EpisodeNumber,
                        //            EpisodeOverview = m.Overview,
                        //            Runtime = m.Runtime,
                        //            AirDate = m.AirDate,
                        //            ImageUrl = m.StillPath
                        //        }));
                        //    }

                        //    if (requestCount >= 30)
                        //    {
                        //        if (requestTimer.Elapsed.Seconds < 1)
                        //        {
                        //            Thread.Sleep(1 - (int)requestTimer.ElapsedMilliseconds);
                        //        }

                        //        requestCount = 0;
                        //        requestTimer.Restart();
                        //    }
                        //}

                        //info.Episodes = episodes;

                        //downloadResult.Id = RefreshTvInfo(info);
                        break;

                    }

                case INFO_TYPE.MOVIE:
                    {
                        //Movie movie = await client.GetMovieAsync(int.Parse(downloadInfo.Id));
                        //MovieInfoModel info = new MovieInfoModel();

                        //info.MovieName = movie.Title;
                        //info.MovieOverview = movie.Overview;
                        //info.TMDbId = movie.Id;
                        //info.Runtime = movie.Runtime;
                        //info.AirDate = movie.ReleaseDate;

                        //downloadResult.Id = RefreshMovieInfo(info);

                        break;
                    }
            }
        }
        else
        {
            downloadResult.Result = ApiResults.OMDBApiKeyMissing;
        }

        return downloadResult;
    }

    public IEnumerable<TvInfoModel> GetTvInfos(Expression<Func<TvInfoModel, bool>>? predicate)
    {
        IEnumerable<TvInfoModel> query = _context.SL_TV_INFO.Select(m => new TvInfoModel
        {
            TvInfoId = m.TV_INFO_ID,
            ShowName = m.SHOW_NAME,
            ShowOverview = m.SHOW_OVERVIEW,
            ApiType = (int)INFO_API.TMDB_API,
            ApiId = m.API_ID,
            OtherNames = m.OTHER_NAMES,
            LastDataRefresh = m.LAST_DATA_REFRESH,
            LastUpdated = m.LAST_UPDATED,
            ImageUrl = !string.IsNullOrEmpty(m.IMAGE_URL) ? $"{_apisConfig.TMDbURL}{TMDBApiPaths.Image}{m.IMAGE_URL}" : ""
        });

        if (predicate != null)
        {
            query = query.AsQueryable().Where(predicate);
        }

        return query;
    }

    public IEnumerable<TvEpisodeInfoModel> GetTvEpisodeInfos(Expression<Func<TvEpisodeInfoModel, bool>>? predicate)
    {
        IEnumerable<TvEpisodeInfoModel> query = _context.SL_TV_EPISODE_INFO
            .Select(m => new TvEpisodeInfoModel
            {
                TvEpisodeInfoId = m.TV_EPISODE_INFO_ID,
                TvInfoId = m.TV_INFO_ID,
                SeasonName = m.SEASON_NAME,
                EpisodeName = m.EPISODE_NAME,
                ApiType = (int)INFO_API.TMDB_API,
                ApiId = m.API_ID,
                SeasonNumber = m.SEASON_NUMBER,
                EpisodeNumber = m.EPISODE_NUMBER,
                EpisodeOverview = m.EPISODE_OVERVIEW,
                Runtime = m.RUNTIME,
                AirDate = m.AIR_DATE,
                ImageUrl = !string.IsNullOrEmpty(m.IMAGE_URL) ? $"{_apisConfig.TMDbURL}{TMDBApiPaths.Image}{m.IMAGE_URL}" : "",
            });

        if (predicate != null)
        {
            query = query.AsQueryable().Where(predicate);
        }

        return query;
    }

    public long RefreshTvInfo(TvInfoModel model)
    {
        //SL_TV_INFO? entity = _context.SL_TV_INFO.FirstOrDefault(m => m.TMDB_ID == model.TMDbId && m.OMDB_ID == model.OMDbId);
        SL_TV_INFO? entity = _context.SL_TV_INFO.FirstOrDefault(m => m.API_TYPE == model.ApiType && m.API_ID == model.ApiId);

        if (entity == null)
        {
            entity = new SL_TV_INFO
            {
                API_TYPE = model.ApiType,
                API_ID = model.ApiId,
            };

            _context.SL_TV_INFO.Add(entity);
        }

        entity.SHOW_NAME = model.ShowName;
        entity.SHOW_OVERVIEW = model.ShowOverview;
        entity.IMAGE_URL = model.ImageUrl;

        entity.LAST_DATA_REFRESH = DateTime.Now;
        entity.LAST_UPDATED = DateTime.Now;

        _context.SaveChanges();
        long id = entity.TV_INFO_ID;

        foreach (TvEpisodeInfoModel episode in model.Episodes)
        {
            //SL_TV_EPISODE_INFO? episodeEntity = _context.SL_TV_EPISODE_INFO.FirstOrDefault(m => m.TMDB_ID == episode.TMDbId && m.OMDB_ID == episode.OMDbId);
            SL_TV_EPISODE_INFO? episodeEntity = _context.SL_TV_EPISODE_INFO.FirstOrDefault(m => m.API_TYPE == model.ApiType && m.API_ID == model.ApiId);

            if (episodeEntity == null)
            {
                episodeEntity = new SL_TV_EPISODE_INFO
                {
                    TV_INFO_ID = entity.TV_INFO_ID,
                    API_TYPE = model.ApiType,
                    API_ID = entity.API_ID,
                };

                _context.SL_TV_EPISODE_INFO.Add(episodeEntity);
            }

            episodeEntity.SEASON_NUMBER = episode.SeasonNumber;
            episodeEntity.EPISODE_NUMBER = episode.EpisodeNumber;

            episodeEntity.EPISODE_NAME = episode.EpisodeName;
            episodeEntity.EPISODE_OVERVIEW = episode.EpisodeOverview;


            episodeEntity.SEASON_NAME = episode.SeasonName;
            episodeEntity.EPISODE_NAME = episode.EpisodeName;

            episodeEntity.RUNTIME = episode.Runtime;
            episodeEntity.AIR_DATE = episode.AirDate;
            episodeEntity.IMAGE_URL = episode.ImageUrl;
        }

        entity.LAST_DATA_REFRESH = DateTime.Now;
        entity.LAST_UPDATED = DateTime.Now;
        _context.SaveChanges();

        return id;
    }

    public long RefreshTvInfo(int infoId)
    {
        TvInfoModel model = GetTvInfos(m => m.TvInfoId == infoId).First();



        long result = RefreshTvInfo(model);

        return result;
    }


    public long UpdateTvInfoOtherNames(int userId, int tvInfoId, string otherNames)
    {
        throw new NotImplementedException();
    }

    public bool DeleteTvInfo(int userId, int tvInfoId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MovieInfoModel> GetMovieInfos(Expression<Func<MovieInfoModel, bool>>? predicate = null)
    {
        IEnumerable<MovieInfoModel> query = _context.SL_MOVIE_INFO.Select(m => new MovieInfoModel
        {
            MovieInfoId = m.MOVIE_INFO_ID,
            MovieName = m.MOVIE_NAME,
            MovieOverview = m.MOVIE_OVERVIEW,
            ApiType = m.API_TYPE,
            ApiId = m.API_ID,
            Runtime = m.RUNTIME,
            AirDate = m.AIR_DATE,
            OtherNames = m.OTHER_NAMES,
            ImageURL = m.IMAGE_URL,
            LastDataRefresh = m.LAST_DATA_REFRESH,
            LastUpdated = m.LAST_UPDATED,
        });

        if (predicate != null)
        {
            query = query.AsQueryable().Where(predicate);
        }

        return query;
    }

    public long RefreshMovieInfo(MovieInfoModel model)
    {
        //SL_MOVIE_INFO? entity = _context.SL_MOVIE_INFO.FirstOrDefault(m => m.TMDB_ID == model.TMDbId && m.OMDB_ID == model.OMDbId);
        SL_MOVIE_INFO? entity = _context.SL_MOVIE_INFO.FirstOrDefault(m => m.API_TYPE == model.ApiType && m.API_ID == model.ApiId);

        if (entity == null)
        {
            entity = new SL_MOVIE_INFO();
            _context.SL_MOVIE_INFO.Add(entity);
        }

        entity.API_TYPE = model.ApiType;
        entity.API_ID = model.ApiId;

        entity.MOVIE_NAME = model.MovieName;
        entity.MOVIE_OVERVIEW = model.MovieOverview;

        entity.RUNTIME = model.Runtime;
        entity.AIR_DATE = model.AirDate;

        entity.LAST_DATA_REFRESH = DateTime.Now;
        entity.LAST_UPDATED = DateTime.Now;

        _context.SaveChanges();
        long id = entity.MOVIE_INFO_ID;

        return id;
    }

    public long UpdateMovieInfoOtherNames(int userId, int movieInfoId, string otherNames)
    {
        throw new NotImplementedException();
    }

    public bool DeleteMovieInfo(int userId, int tvInfoId)
    {
        throw new NotImplementedException();
    }

    public void RefreshInfo(int infoId, INFO_TYPE type)
    {
        switch(type)
        {
            case INFO_TYPE.TV:
                {
                    TvEpisodeInfoModel episode = GetTvEpisodeInfos(m => m.TvEpisodeInfoId == infoId).First();
                    TvInfoModel model = GetTvInfos(m => m.TvInfoId == episode.TvInfoId).First();
                    RefreshTvInfo(model);
                    break;
                }

            case INFO_TYPE.MOVIE:
                {
                    MovieInfoModel model = GetMovieInfos(m => m.MovieInfoId == infoId).First();
                    RefreshMovieInfo(model);
                    break;
                }
        }
    }
    public IEnumerable<UnlinkedShowsModel> GetUnlinkedShows(Expression<Func<UnlinkedShowsModel, bool>>? predicate = null)
    {
        Dictionary<int, string> showTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.SHOW_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);
        SL_SHOW[] data = _context.SL_SHOW.Where(m => m.INFO_ID == null).ToArray();
        Dictionary<string, int> tvData = _context.SL_TV_INFO.ToDictionary(m => m.SHOW_NAME, m=> m.TV_INFO_ID);
        Dictionary<string, int> movieData = _context.SL_MOVIE_INFO.ToDictionary(m => m.MOVIE_NAME, m => m.MOVIE_INFO_ID);


        IEnumerable<UnlinkedShowsModel> query = (from d in data
                                                 group d by new { d.SHOW_NAME, d.SHOW_TYPE_ID } into grp
                                                 select new UnlinkedShowsModel
                                                 {
                                                     ShowName = grp.Key.SHOW_NAME,
                                                     ShowTypeId = grp.Key.SHOW_TYPE_ID,
                                                     ShowTypeIdZ = showTypeIds[grp.Key.SHOW_TYPE_ID],
                                                     WatchCount = grp.Count(),
                                                     LastWatched = grp.Max(m => m.DATE_WATCHED),
                                                     InfoId = grp.Key.SHOW_TYPE_ID == (int)CodeValueIds.TV ? tvData.ContainsKey(grp.Key.SHOW_NAME) ? tvData[grp.Key.SHOW_NAME] : -1 : movieData.ContainsKey(grp.Key.SHOW_NAME) ? movieData[grp.Key.SHOW_NAME] : -1,
                                                     InShowLoggerIndc = grp.Key.SHOW_TYPE_ID == (int)CodeValueIds.TV ? tvData.ContainsKey(grp.Key.SHOW_NAME) : movieData.ContainsKey(grp.Key.SHOW_NAME)
                                                 });

        if (predicate != null)
        {
            query = query.AsQueryable().Where(predicate);
        }

        return query;
    }

    

}
