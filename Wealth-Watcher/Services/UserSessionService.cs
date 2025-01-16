using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services
{
    // Manages the session state of the current user
    public class UserSessionService
    {
        public required User CurrentUser { get; set; }

        public void SetUser(User user)
        {
            CurrentUser = user;
        }
        public int GetCurrentUserId()
        {
            return CurrentUser?.userId ?? 0; 
        }
    }

}
