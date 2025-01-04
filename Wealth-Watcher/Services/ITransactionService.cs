using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services
{
    public interface ITransactionService
    {
        void AddTransaction(Transaction newTransaction);
        List<Transaction> GetAllTransactionsAsync();
        decimal GetTotalInflows();
        decimal GetTotalOutflows();

        void AdjustInflowForClearedDebt(decimal amount);
        void AdjustOutflowForClearedDebt(decimal amount);
    }
}
