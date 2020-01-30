using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUilities.UserGameDelegates
{
    public delegate string PrintUserHandler(Users users);
    public delegate void UserOnlineHandler(Users users);
    public delegate void UserOfflineHandler(Users user);
}
