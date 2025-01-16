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
    // Service for managing debt records
    public class DebtService : IDebtService
    {
        private readonly UserSessionService _userSessionService;
        private readonly string _filePath;
        private List<Debt> debts = new List<Debt>();

        // Constructor loads debts from a file on initialization
        public DebtService(string filePath,UserSessionService userSessionService)
        {
            _userSessionService = userSessionService;
            _filePath = filePath;
            LoadDebts();
        }

        // Loads debts from a JSON file
        public void LoadDebts()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                var allDebts = JsonConvert.DeserializeObject<List<Debt>>(jsonData) ?? new List<Debt>();
                int currentUserId = _userSessionService.GetCurrentUserId();
                debts = allDebts.Where(t => t.userId == currentUserId).ToList();
            }
            else
            {
                debts = new List<Debt>();
            }
        }

        // Adds a new debt to the collection and saves it to the file
        public void addDebt(Debt newDebt)
        {
            List<Debt> allDebts;

            // Load existing debts from the file, if it exists
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                allDebts = JsonConvert.DeserializeObject<List<Debt>>(jsonData) ?? new List<Debt>();
            }
            else
            {
                allDebts = new List<Debt>();
            }
            if (debts.Any())
            {
                newDebt.debtId = debts.Max(d => d.debtId) + 1;
            }
            else
            {
                newDebt.debtId = 1;
            }
            newDebt.debtId = allDebts.Any() ? allDebts.Max(d => d.debtId) + 1 : 1;
            newDebt.userId = _userSessionService.GetCurrentUserId();
            allDebts.Add(newDebt);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(allDebts, Formatting.Indented));
            LoadDebts(); // Refresh local debts list
        }

        // Returns all debts asynchronously
        public async Task<List<Debt>> GetAllDebtsAsync()
        {
            return await Task.FromResult(debts);
        }

        // Updates a specific debt and saves the change
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

        // Calculates the total of all cleared debts
        public decimal GetTotalClearedDebt()
        {
            return debts.Where(t => t.cleared.Equals(true)).Sum(t => t.price);
        }

        // Calculates the total of all pending debts
        public decimal GetTotalPendingDebt()
        {
            return debts.Where(t => t.cleared.Equals(false)).Sum(t => t.price);
        }

        // Retrieves all debts due within a specified date range for the dashboard
        public async Task<List<Debt>> GetAllDebtsDashboardAsync(DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<Debt> filteredDebts = debts;
            if (startDate.HasValue && endDate.HasValue)
            {
                filteredDebts = filteredDebts.Where(d => d.dueDate >= startDate.Value && d.dueDate <= endDate.Value);
            }
            return await Task.FromResult(filteredDebts.ToList());
        }

        // Calculates pending debts within a specified date range
        public decimal GetTotalPendingDebtDashboard(DateTime? startDate, DateTime? endDate)
        {
            var filteredDebts = debts.Where(d => !d.cleared);
            if (startDate.HasValue && endDate.HasValue)
            {
                filteredDebts = filteredDebts.Where(d => d.dueDate >= startDate.Value && d.dueDate <= endDate.Value);
            }
            return filteredDebts.Sum(d => d.price);
        }

        // Calculates cleared debts within a specified date range
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
