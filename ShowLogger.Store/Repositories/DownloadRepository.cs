using Newtonsoft.Json;
using ShowLogger.Data.Context;
using ShowLogger.Data.Entities;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories;
public class DownloadRepository : IDownloadRepository
{
    private readonly ShowLoggerDbContext _context;

    public DownloadRepository(ShowLoggerDbContext context)
    {
        _context = context;
    }

    public ExportModel ExportData()
    {
        ExportModel model = new ExportModel();

        SL_SHOW[] sl_show = _context.SL_SHOW.ToArray();
        SL_BOOK[] sl_book = _context.SL_BOOK.ToArray();
        SL_FRIEND[] sl_friend = _context.SL_FRIEND.ToArray();
        SL_FRIEND_REQUEST[] sl_friend_request = _context.SL_FRIEND_REQUEST.ToArray();
        SL_WATCHLIST[] sl_watchlist = _context.SL_WATCHLIST.ToArray();
        SL_TRANSACTION[] sl_transaction = _context.SL_TRANSACTION.ToArray();
        SL_USER_PREF[] sl_user_pref = _context.SL_USER_PREF.ToArray();
        OA_USER_BASIC[] oa_users = _context.OA_USERS.Select(m => new OA_USER_BASIC
        {
            USERID = m.USER_ID,
            FIRSTNAME = m.FIRST_NAME,
            LASTNAME = m.LAST_NAME,
            EMAIL = m.USER_NAME
        }).OrderBy(m => m.USERID).ToArray();
        SL_TV_INFO_BASIC[] sl_tv_info = _context.SL_TV_INFO.Select(m => new SL_TV_INFO_BASIC
        {
            TV_INFO_ID = m.TV_INFO_ID,
            SHOW_NAME = m.SHOW_NAME,
            SHOW_OVERVIEW = m.SHOW_OVERVIEW,
            API_TYPE = m.API_TYPE,
            API_ID = m.API_ID,
            OTHER_NAMES = m.OTHER_NAMES,
            LAST_DATA_REFRESH = m.LAST_DATA_REFRESH,
            LAST_UPDATED = m.LAST_UPDATED,
            IMAGE_URL = m.IMAGE_URL

        }).ToArray();
        SL_TV_EPISODE_INFO_BASIC[] sl_tv_episode_info = _context.SL_TV_EPISODE_INFO.Select(m => new SL_TV_EPISODE_INFO_BASIC
        {
            TV_EPISODE_INFO_ID = m.TV_EPISODE_INFO_ID,
            TV_INFO_ID = m.TV_INFO_ID,
            API_TYPE = m.API_TYPE,
            API_ID = m.API_ID,
            SEASON_NAME = m.SEASON_NAME,
            EPISODE_NAME = m.EPISODE_NAME,
            SEASON_NUMBER = m.SEASON_NUMBER,
            EPISODE_NUMBER = m.EPISODE_NUMBER,
            EPISODE_OVERVIEW = m.EPISODE_OVERVIEW,
            RUNTIME = m.RUNTIME,
            AIR_DATE = m.AIR_DATE,
            IMAGE_URL = m.IMAGE_URL,

        }).ToArray();
        SL_MOVIE_INFO[] sl_movie_info = _context.SL_MOVIE_INFO.ToArray();

        model.SL_BOOK = JsonConvert.SerializeObject(sl_book, Formatting.Indented);
        model.SL_SHOW = JsonConvert.SerializeObject(sl_show, Formatting.Indented);
        model.SL_FRIEND = JsonConvert.SerializeObject(sl_friend, Formatting.Indented);
        model.SL_FRIEND_REQUEST = JsonConvert.SerializeObject(sl_friend_request, Formatting.Indented);
        model.SL_WATCHLIST = JsonConvert.SerializeObject(sl_watchlist, Formatting.Indented);
        model.SL_TRANSACTION = JsonConvert.SerializeObject(sl_transaction, Formatting.Indented);
        model.SL_USER_PREF = JsonConvert.SerializeObject(sl_user_pref, Formatting.Indented);
        model.OA_USERS = JsonConvert.SerializeObject(oa_users, Formatting.Indented);
        model.SL_TV_INFO = JsonConvert.SerializeObject(sl_tv_info, Formatting.Indented);
        model.SL_TV_EPISODE_INFO = JsonConvert.SerializeObject(sl_tv_episode_info, Formatting.Indented);
        model.SL_MOVIE_INFO = JsonConvert.SerializeObject(sl_movie_info, Formatting.Indented);

        return model;
    }

    public class SL_TV_INFO_BASIC
    {
        public int TV_INFO_ID { get; set; }

        public string SHOW_NAME { get; set; }

        public string SHOW_OVERVIEW { get; set; }

        public int? API_TYPE { get; set; }

        public string? API_ID { get; set; }

        public string? OTHER_NAMES { get; set; }

        public DateTime LAST_DATA_REFRESH { get; set; }

        public DateTime LAST_UPDATED { get; set; }

        public string? IMAGE_URL { get; set; }
    }

    public class SL_TV_EPISODE_INFO_BASIC
    {
        public int TV_EPISODE_INFO_ID { get; set; }

        public int TV_INFO_ID { get; set; }

        public int? API_TYPE { get; set; }

        public string? API_ID { get; set; }

        public string? SEASON_NAME { get; set; }

        public string? EPISODE_NAME { get; set; }

        public int? SEASON_NUMBER { get; set; }

        public int? EPISODE_NUMBER { get; set; }

        public string? EPISODE_OVERVIEW { get; set; }

        public int? RUNTIME { get; set; }

        public DateTime? AIR_DATE { get; set; }

        public string? IMAGE_URL { get; set; }
    }

    public class OA_USER_BASIC
    {
        public int USERID { get; set; }

        public string FIRSTNAME { get; set; }


        public string LASTNAME { get; set; }

        public string EMAIL { get; set; }
    }
}
