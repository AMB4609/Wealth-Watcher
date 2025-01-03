using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services
{
    public interface IUserService
    {
        User HandleRegistration(User newUser);
         bool Login(User loginUser);
    }
}
