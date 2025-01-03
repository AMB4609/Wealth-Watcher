using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wealth_Watcher.Model;

namespace Wealth_Watcher.Services.UserServiceImpl.cs
{
    public class UserService : IUserService
    {
        private List<User> users = new List<User>();
        private readonly string _filePath;

        public UserService(string filePath)
        {
            _filePath = filePath;
            LoadUsers();
        }

        public void LoadUsers()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                users = JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
            }
        }

        public User HandleRegistration(User newUser)
        {
            users.Add(newUser);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(users));
            newUser = new User();
            return newUser;
           // Reset the form
        }

        public bool Login(User loginUser)
        {
            var isValidUser = users.Any(u => u.userName == loginUser.userName && u.password == loginUser.password);
            if (isValidUser)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
