using ShowLogger.Data.Context;
using ShowLogger.Data.Entities;
using ShowLogger.Store.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories;
public class UserRepository : IUserRepository
{
    private readonly ShowLoggerDbContext _context;

    public UserRepository(ShowLoggerDbContext context)
    {
        _context = context;
    }

    public string GetDefaultArea(int userId)
    {
        string area = "";

        SL_USER_PREF entity = _context.SL_USER_PREF.FirstOrDefault(m => m.USER_ID == userId);

        if(entity != null)
        {
            area = entity.DEFAULT_AREA;
        }

        return area;
    }

    public bool SetDefaultArea(int userId, string area)
    {
        bool successful = false;

        SL_USER_PREF entity = _context.SL_USER_PREF.FirstOrDefault(m => m.USER_ID == userId);

        if (entity == null)
        {
            entity = new SL_USER_PREF
            {
                USER_ID = userId,
            };
            _context.SL_USER_PREF.Add(entity);
        }

        entity.DEFAULT_AREA = area;

        _context.SaveChanges();

        successful = true;

        return successful;
    }
}
