using ShowLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories.Interfaces;
public interface IFriendRepository
{
    IEnumerable<FriendModel> GetFriends(int userId);

    bool SendFriendRequest(int userId, int friendId);

    bool AcceptFriendRequest(int userId, int friendRequestId);

    bool DenyFriendRequest(int userId, int friendRequestId);

    bool DeleteFriend(int userId, int friendId);
}
