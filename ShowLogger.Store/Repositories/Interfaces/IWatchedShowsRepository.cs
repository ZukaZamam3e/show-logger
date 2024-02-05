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
    IEnumerable<CodeValueModel> GetCodeValues(Expression<Func<CodeValueModel, bool>>? predicate = null);

    IEnumerable<ShowModel> GetShows(Expression<Func<ShowInfoModel, bool>>? predicate = null);

    long CreateShow(int userId, ShowModel model);
    
    long UpdateShow(int userId, ShowModel model);

    int AddNextEpisode(int userId, int showId);

    bool AddOneDay(int userId, int showId);
    
    bool SubtractOneDay(int userId, int showId);

    bool AddRange(int userId, AddRangeModel model);

    bool DeleteShow(int userId, int showId);

    IEnumerable<GroupedShowModel> GetTVStats(int userId);

    IEnumerable<MovieModel> GetMovieStats(int userId);

    IEnumerable<FriendWatchHistoryModel> GetFriendsWatchHistory(int userId);

    IEnumerable<WatchlistModel> GetWatchList(Expression<Func<WatchlistModel, bool>>? predicate = null);

    long CreateWatchlist(int userId, WatchlistModel model);

    long UpdateWatchlist(int userId, WatchlistModel model);

    bool DeleteWatchlist(int userId, int watchListId);

    bool MoveWatchlistToShow(int userId, int watchListId);

    IEnumerable<TransactionModel> GetTransactions(int userId, Expression<Func<TransactionModel, bool>>? predicate = null);

    long CreateTransaction(int userId, TransactionModel model);

    long UpdateTransaction(int userId, TransactionModel model);

    bool DeleteTransaction(int userId, int transactionId);

    IEnumerable<YearStatsModel> GetYearStats(int userId);

    bool UpdateShowNames(UpdateUnlinkedShowNameModel model);

    bool LinkShows(LinkShowsModel model);
}
