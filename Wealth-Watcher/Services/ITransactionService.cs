using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services
{
    // Interface for managing financial transactions
    public interface ITransactionService
    {
        // abstract methods
        void AddTransaction(Transaction newTransaction);
        List<Transaction> GetAllTransactionsAsync();
        decimal GetTotalInflows();
        decimal GetTotalInflowsDashboard(DateTime? startDate,DateTime? endDate);
        decimal GetTotalOutflowsDashboard(DateTime? startDate, DateTime? endDate);
        void AdjustmentForClearedDebt(decimal amount);
        void SaveNoteForTransaction(int transaction, String note);
        Task<List<Transaction>> GetTopTransactionsDashboardAsync(int count, bool highOrLow,DateTime? startDate,DateTime? endDate);
    }
}
