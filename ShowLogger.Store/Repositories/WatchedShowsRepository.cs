using ShowLogger.Data.Context;
using ShowLogger.Data.Entities;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ShowLogger.Store.Repositories;

public class WatchedShowsRepository : IWatchedShowsRepository
{
    private readonly ShowLoggerDbContext _context;

    public WatchedShowsRepository(ShowLoggerDbContext context)
    {
        _context = context;
    }

    public IEnumerable<ShowModel> GetShows(Expression<Func<ShowModel, bool>>? predicate)
    {
        Dictionary<int, string> showTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.SHOW_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);

        IEnumerable<ShowModel> query = _context.SL_SHOW.Select(m => new ShowModel
        {
            ShowId = m.SHOW_ID,
            UserId = m.USER_ID,
            ShowName = m.SHOW_NAME,
            SeasonNumber = m.SEASON_NUMBER,
            EpisodeNumber = m.EPISODE_NUMBER,
            DateWatched = m.DATE_WATCHED,
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
            USER_ID = userId
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

            return _context.SaveChanges();
        }
        else
            return 0;
    }

    public bool AddNextEpisode(int userId, int showId)
    {
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

            _context.SL_SHOW.Add(nextEpisode);

            _context.SaveChanges();

            return true;
        }
        else
            return false;
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
        IEnumerable<GroupedShowModel> query = _context.SL_SHOW.Where(m => m.SHOW_TYPE_ID == (int)CodeValueIds.TV && m.USER_ID == userId).GroupBy(m => new
        {
            m.SHOW_NAME,
            m.USER_ID
        })
        .Select(m => new GroupedShowModel
        {
            UserId = m.Key.USER_ID,
            ShowId = m.Max(m => m.SHOW_ID),
            ShowName = m.Key.SHOW_NAME,
            FirstWatched = m.Min(m => m.DATE_WATCHED),
            LastWatched = m.Max(m => m.DATE_WATCHED),
            SeasonNumber = m.OrderByDescending(m => m.SHOW_ID).FirstOrDefault().SEASON_NUMBER,
            EpisodeNumber = m.OrderByDescending(m => m.SHOW_ID).FirstOrDefault().EPISODE_NUMBER,
            EpisodesWatched = m.Count()
        });

        return query;
    }

    public IEnumerable<MovieModel> GetMovieStats(int userId)
    {
        Dictionary<int, string> showTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.SHOW_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);

        IEnumerable<MovieModel> query = _context.SL_SHOW.Where(m => m.USER_ID == userId && (m.SHOW_TYPE_ID == (int)CodeValueIds.AMC || m.SHOW_TYPE_ID == (int)CodeValueIds.MOVIE)).Select(m => new MovieModel
        {
            UserId = m.USER_ID,
            MovieName = m.SHOW_NAME,
            ShowTypeIdZ = showTypeIds[m.SHOW_TYPE_ID],
            DateWatched = m.DATE_WATCHED,
        });

        return query;
    }

    public IEnumerable<FriendWatchHistoryModel> GetFriendsWatchHistory(int userId)
    {
        int[] friends = _context.SL_FRIEND.Where(m => m.USER_ID == userId).Select(m => m.FRIEND_USER_ID)
            .Union(_context.SL_FRIEND.Where(m => m.FRIEND_USER_ID == userId).Select(m => m.USER_ID)).ToArray();

        Dictionary<int, string> showTypeIds = _context.SL_CODE_VALUE.Where(m => m.CODE_TABLE_ID == (int)CodeTableIds.SHOW_TYPE_ID).ToDictionary(m => m.CODE_VALUE_ID, m => m.DECODE_TXT);

        IEnumerable<FriendWatchHistoryModel> query = _context.SL_SHOW.Select(m => new FriendWatchHistoryModel
        {
            ShowId = m.SHOW_ID,
            UserId = m.USER_ID,
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

}
