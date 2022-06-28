using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Data;
using VismaInternship2022.Models;
using VismaInternship2022.Services;
using Type = VismaInternship2022.Models.Type;

namespace VismaInternship2022
{
    public class AppUI
    {
        private readonly IMeetingService _meetingsService;
        private readonly IUserService _userService;

        public AppUI(IDataHandler dataHandler)
        {
            dataHandler.LoadData();
            _meetingsService = new MeetingService();
            _userService = new UserService(dataHandler);
        }

        public void Start()
        {
            while (true)
            {
                Intro();

            }
        }

        private void Intro()
        {
            Console.WriteLine("Welcome to Visma's meeting system\n");

            Console.WriteLine(" " +
                    "1 - Register \n " +
                    "2 - Login \n " +
                    "3 - Meeting system \n " +
                    "4 - Logout\n");

            Console.Write("Your choice: ");
            var selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    if (UserService.ActiveUser != null)
                        Console.WriteLine("Can't register because you are already logged in \n");
                    else
                    {
                        Console.Clear();
                        Register();
                    }

                    break;

                case "2":
                    Console.Clear();
                    Login();
                    break;

                case "3":
                    if (UserService.ActiveUser == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Please login to proceed\n");
                    }
                    else
                    {
                        Console.Clear();
                        MeetingSystem();
                    }
                    break;

                case "4":
                    Console.Clear();
                    if (UserService.ActiveUser != null)
                    {
                        Console.WriteLine("Successful logout \n");
                        UserService.ActiveUser = null;
                    }
                    else
                        Console.WriteLine("You are not logged in \n");
                    return;

                default:
                    Console.Clear();
                    Console.WriteLine("Enter valid number 1-4\n");
                    break;
            }
        }

        private void Register()
        {
            while (true)
            {
                Console.WriteLine("Register form:\n");
                Console.WriteLine("Please enter username");

                var username = Console.ReadLine();

                if (CheckIfEmpty(username, "Username must contain atleast 1 character\n") || username == null)
                    continue;
                else if (_userService.IsUsernameTaken(username))
                {
                    Console.Clear();
                    Console.WriteLine("Username already exists\n");
                    continue;
                }

                Console.WriteLine("Please enter Name");
                var name = Console.ReadLine();

                if (CheckIfEmpty(name, "Name must contain atleast 1 character\n") || name == null)
                    continue;

                Console.WriteLine("Please enter password");
                var password = Console.ReadLine();

                if (CheckIfEmpty(password, "Password must contain atleast 1 character\n") || password == null)
                    continue;

                User user = new User(username, name, password);

                _userService.SaveUser(user);
                Console.Clear();
                break;
            }
        }
        private void Login()
        {
            while (true)
            {
                Console.WriteLine("Login form:\n");
                Console.WriteLine("Please enter username");

                var username = Console.ReadLine();

                Console.WriteLine("Please enter password");
                var password = Console.ReadLine();

                if (password == null || username == null)
                {
                    Console.Clear();
                    Console.WriteLine("Username or password can't be null!\n");
                    continue;
                }

                var user = _userService.UserExists(username, password);

                if (user == null)
                {
                    Console.Clear();
                    Console.WriteLine("Wrong username or password!\n");
                    continue;
                }

                Console.Clear();
                Console.WriteLine($"Welcome back, {user.Name}\n");
                UserService.ActiveUser = user;
                break;
            }
        }

        private void MeetingSystem()
        {
            while (true)
            {
                Console.WriteLine(  "1 - View meetings \n " +
                                    "2 - Add new meeting \n " +
                                    "3 - Delete meeting \n " +
                                    "4 - Add user to meeting \n " +
                                    "5 - Remove user from meeting \n " +
                                    "6 - Go back\n");

                Console.Write("Your choice: ");
                var selection = Console.ReadLine();

                switch (selection)
                {
                    case "1":
                        break;

                    case "2":
                        AddMeeting();
                        break;

                    case "3":
                        break;

                    case "4":
                        break;

                    case "5":
                        break;

                    case "6":
                        Console.Clear();
                        Start();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Enter valid number 1-6\n");
                        break;
                }
            }
        }

        private void AddMeeting()
        {
            while (true)
            {
                Console.WriteLine("Create meeting\n");
                Console.WriteLine("Please enter meeting name");

                var name = Console.ReadLine();

                if (CheckIfEmpty(name, "Name must contain atleast 1 character\n") || name == null)
                    continue;

                var description = Console.ReadLine();

                Category? category = null;
                while (category == null)
                {
                    Console.WriteLine("Please choose category");
                    Console.WriteLine(  "1 - CodeMonkey \n " +
                                        "2 - Hub \n " +
                                        "3 - Short \n " +
                                        "4 - TeamBuilding \n ");
                    var selection = Console.ReadLine();

                    switch (selection)
                    {
                        case "1":
                            category = Category.CodeMonkey;
                            break;

                        case "2":
                            category = Category.Hub;
                            break;

                        case "3":
                            category = Category.Short;
                            break;

                        case "4":
                            category = Category.TeamBuilding;
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Enter valid number 1-4\n");
                            break;
                    }
                }

                Type? type = null;
                while (type == null)
                {
                    Console.WriteLine("Please choose type");
                    Console.WriteLine(  "1 - Live \n " +
                                        "2 - InPerson \n " );
                    var selection = Console.ReadLine();

                    switch (selection)
                    {
                        case "1":
                            type = Type.Live;
                            break;

                        case "2":
                            type = Type.InPerson;
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Enter valid number 1-2\n");
                            break;
                    }
                }

                var startDate = new DateTime(2016, 7, 15, 3, 15, 0);
                var endDate = new DateTime(2016, 7, 15, 3, 15, 0);
            }

        }

        private bool CheckIfEmpty(string? text, string messageIfEmpty)
        {
            if (text != null)
            {
                string trimmedUsername = String.Concat(text.Where(c => !Char.IsWhiteSpace(c)));
                if (trimmedUsername.Length < 1)
                {
                    Console.Clear();
                    Console.WriteLine(messageIfEmpty);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
