using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services
{
    // Defines interface for user management functionalities
    public interface IUserService
    {
        void UpdateBalanceTransaction(Transaction newTransaction);
        void UpdateBalanceDebt(Debt Debt);
        
        decimal GetCurrentBalanceDashboard();
        User HandleRegistration(User user);

        bool Login(User loginUser);
    }
}
