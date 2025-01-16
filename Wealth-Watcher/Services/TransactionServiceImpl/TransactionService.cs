using Newtonsoft.Json;
using Wealth_Watcher.Model;
namespace Wealth_Watcher.Services
{
    // Service class for managing transaction data
    public class TransactionService : ITransactionService
    {
        private readonly string _filePath;
        private readonly UserSessionService _userSessionService;
        private List<Transaction> transactions = new List<Transaction>();

        // Constructor initializes services and loads transactions for the current user
        public TransactionService(UserSessionService userSessionService, string filePath)
        {
            _userSessionService = userSessionService;
            _filePath = filePath;
            LoadTransactions();
        }

        // Loads user-specific transactions from a JSON file
        private void LoadTransactions()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                var allTransactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonData) ?? new List<Transaction>();
                int currentUserId = _userSessionService.GetCurrentUserId();
                transactions = allTransactions.Where(t => t.userId == currentUserId).ToList();
            }
            else
            {
                transactions = new List<Transaction>(); // Initialize with empty list if no file found
            }
        }

        // Adds a new transaction for the current user and updates the JSON file
        public void AddTransaction(Transaction newTransaction)
        {
            List<Transaction> allTransactions;
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                allTransactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonData) ?? new List<Transaction>();
            }
            else
            {
                allTransactions = new List<Transaction>();
            }
            newTransaction.transactionCode = allTransactions.Any() ? allTransactions.Max(t => t.transactionCode) + 1 : 1;
            newTransaction.userId = _userSessionService.GetCurrentUserId();
            allTransactions.Add(newTransaction);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(allTransactions, Formatting.Indented));
            LoadTransactions(); // Refresh local transaction list
        }

        // Returns all transactions for the current user
        public List<Transaction> GetAllTransactionsAsync()
        {
            return transactions;
        }

        // Calculates the total amount of inflows (e.g., deposits)
        public decimal GetTotalInflows()
        {
            return transactions.Where(t => t.type.Equals("Inflow")).Sum(t => t.amount);
        }

        // Records an adjustment transaction for a cleared debt
        public void AdjustmentForClearedDebt(decimal amount)
        {
            var adjustmentTransaction = new Transaction { amount = amount, transactionDate = DateTime.Now, type = "Debt", title = "Adjustment for cleared debt" };
            AddTransaction(adjustmentTransaction);
        }

        // Updates the note for a specific transaction
        public void SaveNoteForTransaction(int transactionCode, string note)
        {
            var transaction = transactions.FirstOrDefault(t => t.transactionCode == transactionCode);
            if (transaction != null)
            {
                transaction.note = note;
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(transactions, Formatting.Indented)); // Save updated transactions to file
            }
        }

        // Calculates total inflows within a specified date range for dashboard display
        public decimal GetTotalInflowsDashboard(DateTime? startDate, DateTime? endDate)
        {
            return transactions
                .Where(t => t.type.Equals("Inflow") && t.transactionDate >= startDate && t.transactionDate <= endDate)
                .Sum(t => t.amount);
        }

        // Calculates total outflows within a specified date range for dashboard display
        public decimal GetTotalOutflowsDashboard(DateTime? startDate, DateTime? endDate)
        {
            return transactions
                .Where(t => t.type.Equals("Outflow") && t.transactionDate >= startDate && t.transactionDate <= endDate)
                .Sum(t => t.amount);
        }

        // Retrieves top transactions by amount for the dashboard, sorted by magnitude and filtered by date
        public async Task<List<Transaction>> GetTopTransactionsDashboardAsync(int count, bool highOrLow, DateTime? startDate, DateTime? endDate)
        {
            var filteredTransactions = transactions.Where(t => t.transactionDate >= startDate && t.transactionDate <= endDate);
            var sortedTransactions = highOrLow ? filteredTransactions.OrderByDescending(t => t.amount).Take(count) : filteredTransactions.OrderBy(t => t.amount).Take(count);
            return await Task.FromResult(sortedTransactions.ToList());
        }
    }
}
