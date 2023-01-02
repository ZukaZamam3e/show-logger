using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories.Interfaces;
public interface IUserRepository
{
    string GetDefaultArea(int userId);
    bool SetDefaultArea(int userId, string area);
}
