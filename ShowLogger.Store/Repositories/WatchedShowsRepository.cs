using ShowLogger.Data.Context;
using ShowLogger.Data.Entities;
using ShowLogger.Models;
using ShowLogger.Models.Api;
using ShowLogger.Store.Repositories.Interfaces;
using System;
using System.Linq.Expressions;
using TMDbLib.Objects.TvShows;

namespace ShowLogger.Store.Repositories;

public class WatchedShowsRepository : IWatchedShowsRepository
{
    private readonly ShowLoggerDbContext _context;
    private readonly ApisConfig _apisConfig;

    public WatchedShowsRepository(ShowLoggerDbContext context, ApisConfig apisConfig)
    {
        _context = context;
        _apisConfig = apisConfig;
    }

    public IEnumerable<CodeValueModel> GetCodeValues(Expression<Func<CodeValueModel, bool>>? predicate = null)
    {
        IEnumerable<CodeValueModel> query = _context.SL_CODE_VALUE.Select(m => new CodeValueModel
        {
            CodeTableId = m.CODE_TABLE_ID,
            CodeValueId = m.CODE_VALUE_ID,
            DecodeTxt = m.DECODE_TXT
        });

        if (predicate != null)
        {
            query = query.AsQueryable().Where(predicate);
        }

        return query;
    }

    public IEnumerable<ShowModel> GetShows(Expression<Func<ShowInfoModel, bool>>? predicate)
    {
        Dictionary<int, string> showTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.SHOW_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);

