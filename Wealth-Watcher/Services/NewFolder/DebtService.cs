using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;


namespace Wealth_Watcher.Services.NewFolder
{

    public class DebtService : IDebtService
    {
        private readonly string _filePath;
        private List<Debt> debts = new List<Debt>();
        public DebtService(string filePath)
        {
            _filePath = filePath;
            LoadUsers();
        }

        public void LoadUsers()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                debts = JsonConvert.DeserializeObject<List<Debt>>(jsonData) ?? new List<Debt>();
            }
        }
        public void addDebt(Debt newDebt)
        {
            if (debts.Any())
            {
                newDebt.debtId = debts.Max(d => d.debtId) + 1;
            }
            else
            {
                newDebt.debtId = 1;
            }
            debts.Add(newDebt); 
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(debts));
        }
        public async Task<List<Debt>> GetAllDebtsAsync()
        {
            return await Task.FromResult(debts);
        }

        public async Task UpdateDebt(Debt updatedDebt)
        {
            var index = debts.FindIndex(d => d.debtId == updatedDebt.debtId);
            if (index != -1)
            {
                debts[index] = updatedDebt;
                var jsonData = JsonConvert.SerializeObject(debts, Formatting.Indented);
                await File.WriteAllTextAsync(_filePath, jsonData);
            }
        }
        public decimal GetTotalClearedDebt()
        {

            return debts.Where(t => t.cleared.Equals(true)).Sum(t => t.price);
        }

        public decimal GetTotalPendingDebt()
        {
            return debts.Where(t => t.cleared.Equals(false)).Sum(t => t.price);
        }

        public async Task<List<Debt>> GetAllDebtsDashboardAsync(DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<Debt> filteredDebts = debts;
            if (startDate.HasValue && endDate.HasValue)
            {
                filteredDebts = filteredDebts.Where(d => d.dueDate >= startDate.Value && d.dueDate <= endDate.Value);
            }
            return await Task.FromResult(filteredDebts.ToList());
        }

        public decimal GetTotalPendingDebtDashboard(DateTime? startDate, DateTime? endDate)
        {
            var filteredDebts = debts.Where(d => !d.cleared);
            if (startDate.HasValue && endDate.HasValue)
            {
                filteredDebts = filteredDebts.Where(d => d.dueDate >= startDate.Value && d.dueDate <= endDate.Value);
            }
            return filteredDebts.Sum(d => d.price);
        }
        public decimal GetTotalClearedDebtDashboard(DateTime? startDate, DateTime? endDate)
        {
            var filteredDebts = debts.Where(d => d.cleared);
            if (startDate.HasValue && endDate.HasValue)
            {
                filteredDebts = filteredDebts.Where(d => d.dueDate >= startDate.Value && d.dueDate <= endDate.Value);
            }
            return filteredDebts.Sum(d => d.price);
        }

    }
}
