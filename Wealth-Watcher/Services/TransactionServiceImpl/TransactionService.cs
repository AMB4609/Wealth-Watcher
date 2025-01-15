using Newtonsoft.Json;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly string _filePath;
        private List<Transaction> transactions = new List<Transaction>();

        public TransactionService(string filePath)
        {
            _filePath = filePath;
            LoadTransactions();
        }
        private void LoadTransactions()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonData) ?? new List<Transaction>();
            }
        }

        public void AddTransaction(Transaction newTransaction)
        {
            if (transactions.Any())
            {
                newTransaction.transactionCode = transactions.Max(t => t.transactionCode) + 1;
            }
            else
            {
                newTransaction.transactionCode = 1;
            }
            transactions.Add(newTransaction);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(transactions, Formatting.Indented));
        }
        public List<Transaction> GetAllTransactionsAsync()
        {
            return transactions;
        }
        public decimal GetTotalInflows()
        {

            return transactions.Where(t => t.type.Equals("Inflow")).Sum(t => t.amount);
        }
        public void AdjustmentForClearedDebt(decimal amount)
        {
            var adjustmentTransaction = new Transaction
            {
                amount = amount,
                transactionDate = DateTime.Now,
                type = "Debt",
                title = "Adjustment for cleared debt"
            };
            AddTransaction(adjustmentTransaction);
        }
        public void SaveNoteForTransaction(int transactionCode, string note)
        {
            var transaction = transactions.FirstOrDefault(t => t.transactionCode == transactionCode);
            if (transaction != null)
            {
                transaction.note = note;
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(transactions, Formatting.Indented));
            }
        }

        public decimal GetTotalInflowsDashboard(DateTime? startDate, DateTime? endDate)
        {
            var filteredTransactions = transactions.Where(t => t.type.Equals("Inflow"));
            if (startDate.HasValue && endDate.HasValue)
            {
                filteredTransactions = filteredTransactions.Where(t => t.transactionDate >= startDate.Value && t.transactionDate <= endDate.Value);
            }
            return filteredTransactions.Sum(t => t.amount);
        }

        public decimal GetTotalOutflowsDashboard(DateTime? startDate, DateTime? endDate)
        {
            var filteredTransactions = transactions.Where(t => t.type.Equals("Outflow"));
            if (startDate.HasValue && endDate.HasValue)
            {
                filteredTransactions = filteredTransactions.Where(t => t.transactionDate >= startDate.Value && t.transactionDate <= endDate.Value);
            }
            return filteredTransactions.Sum(t => t.amount);
        }

        public async Task<List<Transaction>> GetTopTransactionsDashboardAsync(int count, bool highOrLow, DateTime? startDate, DateTime? endDate)
        {
            if (!transactions.Any())
            {
                await Task.Run(() => LoadTransactions());
            }
            IEnumerable<Transaction> filteredTransactions = transactions;
            if (startDate.HasValue && endDate.HasValue)
            {
                filteredTransactions = filteredTransactions.Where(t => t.transactionDate >= startDate.Value && t.transactionDate <= endDate.Value);
            }
            var sortedTransactions = highOrLow
                ? filteredTransactions.OrderByDescending(t => t.amount).Take(count).ToList()
                : filteredTransactions.OrderBy(t => t.amount).Take(count).ToList();

            return sortedTransactions;
        }

    }
}