        IEnumerable<ShowInfoModel> infoQuery = from s in _context.SL_SHOW
                                       join t in _context.SL_TV_EPISODE_INFO on new { Id = s.INFO_ID ?? -1, Type = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? INFO_TYPE.TV : INFO_TYPE.MOVIE } equals new { Id = t.TV_EPISODE_INFO_ID, Type = INFO_TYPE.TV } into ts
                                       from t in ts.DefaultIfEmpty()
                                       join ti in _context.SL_TV_INFO on new { Id = t.TV_INFO_ID, Type = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? INFO_TYPE.TV : INFO_TYPE.MOVIE } equals new { Id = ti.TV_INFO_ID, Type = INFO_TYPE.TV } into tis
                                       from ti in tis.DefaultIfEmpty()
                                       join m in _context.SL_MOVIE_INFO on new { Id = s.INFO_ID ?? -1, Type = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? INFO_TYPE.TV : INFO_TYPE.MOVIE } equals new { Id = m.MOVIE_INFO_ID, Type = INFO_TYPE.MOVIE } into ms
                                       from m in ms.DefaultIfEmpty()
                                       select new ShowInfoModel
                                       {
                                           ShowId = s.SHOW_ID,
                                           UserId = s.USER_ID,
                                           ShowName = s.SHOW_NAME,
                                           SeasonNumber = s.SEASON_NUMBER,
                                           EpisodeNumber = s.EPISODE_NUMBER,
                                           DateWatched = s.DATE_WATCHED,
                                           ShowTypeId = s.SHOW_TYPE_ID,
                                           ShowTypeIdZ = showTypeIds[s.SHOW_TYPE_ID],
                                           ShowNotes = s.SHOW_NOTES,
                                           RestartBinge = s.RESTART_BINGE,
                                           Runtime = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? t.RUNTIME : null) : (m != null ? m.RUNTIME : null),
                                           EpisodeName = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? t.EPISODE_NAME : null) : "",
                                           //ImageUrl = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? !string.IsNullOrEmpty(t.IMAGE_URL) ? $"{_apisConfig.TMDbURL}{TMDBApiPaths.Image}{t.IMAGE_URL}" : "" : null) : "",
                                           //ImageUrl = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? GetImageUrl(t.API_TYPE, t.IMAGE_URL) : "") : "",
                                           //InfoUrl = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? !string.IsNullOrEmpty(t.IMAGE_URL) ? $"{_apisConfig.TMDbURL}{TMDBApiPaths.TV}{$"/{ti.TMDB_ID}/season/{t.SEASON_NUMBER}/episode/{t.EPISODE_NUMBER}"}" : "" : null) : "",
                                           //InfoUrl = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? GetTvEpisodeInfoUrl(t.API_TYPE, t.API_ID, t.SEASON_NUMBER, t.EPISODE_NUMBER) : "") : "",
                                           InfoId = s.INFO_ID,

                                           InfoApiType = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? t.API_TYPE : null) : (m != null ? m.API_TYPE : null),
                                           InfoApiId = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? ti.API_ID : null) : (m != null ? m.API_ID : null),
                                           InfoSeasonNumber = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? t.SEASON_NUMBER : null) : null,
                                           InfoEpisodeNumber = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? t.EPISODE_NUMBER : null) : null,
                                           InfoImageUrl = s.SHOW_TYPE_ID == (int)CodeValueIds.TV ? (t != null ? t.IMAGE_URL : null) : (m != null ? m.IMAGE_URL : null),
                                       };



        //IEnumerable < ShowModel > query = _context.SL_SHOW.Select(m => new ShowModel
        //{
        //    ShowId = m.SHOW_ID,
        //    UserId = m.USER_ID,
        //    ShowName = m.SHOW_NAME,
        //    SeasonNumber = m.SEASON_NUMBER,
        //    EpisodeNumber = m.EPISODE_NUMBER,
        //    DateWatched = m.DATE_WATCHED,
        //    ShowTypeId = m.SHOW_TYPE_ID,
        //    ShowTypeIdZ = showTypeIds[m.SHOW_TYPE_ID],
        //    ShowNotes = m.SHOW_NOTES,
        //    RestartBinge = m.RESTART_BINGE
        //});

        if (predicate != null)
        {
            infoQuery = infoQuery.AsQueryable().Where(predicate);
        }

        IEnumerable<ShowModel> query = infoQuery.ToList().Select(m =>
        {
            m.ImageUrl = GetImageUrl(m.InfoApiType, m.InfoImageUrl);
            m.InfoUrl = m.ShowTypeId == (int)CodeValueIds.TV ? GetTvEpisodeInfoUrl(m.InfoApiType, m.InfoApiId, m.InfoSeasonNumber, m.InfoEpisodeNumber) : GetMovieInfoUrl(m.InfoApiType, m.InfoApiId);
            return m;
        });

        return query;
    }

    private string GetImageUrl(int? apiType, string? imageUrl)
    {
        if(apiType == null
            || string.IsNullOrEmpty(imageUrl))
        {
            return "";
        }

        return (INFO_API)apiType switch
        {
            INFO_API.TMDB_API => $"{_apisConfig.TMDbURL}{TMDBApiPaths.Image}{imageUrl}",
            INFO_API.OMDB_API => "",
            _ => throw new NotImplementedException(),
        };
    }

    private string GetTvEpisodeInfoUrl(int? apiType, string? apiId, int? seasonNumber, int? episodeNumber)
    {
        if(apiType == null
            || string.IsNullOrEmpty(apiId)
            || seasonNumber == null
            || episodeNumber == null)
        {
            return "";
        }

        return (INFO_API)apiType switch
        {
            INFO_API.TMDB_API => $"{_apisConfig.TMDbURL}{TMDBApiPaths.TV}{$"/{apiId}/season/{seasonNumber}/episode/{episodeNumber}"}",
            INFO_API.OMDB_API => "",
            _ => throw new NotImplementedException(),
        };
    }

    private string GetMovieInfoUrl(int? apiType, string? apiId)
    {
        if (apiType == null
            || string.IsNullOrEmpty(apiId))
        {
            return "";
        }

        return (INFO_API)apiType switch
        {
            INFO_API.TMDB_API => $"{_apisConfig.TMDbURL}{TMDBApiPaths.Movie}{apiId}",
            INFO_API.OMDB_API => "",
            _ => throw new NotImplementedException(),
        };
    }

    public long CreateShow(int userId, ShowModel model)
    {
        long id = 0;

        SL_SHOW entity = new SL_SHOW
        {
            SHOW_TYPE_ID = model.ShowTypeId,
            DATE_WATCHED = model.DateWatched,
            EPISODE_NUMBER = model.EpisodeNumber,
            SEASON_NUMBER = model.SeasonNumber,
            SHOW_NAME = model.ShowName,
            SHOW_NOTES = model.ShowNotes,
            RESTART_BINGE = model.RestartBinge,
            USER_ID = userId,
            INFO_ID = model.InfoId
        };

        _context.SL_SHOW.Add(entity);
        _context.SaveChanges();
        id = entity.SHOW_ID;

        return id;
    }

    public long UpdateShow(int userId, ShowModel model)
    {
        SL_SHOW? entity = _context.SL_SHOW.FirstOrDefault(m => m.SHOW_ID == model.ShowId && m.USER_ID == userId);

        if (entity != null)
        {
            entity.SHOW_TYPE_ID = model.ShowTypeId;
            entity.DATE_WATCHED = model.DateWatched;
            entity.EPISODE_NUMBER = model.EpisodeNumber;
            entity.SEASON_NUMBER = model.SeasonNumber;
            entity.SHOW_NAME = model.ShowName;
            entity.SHOW_NOTES = model.ShowNotes;
            entity.RESTART_BINGE = model.RestartBinge;

            if(entity.SHOW_TYPE_ID == (int)CodeValueIds.TV)
            {
                entity.INFO_ID = GetTvEpisodeInfoId(model.ShowName, model.SeasonNumber, model.EpisodeNumber);
            }

            return _context.SaveChanges();
        }
        else
            return 0;
    }

    public int AddNextEpisode(int userId, int showId)
    {
        int newShowId = -1;
        SL_SHOW? entity = _context.SL_SHOW.FirstOrDefault(m => m.SHOW_ID == showId && m.USER_ID == userId);

        if (entity != null)
        {
            SL_SHOW nextEpisode = new SL_SHOW
            {
                SHOW_NAME = entity.SHOW_NAME,
                SHOW_TYPE_ID = entity.SHOW_TYPE_ID,
                USER_ID = userId,
                SEASON_NUMBER = entity.SEASON_NUMBER,
                EPISODE_NUMBER = entity.EPISODE_NUMBER + 1,
                DATE_WATCHED = DateTime.Now.GetEST().Date,
            };

            if (entity.INFO_ID != null)
            {
                SL_TV_EPISODE_INFO nextEpisodeInfo = null;

                List<SL_TV_EPISODE_INFO> episodes = GetEpisodes(entity.INFO_ID);

                if (episodes != null)
                {
                    int index = episodes.FindIndex(m => m.TV_EPISODE_INFO_ID == entity.INFO_ID);
                    if(index != -1 && index + 1 < episodes.Count - 1)
                    {
                        nextEpisodeInfo = episodes[index + 1];
                    }
                }

                if(nextEpisodeInfo != null)
                {
                    nextEpisode.SEASON_NUMBER = nextEpisodeInfo.SEASON_NUMBER;
                    nextEpisode.EPISODE_NUMBER = nextEpisodeInfo.EPISODE_NUMBER;
                    nextEpisode.INFO_ID = nextEpisodeInfo.TV_EPISODE_INFO_ID;
                }
                else
                {
                    nextEpisode.INFO_ID = GetTvEpisodeInfoId(entity.INFO_ID, entity.SEASON_NUMBER, entity.EPISODE_NUMBER);
                }
            }

            _context.SL_SHOW.Add(nextEpisode);

            _context.SaveChanges();

            newShowId = nextEpisode.SHOW_ID;
        }

        return newShowId;
    }

    private int? GetTvEpisodeInfoId(int? infoId, int? seasonNumber, int? episodeNumber)
    {
        SL_TV_EPISODE_INFO? currentInfo = _context.SL_TV_EPISODE_INFO.FirstOrDefault(m => m.TV_EPISODE_INFO_ID == infoId);
        SL_TV_EPISODE_INFO? nextInfo = null;
        if (currentInfo != null)
        {
            nextInfo = _context.SL_TV_EPISODE_INFO.FirstOrDefault(m => m.SEASON_NUMBER == seasonNumber && m.EPISODE_NUMBER == episodeNumber + 1 && m.TV_INFO_ID == currentInfo.TV_INFO_ID);

            if (nextInfo == null && episodeNumber != null)
            {
                // Somes shows I track are by episode and ignoring season. 
                // So get all episodes and then find the row that matches the episode
                nextInfo = _context.SL_TV_EPISODE_INFO.Where(m => m.TV_INFO_ID == currentInfo.TV_INFO_ID && m.SEASON_NUMBER > 0)
                .OrderBy(m => m.SEASON_NUMBER).ThenBy(m => m.EPISODE_NUMBER)
                    .Skip(episodeNumber.Value - 1).FirstOrDefault();
            }
        }

        return nextInfo?.TV_EPISODE_INFO_ID;
    }

    private int? GetTvEpisodeInfoId(string name, int? seasonNumber, int? episodeNumber)
    {
        int? infoId = null;
        Dictionary<string, SL_TV_INFO> lookUp = new();

        SL_TV_INFO[] shows = _context.SL_TV_INFO.ToArray();

        foreach(SL_TV_INFO info in shows)
        {
            lookUp.Add(info.SHOW_NAME.ToLower(), info);

            if (!string.IsNullOrEmpty(info.OTHER_NAMES))
            {
                string[] otherNames = info.OTHER_NAMES.ToLower().Split("|");

                foreach (string otherName in otherNames)
                {
                    lookUp.Add(otherName, info);
                }
            }
        }

        //SL_TV_INFO? show = _context.SL_TV_INFO.FirstOrDefault(m => m.SHOW_NAME.ToLower() == name.ToLower() || m.)

        SL_TV_INFO found;

        if(lookUp.TryGetValue(name.ToLower(), out found))
        {
            SL_TV_EPISODE_INFO? episodeInfo = _context.SL_TV_EPISODE_INFO.FirstOrDefault(m => m.TV_INFO_ID == found.TV_INFO_ID && m.SEASON_NUMBER == seasonNumber && m.EPISODE_NUMBER == episodeNumber);

            if (episodeInfo == null && episodeNumber != null)
            {
                // Somes shows I track are by episode and ignoring season. 
                // So get all episodes and then find the row that matches the episode
                episodeInfo = _context.SL_TV_EPISODE_INFO.Where(m => m.TV_INFO_ID == found.TV_INFO_ID && m.SEASON_NUMBER > 0)
                    .OrderBy(m => m.SEASON_NUMBER).ThenBy(m => m.EPISODE_NUMBER)
                    .Skip(episodeNumber.Value-1).FirstOrDefault();
            }

            if (episodeInfo != null)
            {
                infoId = episodeInfo.TV_EPISODE_INFO_ID;
            }
        }

        return infoId;
    }

    public bool AddOneDay(int userId, int showId)
    {
        SL_SHOW? entity = _context.SL_SHOW.FirstOrDefault(m => m.SHOW_ID == showId && m.USER_ID == userId);

        if (entity != null)
        {
            entity.DATE_WATCHED = entity.DATE_WATCHED.AddDays(1);

            _context.SaveChanges();

            return true;
        }
        else
            return false;
    }

    public bool SubtractOneDay(int userId, int showId)
    {
        SL_SHOW? entity = _context.SL_SHOW.FirstOrDefault(m => m.SHOW_ID == showId && m.USER_ID == userId);

        if (entity != null)
        {
            entity.DATE_WATCHED = entity.DATE_WATCHED.AddDays(-1);

            _context.SaveChanges();

            return true;
        }
        else
            return false;
    }

    public bool AddRange(int userId, AddRangeModel model)
    {
        bool successful = false;

        for(int i = model.AddRangeStartEpisode; i <= model.AddRangeEndEpisode; i++)
        {
            int? nextInfoId = GetTvEpisodeInfoId(model.AddRangeShowName, model.AddRangeSeasonNumber, i);

            SL_SHOW nextEpisode = new SL_SHOW
            {
                SHOW_NAME = model.AddRangeShowName,
                SHOW_TYPE_ID = (int)CodeValueIds.TV,
                USER_ID = userId,
                SEASON_NUMBER = model.AddRangeSeasonNumber,
                EPISODE_NUMBER = i,
                INFO_ID = nextInfoId,
                DATE_WATCHED = model.AddRangeDateWatched.Date,
            };

            _context.SL_SHOW.Add(nextEpisode);
        }

        _context.SaveChanges();

        successful = true;

        return successful;
    }

    public bool DeleteShow(int userId, int showId)
    {
        SL_SHOW? entity = _context.SL_SHOW.FirstOrDefault(m => m.SHOW_ID == showId && m.USER_ID == userId);

        if (entity != null)
        {
            _context.SL_SHOW.Remove(entity);

            _context.SaveChanges();

            return true;
        }
        else
            return false;
    }

    public IEnumerable<GroupedShowModel> GetTVStats(int userId)
    {
        //IEnumerable<GroupedShowModel> query = _context.SL_SHOW.Where(m => m.SHOW_TYPE_ID == (int)CodeValueIds.TV && m.USER_ID == userId).GroupBy(m => new
        //{
        //    m.SHOW_NAME,
        //    m.USER_ID
        //})
        //.Select(m => new GroupedShowModel
        //{
        //    UserId = m.Key.USER_ID,
        //    ShowId = m.Max(m => m.SHOW_ID),
        //    ShowName = m.Key.SHOW_NAME,
        //    FirstWatched = m.Min(m => m.DATE_WATCHED),
        //    LastWatched = m.Max(m => m.DATE_WATCHED),
        //    LatestSeasonNumber = m.OrderByDescending(m => m.SHOW_ID).FirstOrDefault().SEASON_NUMBER,
        //    LatestEpisodeNumber = m.OrderByDescending(m => m.SHOW_ID).FirstOrDefault().EPISODE_NUMBER,
        //    EpisodesWatched = m.Count()
        //}).ToList();

        IEnumerable<SL_SHOW> shows = _context.SL_SHOW.Where(m => m.SHOW_TYPE_ID == (int)CodeValueIds.TV && m.USER_ID == userId)
            .OrderBy(m => m.SHOW_NAME)
            .ThenBy(m => m.DATE_WATCHED)
            .ThenBy(m => m.SHOW_ID);

        GroupedShowModel model = new GroupedShowModel();
        int count = 0;
        SL_SHOW? previousShow = null;

        List<GroupedShowModel> list = new List<GroupedShowModel>();

        foreach (SL_SHOW? show in shows)
        {
            if(model.ShowName != show.SHOW_NAME
                || show.RESTART_BINGE
                //|| (previousShow?.SHOW_NAME == show.SHOW_NAME
                //    && previousShow.SEASON_NUMBER > show.SEASON_NUMBER)
               )
            {
                if(previousShow != null)
                {
                    model.LastWatched = previousShow.DATE_WATCHED;
                    model.LatestSeasonNumber = previousShow.SEASON_NUMBER;
                    model.LatestEpisodeNumber = previousShow.EPISODE_NUMBER;
                    model.EpisodesWatched = (model.LatestSeasonNumber == previousShow.SEASON_NUMBER && model.LatestEpisodeNumber == previousShow.EPISODE_NUMBER ? count : ++count);
                    model.ShowId = previousShow.SHOW_ID;

                    list.Add(model);
                }

                model = new GroupedShowModel
                {
                    UserId = userId,
                    ShowName = show.SHOW_NAME,
                    FirstWatched = show.DATE_WATCHED,
                    StartingSeasonNumber = show.SEASON_NUMBER,
                    StartingEpisodeNumber = show.EPISODE_NUMBER,
                };

                count = 1;
            }
            else if(previousShow != null && previousShow.DATE_WATCHED.AddMonths(4) < show.DATE_WATCHED)
            {
                model.LastWatched = previousShow.DATE_WATCHED;
                model.LatestSeasonNumber = previousShow.SEASON_NUMBER;
                model.LatestEpisodeNumber = previousShow.EPISODE_NUMBER;
                model.EpisodesWatched = (model.LatestSeasonNumber == previousShow.SEASON_NUMBER && model.LatestEpisodeNumber == previousShow.EPISODE_NUMBER ? count : ++count);
                model.ShowId = previousShow.SHOW_ID;

                list.Add(model);

                model = new GroupedShowModel
                {
                    UserId = userId,
                    ShowName = show.SHOW_NAME,
                    FirstWatched = show.DATE_WATCHED,
                    StartingSeasonNumber = show.SEASON_NUMBER,
                    StartingEpisodeNumber = show.EPISODE_NUMBER,
                };

                count = 1;
            }
            else 
            {
                ++count;
            }

            previousShow = show;
        }


        return list;
    }

    public IEnumerable<MovieModel> GetMovieStats(int userId)
    {
        Dictionary<int, string> showTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.SHOW_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);

        List<MovieModel> query = _context.SL_SHOW.Where(m => m.USER_ID == userId && (m.SHOW_TYPE_ID == (int)CodeValueIds.AMC || m.SHOW_TYPE_ID == (int)CodeValueIds.MOVIE)).Select(m => new MovieModel
        {
            UserId = m.USER_ID,
            MovieName = m.SHOW_NAME,
            ShowId = m.SHOW_ID,
            ShowTypeId = m.SHOW_TYPE_ID,
            ShowTypeIdZ = showTypeIds[m.SHOW_TYPE_ID],
            DateWatched = m.DATE_WATCHED,
        }).ToList();

        int[] amcIds = query.Where(m => m.ShowTypeId == (int)CodeValueIds.AMC).Select(m => m.ShowId).ToArray();

        if(amcIds.Length > 0)
        {
            IEnumerable<IGrouping<int, SL_TRANSACTION>> transactions = _context.SL_TRANSACTION.Where(m => m.SHOW_ID != null && m.USER_ID == userId).AsEnumerable().GroupBy(m => m.SHOW_ID.Value);

            if(transactions != null && transactions.Count() > 0)
            {
                foreach (IGrouping<int, SL_TRANSACTION> transaction in transactions)
                {
                    MovieModel movie = query.First(m => m.ShowId == transaction.Key);

                    movie.AlistTicketAmt = transaction.Where(m => m.TRANSACTION_TYPE_ID == (int)CodeValueIds.ALIST_TICKET).Sum(m => m.COST_AMT - (m.DISCOUNT_AMT ?? 0) - (m.BENEFIT_AMT ?? 0));
                    movie.TicketAmt = transaction.Where(m => m.TRANSACTION_TYPE_ID == (int)CodeValueIds.TICKET).Sum(m => m.COST_AMT - (m.DISCOUNT_AMT ?? 0) - (m.BENEFIT_AMT ?? 0));
                    movie.PurchaseAmt = transaction.Where(m => m.TRANSACTION_TYPE_ID == (int)CodeValueIds.PURCHASE).Sum(m => m.COST_AMT - (m.DISCOUNT_AMT ?? 0) - (m.BENEFIT_AMT ?? 0));
                }
            }
        }

        return query;
    }

    public IEnumerable<FriendWatchHistoryModel> GetFriendsWatchHistory(int userId)
    {
        int[] friends = _context.SL_FRIEND.Where(m => m.USER_ID == userId).Select(m => m.FRIEND_USER_ID)
            .Union(_context.SL_FRIEND.Where(m => m.FRIEND_USER_ID == userId).Select(m => m.USER_ID)).ToArray();

        Dictionary<int, string> showTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.SHOW_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);

        IEnumerable<FriendWatchHistoryModel> query = (from m in _context.SL_SHOW
                                                      join u in _context.OA_USERS on m.USER_ID equals u.USER_ID
                                                      select new FriendWatchHistoryModel
                                                      {
                                                          ShowId = m.SHOW_ID,
                                                          UserId = m.USER_ID,
                                                          Name = $"{u.LAST_NAME}, {u.FIRST_NAME}",
                                                          ShowName = m.SHOW_NAME,
                                                          SeasonNumber = m.SEASON_NUMBER,
                                                          EpisodeNumber = m.EPISODE_NUMBER,
                                                          DateWatched = m.DATE_WATCHED,
                                                          ShowTypeId = m.SHOW_TYPE_ID,
                                                          ShowTypeIdZ = showTypeIds[m.SHOW_TYPE_ID],
                                                          ShowNotes = m.SHOW_NOTES,
                                                      }).Where(m => friends.Contains(m.UserId));

        return query;
    }

    public IEnumerable<WatchlistModel> GetWatchList(Expression<Func<WatchlistModel, bool>>? predicate)
    {
        Dictionary<int, string> showTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.SHOW_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);

        IEnumerable<WatchlistModel> query = _context.SL_WATCHLIST.Select(m => new WatchlistModel
        {
            WatchlistId = m.WATCHLIST_ID,
            UserId = m.USER_ID,
            ShowName = m.SHOW_NAME,
            SeasonNumber = m.SEASON_NUMBER,
            EpisodeNumber = m.EPISODE_NUMBER,
            DateAdded = m.DATE_ADDED,
            ShowTypeId = m.SHOW_TYPE_ID,
            ShowTypeIdZ = showTypeIds[m.SHOW_TYPE_ID],
            ShowNotes = m.SHOW_NOTES,
        });

        if (predicate != null)
        {
            query = query.AsQueryable().Where(predicate);
        }

        return query;
    }

    public long CreateWatchlist(int userId, WatchlistModel model)
    {
        long id = 0;

        SL_WATCHLIST entity = new SL_WATCHLIST
        {
            SHOW_TYPE_ID = model.ShowTypeId,
            DATE_ADDED = DateTime.Now.Date,
            EPISODE_NUMBER = model.EpisodeNumber,
            SEASON_NUMBER = model.SeasonNumber,
            SHOW_NAME = model.ShowName,
            SHOW_NOTES = model.ShowNotes,
            USER_ID = userId
        };

        _context.SL_WATCHLIST.Add(entity);
        _context.SaveChanges();
        id = entity.WATCHLIST_ID;

        return id;
    }

    public long UpdateWatchlist(int userId, WatchlistModel model)
    {
        SL_WATCHLIST? entity = _context.SL_WATCHLIST.FirstOrDefault(m => m.WATCHLIST_ID == model.WatchlistId && m.USER_ID == userId);

        if (entity != null)
        {
            entity.SHOW_TYPE_ID = model.ShowTypeId;
            entity.EPISODE_NUMBER = model.EpisodeNumber;
            entity.SEASON_NUMBER = model.SeasonNumber;
            entity.SHOW_NAME = model.ShowName;
            entity.SHOW_NOTES = model.ShowNotes;

            return _context.SaveChanges();
        }
        else
            return 0;
    }

    public bool DeleteWatchlist(int userId, int watchListId)
    {
        SL_WATCHLIST? entity = _context.SL_WATCHLIST.FirstOrDefault(m => m.WATCHLIST_ID == watchListId && m.USER_ID == userId);

        if (entity != null)
        {
            _context.SL_WATCHLIST.Remove(entity);

            _context.SaveChanges();

            return true;
        }
        else
            return false;
    }

    public bool MoveWatchlistToShow(int userId, int watchListId)
    {
        SL_WATCHLIST? entity = _context.SL_WATCHLIST.FirstOrDefault(m => m.WATCHLIST_ID == watchListId && m.USER_ID == userId);

        if (entity != null)
        {
            long id = CreateShow(userId, new ShowModel
            {
                EpisodeNumber = entity.EPISODE_NUMBER,
                SeasonNumber = entity.SEASON_NUMBER,
                ShowName = entity.SHOW_NAME,
                ShowNotes = entity.SHOW_NOTES,
                ShowTypeId = entity.SHOW_TYPE_ID,
                DateWatched = DateTime.Now.GetEST().Date
            });

            if(id > 0)
            {
                DeleteWatchlist(userId, watchListId);
            }

            return true;
        }
        else
            return false;
    }

    public IEnumerable<TransactionModel> GetTransactions(int userId, Expression<Func<TransactionModel, bool>>? predicate = null)
    {
        Dictionary<int, string> transactionTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.TRANSACTION_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);
        Dictionary<int, string> showIds = _context.SL_SHOW.Where(m => m.USER_ID == userId).ToDictionary(m => m.SHOW_ID, m => m.SHOW_NAME);

        IEnumerable<TransactionModel> query = _context.SL_TRANSACTION.Select(m => new TransactionModel
        {
            TransactionId = m.TRANSACTION_ID,
            UserId = m.USER_ID,
            TransactionTypeId = m.TRANSACTION_TYPE_ID,
            TransactionTypeIdZ = transactionTypeIds[m.TRANSACTION_TYPE_ID],
            ShowId = m.SHOW_ID,
            ShowIdZ = m.SHOW_ID != null ? showIds[m.SHOW_ID.Value] : "",
            Item = m.ITEM,
            CostAmt = m.COST_AMT,
            DiscountAmt = m.DISCOUNT_AMT,
            BenefitAmt = m.BENEFIT_AMT,
            TransactionNotes = m.TRANSACTION_NOTES,
            TransactionDate = m.TRANSACTION_DATE,
        });

        if (predicate != null)
        {
            query = query.AsQueryable().Where(predicate);
        }

        return query;
    }

    public long CreateTransaction(int userId, TransactionModel model)
    {
        long id = 0;

        SL_TRANSACTION entity = new SL_TRANSACTION
        {
            TRANSACTION_TYPE_ID = model.TransactionTypeId,
            SHOW_ID = model.ShowId,
            ITEM = model.Item,
            COST_AMT = model.CostAmt,
            DISCOUNT_AMT = model.DiscountAmt,
            BENEFIT_AMT = model.BenefitAmt,
            TRANSACTION_NOTES = model.TransactionNotes,
            TRANSACTION_DATE = model.TransactionDate,
            USER_ID = userId
        };

        _context.SL_TRANSACTION.Add(entity);
        _context.SaveChanges();
        id = entity.TRANSACTION_ID;

        return id;
    }

    public long UpdateTransaction(int userId, TransactionModel model)
    {
        SL_TRANSACTION? entity = _context.SL_TRANSACTION.FirstOrDefault(m => m.TRANSACTION_ID == model.TransactionId && m.USER_ID == userId);

        if (entity != null)
        {
            entity.TRANSACTION_TYPE_ID = model.TransactionTypeId;
            entity.SHOW_ID = model.ShowId;
            entity.ITEM = model.Item;
            entity.COST_AMT = model.CostAmt;
            entity.DISCOUNT_AMT = model.DiscountAmt;
            entity.BENEFIT_AMT = model.BenefitAmt;
            entity.TRANSACTION_NOTES = model.TransactionNotes;
            entity.TRANSACTION_DATE = model.TransactionDate;

            return _context.SaveChanges();
        }
        else
            return 0;
    }

    public bool DeleteTransaction(int userId, int transactionId)
    {
        SL_TRANSACTION? entity = _context.SL_TRANSACTION.FirstOrDefault(m => m.TRANSACTION_ID == transactionId && m.USER_ID == userId);

        if (entity != null)
        {
            _context.SL_TRANSACTION.Remove(entity);

            _context.SaveChanges();

            return true;
        }
        else
            return false;
    }

    public IEnumerable<YearStatsModel> GetYearStats(int userId)
    {
        int[] friends = _context.SL_FRIEND.Where(m => m.USER_ID == userId).Select(m => m.FRIEND_USER_ID)
            .Union(_context.SL_FRIEND.Where(m => m.FRIEND_USER_ID == userId).Select(m => m.USER_ID)).ToArray();

        IEnumerable<YearStatsModel> modelShows = (from u in _context.OA_USERS
                                                  join x in _context.SL_SHOW on u.USER_ID equals x.USER_ID
                                                  group new { x, u } by new { x.USER_ID, x.DATE_WATCHED.Year, u.FIRST_NAME, u.LAST_NAME, u.USER_NAME } into g
                                                  select new YearStatsModel
                                                  {
                                                      UserId = g.Key.USER_ID,
                                                      Name = $"{g.Key.LAST_NAME}, {g.Key.FIRST_NAME}",
                                                      Year = g.Key.Year,
                                                      TvCnt = g.Count(m => m.x.SHOW_TYPE_ID == (int)CodeValueIds.TV),
                                                      MoviesCnt = g.Count(m => m.x.SHOW_TYPE_ID == (int)CodeValueIds.MOVIE),
                                                      AmcCnt = g.Count(m => m.x.SHOW_TYPE_ID == (int)CodeValueIds.AMC),
                                                  }).Where(m => m.UserId == userId || friends.Contains(m.UserId)).ToList();

        IEnumerable<YearStatsModel> modelTransactions = (from u in _context.OA_USERS
                                     join t in _context.SL_TRANSACTION on u.USER_ID equals t.USER_ID
                                     group new { t, u } by new { u.USER_ID, t.TRANSACTION_DATE.Year, u.FIRST_NAME, u.LAST_NAME, u.USER_NAME } into g
                                     select new YearStatsModel
                                     {
                                         UserId = g.Key.USER_ID,
                                         Name = $"{g.Key.LAST_NAME}, {g.Key.FIRST_NAME}",
                                         Year = g.Key.Year,
                                         AListMembership = g.Where(m => m.t.TRANSACTION_TYPE_ID == (int)CodeValueIds.ALIST).Sum(m => m.t.COST_AMT),
                                         AListTickets = g.Where(m => m.t.TRANSACTION_TYPE_ID == (int)CodeValueIds.ALIST_TICKET).Sum(m => m.t.COST_AMT),
                                         AmcPurchases = g.Where(m => m.t.TRANSACTION_TYPE_ID == (int)CodeValueIds.TICKET || m.t.TRANSACTION_TYPE_ID == (int)CodeValueIds.PURCHASE).Sum(m => m.t.COST_AMT - (m.t.BENEFIT_AMT ?? 0) - (m.t.DISCOUNT_AMT ?? 0)),
                                     }).Where(m => m.UserId == userId || friends.Contains(m.UserId)).ToList();

        IEnumerable<YearStatsModel> tvRuntimes = (from u in _context.OA_USERS
                                                  join x in _context.SL_SHOW on u.USER_ID equals x.USER_ID
                                                  join ei in _context.SL_TV_EPISODE_INFO on x.INFO_ID equals ei.TV_EPISODE_INFO_ID
                                                  where x.SHOW_TYPE_ID == (int)CodeValueIds.TV && x.INFO_ID != null
                                                  group new { x, u, ei } by new { x.USER_ID, x.DATE_WATCHED.Year } into g
                                                  select new YearStatsModel
                                                  {
                                                      UserId = g.Key.USER_ID,
                                                      Year = g.Key.Year,
                                                      TvRuntime = g.Sum(m => m.ei.RUNTIME) ?? 0
                                                  }).Where(m => m.UserId == userId || friends.Contains(m.UserId)).ToList();

        IEnumerable<YearStatsModel> movieRuntimes = (from u in _context.OA_USERS
                                                  join x in _context.SL_SHOW on u.USER_ID equals x.USER_ID
                                                  join mi in _context.SL_MOVIE_INFO on x.INFO_ID equals mi.MOVIE_INFO_ID
                                                  where x.SHOW_TYPE_ID == (int)CodeValueIds.MOVIE && x.INFO_ID != null
                                                  group new { x, u, mi } by new { x.USER_ID, x.DATE_WATCHED.Year } into g
                                                  select new YearStatsModel
                                                  {
                                                      UserId = g.Key.USER_ID,
                                                      Year = g.Key.Year,
                                                      MoviesRuntime = g.Sum(m => m.mi.RUNTIME) ?? 0
                                                  }).Where(m => m.UserId == userId || friends.Contains(m.UserId)).ToList();

        IEnumerable<YearStatsModel> amcRuntimes = (from u in _context.OA_USERS
                                                     join x in _context.SL_SHOW on u.USER_ID equals x.USER_ID
                                                     join mi in _context.SL_MOVIE_INFO on x.INFO_ID equals mi.MOVIE_INFO_ID
                                                     where x.SHOW_TYPE_ID == (int)CodeValueIds.AMC && x.INFO_ID != null
                                                     group new { x, u, mi } by new { x.USER_ID, x.DATE_WATCHED.Year } into g
                                                     select new YearStatsModel
                                                     {
                                                         UserId = g.Key.USER_ID,
                                                         Year = g.Key.Year,
                                                         AmcRuntime = g.Sum(m => m.mi.RUNTIME) ?? 0
                                                     }).Where(m => m.UserId == userId || friends.Contains(m.UserId)).ToList();

        IEnumerable<YearStatsModel> model = (from s in modelShows
                                             join t in modelTransactions on new { s.UserId, s.Year } equals new { t.UserId, t.Year } into ts
                                             from t in ts.DefaultIfEmpty()
                                             join rtv in tvRuntimes on new { s.UserId, s.Year } equals new { rtv.UserId, rtv.Year } into rtvs
                                             from rtv in rtvs.DefaultIfEmpty()
                                             join rmovies in movieRuntimes on new { s.UserId, s.Year } equals new { rmovies.UserId, rmovies.Year } into rmoviess
                                             from rmovies in rmoviess.DefaultIfEmpty()
                                             join ramc in tvRuntimes on new { s.UserId, s.Year } equals new { ramc.UserId, ramc.Year } into ramcs
                                             from ramc in rmoviess.DefaultIfEmpty()
                                             select new YearStatsModel
                                             {
                                                 UserId = s.UserId,
                                                 Year = s.Year,
                                                 Name = s.Name,
                                                 TvCnt = s.TvCnt,
                                                 TvRuntime = rtv?.TvRuntime,
                                                 MoviesCnt = s.MoviesCnt,
                                                 MoviesRuntime = rmovies?.MoviesRuntime,
                                                 AmcCnt = s.AmcCnt,
                                                 AmcRuntime = ramc?.AmcRuntime,
                                                 AListMembership = t?.AListMembership ?? 0,
                                                 AListTickets = t?.AListTickets ?? 0,
                                                 AmcPurchases = t?.AmcPurchases ?? 0
                                             });

        return model;
    }

    public bool UpdateShowNames(UpdateUnlinkedShowNameModel model)
    {
        IEnumerable<SL_SHOW> shows = _context.SL_SHOW.Where(m => m.SHOW_NAME == model.UpdateUnlinkedShowNameShowName && m.SHOW_TYPE_ID == model.UpdateUnlinkedShowNameShowTypeId);

        foreach(SL_SHOW show in shows)
        {
            show.SHOW_NAME = model.UpdateUnlinkedShowNameNewShowName;
        }

        _context.SaveChanges();
        return true;
    }

    public bool LinkShows(LinkShowsModel model)
    {
        IEnumerable<SL_SHOW> shows = _context.SL_SHOW.Where(m => m.SHOW_NAME == model.ShowName && m.SHOW_TYPE_ID == model.ShowTypeId).ToList();

        if(model.ShowTypeId == (int)CodeValueIds.TV)
        {
            foreach (SL_SHOW show in shows)
            {
                int? infoId = GetTvEpisodeInfoId(model.ShowName, show.SEASON_NUMBER, show.EPISODE_NUMBER);

                show.INFO_ID = infoId;
            }
        }
        else
        {
            foreach (SL_SHOW show in shows)
            {
                show.INFO_ID = model.InfoId;
            }
        }

        _context.SaveChanges();
        return true;
    }

    private List<SL_TV_EPISODE_INFO> GetEpisodes(int? tvEpisodeInfoId)
    {
        SL_TV_EPISODE_INFO? currentInfo = _context.SL_TV_EPISODE_INFO.FirstOrDefault(m => m.TV_EPISODE_INFO_ID == tvEpisodeInfoId);

        if (currentInfo == null)
            return null;

        return _context.SL_TV_EPISODE_INFO.Where(m => m.TV_INFO_ID == currentInfo.TV_INFO_ID)
                .OrderByDescending(m => m.SEASON_NUMBER > 0)
                .ThenBy(m => m.SEASON_NUMBER)
                .ThenBy(m => m.EPISODE_NUMBER)
                .ToList();
    }
}
