using ShowLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories.Interfaces;

public interface IWatchedShowsRepository : IRepository
{
    IEnumerable<ShowModel> GetShows(Expression<Func<ShowModel, bool>> predicate = null);

    long CreateShow(int userId, ShowModel model);

    long UpdateShow(int userId, ShowModel model);

    bool DeleteShow(int userId, int showId);
}
