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
    }
}
