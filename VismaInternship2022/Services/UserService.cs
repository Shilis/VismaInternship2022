using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Data;
using VismaInternship2022.Models;

namespace VismaInternship2022.Services
{
    public class UserService : IUserService
    {
        public static User? ActiveUser { get; set; }

        private readonly IDataHandler _dataHandler;

        public UserService(IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
        }

        public void SaveUser(User user)
        {
            _dataHandler.Users.Add(user);
            _dataHandler.SaveUsersData();
        }

        public bool IsUsernameTaken(string username)
        {
            var user = _dataHandler.Users.FirstOrDefault(user => user.Username == username);
            if (user == null)
                return false;

            return true;
        }

        public User? UserExists(string username, string password)
        {
            var user = _dataHandler.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
                return null;

            return user;
        }
    }
}
