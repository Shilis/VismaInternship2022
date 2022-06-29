using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Data;
using VismaInternship2022.Models;
using VismaInternship2022.Services;

namespace VismaInternship2022.UnitTests
{
    public class UserServiceTests
    {
        private User _user;
        private IDataHandler _dataHandler;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _user = new User("test", "test", "test");
            _dataHandler = new FakeDataHandler();
            _dataHandler.Users.Add(_user);
            _userService = new UserService(_dataHandler);
        }

        [Test]
        public void UserExists_CorrectInformation_ReturnUser()
        {
            var result = _userService.UserExists(_user.Username, _user.Password);

            Assert.That(result, Is.EqualTo(_user));
        }

        [Test]
        public void UserExists_WrongInformation_ReturnNull()
        {
            var result = _userService.UserExists("", "");

            Assert.IsNull(result);
        }

        [Test]
        public void SaveUser_WhenCalled_AddUserToList()
        {
            var newUser = new User("test2", "test2", "test2");

            _userService.SaveUser(newUser);

            Assert.IsTrue(_dataHandler.Users.Contains(newUser));
        }

        [Test]
        public void IsUsernameTaken_Taken_ReturnTrue()
        {
            var result = _userService.IsUsernameTaken("test");

            Assert.IsTrue(result);
        }

        [Test]
        public void IsUsernameTaken_NotTaken_ReturnFalse()
        {
            var result = _userService.IsUsernameTaken("test2");

            Assert.IsFalse(result);
        }
    }
}
