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
        Task<List<Debt>> GetAllDebtsDashboardAsync(DateTime? startDate, DateTime? endDate);
        Task UpdateDebt(Debt updatedDebt);
        void addDebt(Debt newDebt);
        decimal GetTotalPendingDebtDashboard(DateTime? startDate, DateTime? endDate);
        decimal GetTotalClearedDebtDashboard(DateTime? startDate,DateTime? endDate);
    }
}
