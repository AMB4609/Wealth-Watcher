using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services.UserServiceImpl.cs
{
    // Service handling user management based on a local file
    public class UserService : IUserService
    {
        private List<User> users = new List<User>();
        private readonly string _filePath;
        private UserSessionService _userSessionService;

        public UserService(string filePath, UserSessionService userSessionService)
        {
            _filePath = filePath; // Set the path to the user data file
            LoadUsers(); // Load users from file
            _userSessionService = userSessionService;
        }

        // Load user data from the specified file path
        public void LoadUsers()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                users = JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
            }
        }

        // Register a new user and return a new user instance
        public User HandleRegistration(User newUser)
        {
            if (users.Any())
            {
                newUser.userId = users.Max(u => u.userId) + 1;
            }
            else
            {
                newUser.userId = 1;
            }
            users.Add(newUser);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(users));
            newUser = new User(); // Clear user data after registration
            return newUser;
        }

        // Authenticate and log in a user
        public bool Login(User loginUser)
        {
            var user = users.FirstOrDefault(u => u.userName == loginUser.userName && u.password == loginUser.password);
            if (user != null)
            {
                _userSessionService.SetUser(user); // Set the session to the logged in user
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpdateBalanceTransaction(Transaction Transaction)
        {
            var existingUser = users.FirstOrDefault(u => u.userId == _userSessionService.GetCurrentUserId());
            if (existingUser != null)
            {
                if (Transaction.type == "Inflow")
                {
                    existingUser.balance += Transaction.amount;
                }
                else
                {
                    existingUser.balance -= Transaction.amount;
                }
            }
            // Write the updated list of users back to the file
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(users, Formatting.Indented));
        }

        public void UpdateBalanceDebt(Debt debt)
        {
            var existingUser = users.FirstOrDefault(u => u.userId == _userSessionService.GetCurrentUserId());
            if (existingUser != null) {
                existingUser.balance -= debt.price;
            }
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(users, Formatting.Indented));
        }
        public decimal GetCurrentBalanceDashboard()
        {
            var existingUser = users.FirstOrDefault(u => u.userId == _userSessionService.GetCurrentUserId());
            if (existingUser != null)
            {
                return existingUser.balance;
            }
            return 0;
        }
    }
}

