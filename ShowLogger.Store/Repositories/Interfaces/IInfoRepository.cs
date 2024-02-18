using ShowLogger.Models;
using ShowLogger.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories.Interfaces;
public interface IInfoRepository
{
    Task<DownloadResultModel> Download(int userId, InfoApiDownloadModel downloadInfo);

    Task<ApiResultModel<IEnumerable<ApiSearchResultModel>>> Search(int userId, InfoApiSearchModel searchInfo);

    IEnumerable<TvInfoModel> GetTvInfos(Expression<Func<TvInfoModel, bool>>? predicate = null);

    IEnumerable<TvInfoSeasonModel> GetTvInfoSeasons(int tvInfoId);

    IEnumerable<TvEpisodeInfoModel> GetTvEpisodeInfos(Expression<Func<TvEpisodeInfoModel, bool>>? predicate = null);

    long RefreshTvInfo(TvInfoModel model);

    long RefreshTvInfo(int infoId);

    long UpdateTvInfoOtherNames(int userId, int tvInfoId, string otherNames);

    bool DeleteTvInfo(int userId, int tvInfoId);

    IEnumerable<MovieInfoModel> GetMovieInfos(Expression<Func<MovieInfoModel, bool>>? predicate = null);

    long RefreshMovieInfo(MovieInfoModel model);

    long UpdateMovieInfoOtherNames(int userId, int movieInfoId, string otherNames);

    bool DeleteMovieInfo(int userId, int tvInfoId);

    void RefreshInfo(int infoId, INFO_TYPE type);

    IEnumerable<UnlinkedShowsModel> GetUnlinkedShows(Expression<Func<UnlinkedShowsModel, bool>>? predicate = null);
}
