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

        public decimal GetTotalOutflows()
        {
            return transactions.Where(t => t.type.Equals("Outflow")).Sum(t => t.amount);
        }
        public void AdjustInflowForClearedDebt(decimal amount)
        {
            var adjustmentTransaction = new Transaction
            {
                amount = -amount, 
                transactionDate = DateTime.Now,
                type = "Inflow",
                title = "Adjustment for cleared debt"
            };
            AddTransaction(adjustmentTransaction);
        }

        public void AdjustOutflowForClearedDebt(decimal amount)
        {
            var adjustmentTransaction = new Transaction
            {
                amount = amount,
                transactionDate = DateTime.Now,
                type = "Outflow",
                title = "Adjustment for cleared debt"
            };
            AddTransaction(adjustmentTransaction);
        }

    }
}
