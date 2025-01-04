using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services
{
    public interface IDebtService
    {
        Task<List<Debt>> GetAllDebtsAsync();
        Task UpdateDebt(Debt updatedDebt);
        void addDebt(Debt newDebt);
        decimal GetTotalPendingDebt();
        decimal GetTotalClearedDebt();
    }
}
