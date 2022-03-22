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

    public IEnumerable<ShowModel> GetShows(Expression<Func<ShowModel, bool>> predicate)
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
            ShowTypeIdZ = showTypeIds[m.SHOW_TYPE_ID]
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
            USER_ID = userId
        };

        _context.SL_SHOW.Add(entity);
        _context.SaveChanges();
        id = entity.SHOW_ID;

        return id;
    }

    public long UpdateShow(int userId, ShowModel model)
    {
        SL_SHOW entity = _context.SL_SHOW.FirstOrDefault(m => m.SHOW_ID == model.ShowId && m.USER_ID == userId);

        if (entity != null)
        {
            entity.SHOW_TYPE_ID = model.ShowTypeId;
            entity.DATE_WATCHED = model.DateWatched;
            entity.EPISODE_NUMBER = model.EpisodeNumber;
            entity.SEASON_NUMBER = model.SeasonNumber;
            entity.SHOW_NAME = model.ShowName;

            return _context.SaveChanges();
        }
        else
            return 0;
    }

    public bool DeleteShow(int userId, int showId)
    {
        SL_SHOW entity = _context.SL_SHOW.FirstOrDefault(m => m.SHOW_ID == showId && m.USER_ID == userId);

        if (entity != null)
        {
            _context.SL_SHOW.Remove(entity);

            _context.SaveChanges();

            return true;
        }
        else
            return false;
    }
}
