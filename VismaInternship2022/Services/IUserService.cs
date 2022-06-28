using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Models;

namespace VismaInternship2022.Services
{
    public interface IUserService
    {
        void SaveUser(User user);
        User? UserExists(string username, string password);
        bool IsUsernameTaken(string username);
    }
}
