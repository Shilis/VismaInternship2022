using System;
using System.Collections.Generic;
using System.Globalization;
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
            _meetingsService = new MeetingService(dataHandler);
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
                Console.WriteLine(" 1 - View meetings \n " +
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
                        DisplayMeetings();
                        break;

                    case "2":
                        AddMeeting();
                        break;

                    case "3":
                        DeleteMeeting();
                        break;

                    case "4":
                        AddUserToMeeting();
                        break;

                    case "5":
                        RemoveUserFromMeeting();
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

                Console.WriteLine("Please enter meeting desciption");
                var description = Console.ReadLine();

                if (description == null)
                {
                    description = "";
                }

                Category category = ChooseCategory();

                Type type = ChooseType();

                DateTime startDate, endDate;

                while (true)
                {
                    Console.WriteLine("Please enter start date (format: yyyy-MM-dd HH:mm)");

                    var startDateString = Console.ReadLine();
                    if (DateTime.TryParse(startDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                    {
                        break;
                    }
                }
                while (true)
                {
                    Console.WriteLine("Please enter end date (format: yyyy-MM-dd HH:mm)");

                    var endDateString = Console.ReadLine();
                    if (DateTime.TryParse(endDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                    {
                        break;
                    }
                }

                Meeting meeting = new Meeting(new Random().Next(99999), name, description, category, type, startDate, endDate, null);
                _meetingsService.AddMeeting(meeting);
                Console.Clear();
                break;
            }

        }

        private void DisplayMeetings()
        {
            Console.WriteLine(" 1 - Show all meetings \n " +
                                "2 - Filter meetings \n ");

            Console.Write("Your choice: ");
            var selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    Console.Clear();
                    var meetings = _meetingsService.GetMeetings();
                    Console.WriteLine("Meetings\n");
                    PrintList(meetings);
                    break;

                case "2":
                    Console.Clear();
                    Filters();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Enter valid number 1-2\n");
                    break;
            }
        }

        public void DeleteMeeting()
        {
            Console.WriteLine("Please type id of the meeting you would like to delete");
            var meetingIdString = Console.ReadLine();

            bool success = int.TryParse(meetingIdString, out int meetingId);
            if (success && meetingId >= 0)
                _meetingsService.DeleteMeeting(meetingId);
            else
                Console.WriteLine("Please provide correct number");
        }

        public void AddUserToMeeting()
        {
            Console.Clear();
            Console.WriteLine("Please type id of the meeting you would like to add person");
            var meetingIdString = Console.ReadLine();
            bool success = int.TryParse(meetingIdString, out int meetingId);
            if (success && meetingId >= 0)
            {
                Console.WriteLine("Please provide username of the person");
                var username = Console.ReadLine();

                Console.WriteLine("Please enter end date (format: yyyy-MM-dd HH:mm)");

                var startDateString = Console.ReadLine();
                if (DateTime.TryParse(startDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate) && username != null)
                {
                    _meetingsService.AddPersonToMeeting(meetingId, username, startDate);
                }
                else
                {
                    Console.WriteLine("Wrong date");
                };
            }
            else
                Console.WriteLine("Please provide correct number");
        }

        private void Filters()
        {
            Console.WriteLine("How would you like filter meetings");
            Console.WriteLine("By:");

            Console.WriteLine(" 1 - Description \n " +
                                "2 - Responsible person \n " +
                                "3 - Category \n " +
                                "4 - Type \n " +
                                "5 - Dates \n " +
                                "6 - Number of attendees");
            var selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    Console.WriteLine("Please enter description");
                    var description = Console.ReadLine();
                    Console.Clear();
                    if (description != null)
                        PrintList(_meetingsService.FilterMeetingsByDescription(description));
                    break;

                case "2":
                    Console.WriteLine("Please enter the username of the person in charge");
                    var username = Console.ReadLine();
                    Console.Clear();
                    if (username != null)
                        PrintList(_meetingsService.FilterMeetingsByResponsiblePerson(username));
                    break;

                case "3":
                    Console.WriteLine("Please choose one of the categories");
                    Console.Clear();
                    PrintList(_meetingsService.FilterMeetingsByCategory(ChooseCategory()));
                    break;

                case "4":
                    Console.WriteLine("Please choose one of the types");
                    Console.Clear();
                    PrintList(_meetingsService.FilterMeetingsByType(ChooseType()));
                    break;

                case "5":
                    Console.WriteLine("1 - Between two dates\n" +
                                        "2 - Later than provided date\n" +
                                        "3 - Earlier than provided date\n");

                    var choice = Console.ReadLine();

                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            DateTime startDate, endDate;
                            while (true)
                            {
                                Console.WriteLine("Please enter start date (format: yyyy-MM-dd HH:mm)");

                                var startDateString = Console.ReadLine();
                                if (DateTime.TryParse(startDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                                {
                                    break;
                                }
                            }
                            while (true)
                            {
                                Console.WriteLine("Please enter end date (format: yyyy-MM-dd HH:mm)");

                                var endDateString = Console.ReadLine();
                                if (DateTime.TryParse(endDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                                {
                                    break;
                                }
                            }
                            PrintList(_meetingsService.FilterMeetingsByDates(startDate, endDate));
                            break;

                        case "2":
                            Console.Clear();
                            DateTime from;
                            while (true)
                            {
                                Console.WriteLine("Please enter date (format: yyyy-MM-dd HH:mm)");

                                var startDateString = Console.ReadLine();
                                if (DateTime.TryParse(startDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out from))
                                {
                                    break;
                                }
                            }
                            PrintList(_meetingsService.FilterMeetingsByDates(from));
                            break;

                        case "3":
                            Console.Clear();
                            DateTime to;
                            while (true)
                            {
                                Console.WriteLine("Please enter date (format: yyyy-MM-dd HH:mm)");

                                var startDateString = Console.ReadLine();
                                if (DateTime.TryParse(startDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out to))
                                {
                                    break;
                                }
                            }
                            PrintList(_meetingsService.FilterMeetingsByEndDate(to));
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Enter valid number 1-3\n");
                            break;
                    }
                    break;

                case "6":
                    Console.WriteLine("1 - Same count as provided number\n" +
                                        "2 - More than provided number\n" +
                                        "3 - Less than provided number\n");

                    var userChoice = Console.ReadLine();
                    Console.Clear();

                    switch (userChoice)
                    {
                        case "1":
                            while (true)
                            {
                                Console.WriteLine("Please enter number");

                                var numberString = Console.ReadLine();
                                bool success = int.TryParse(numberString, out int number);
                                if (success)
                                {
                                    PrintList(_meetingsService.FilterMeetingsByNumberOfAttendeesEqual(number));
                                    break;
                                }
                            }
                            break;

                        case "2":
                            while (true)
                            {
                                Console.WriteLine("Please enter number");

                                var numberString = Console.ReadLine();
                                bool success = int.TryParse(numberString, out int number);
                                if (success)
                                {
                                    PrintList(_meetingsService.FilterMeetingsByNumberOfAttendeesMore(number));
                                    break;
                                }
                            }
                            break;

                        case "3":
                            while (true)
                            {
                                Console.WriteLine("Please enter number");

                                var numberString = Console.ReadLine();
                                bool success = int.TryParse(numberString, out int number);
                                if (success)
                                {
                                    PrintList(_meetingsService.FilterMeetingsByNumberOfAttendeesLess(number));
                                    break;
                                }
                            }
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Enter valid number 1-3\n");
                            break;
                    }

                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Enter valid number 1-3\n");
                    break;
            }
        }

        private void RemoveUserFromMeeting()
        {
            Console.Clear();
            Console.WriteLine("Please type id of the meeting you would like to remove user");
            var meetingIdString = Console.ReadLine();
            bool success = int.TryParse(meetingIdString, out int meetingId);
            if (success && meetingId >= 0)
            {
                Console.WriteLine("Please provide username of the user");
                var username = Console.ReadLine();
                if (username != null)
                    _meetingsService.RemovePersonFromMeeting(meetingId, username);
            }
            else
                Console.WriteLine("Please provide correct number");
        }

        private void PrintList(IEnumerable<Meeting> meetings)
        {
            foreach (var meeting in meetings)
            {
                Console.WriteLine($"Id: {meeting.Id}\n Name: {meeting.Name}\n" +
                    $" Description: {meeting.Description}\n Type: {meeting.Type}\n Category: {meeting.Category}\n " +
                    $"StartDate: {meeting.StartDate}\n EndDate: {meeting.EndDate}\n " +
                    $"ResponsiblePerson: {meeting.ResponsiblePerson.Name} (username: {meeting.ResponsiblePerson.Username})\n");

                Console.WriteLine("Participants: ");
                foreach (var participant in meeting.MeetingParticipants)
                {
                    Console.WriteLine(participant.Name);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private Type ChooseType()
        {
            while (true)
            {
                Console.WriteLine("Please choose type");
                Console.WriteLine(" 1 - Live \n " +
                                    "2 - InPerson \n ");
                var selection = Console.ReadLine();

                switch (selection)
                {
                    case "1":
                        return Type.Live;

                    case "2":
                        return Type.InPerson;

                    default:
                        Console.Clear();
                        Console.WriteLine("Enter valid number 1-2\n");
                        break;
                }
                if (selection == "1" || selection == "2")
                {
                    break;
                }
            }
            return Type.Live;
        }

        private Category ChooseCategory()
        {
            while (true)
            {
                Console.WriteLine("Please choose category");
                Console.WriteLine(" 1 - CodeMonkey \n " +
                                    "2 - Hub \n " +
                                    "3 - Short \n " +
                                    "4 - TeamBuilding \n ");
                var selection = Console.ReadLine();

                switch (selection)
                {
                    case "1":
                        return Category.CodeMonkey;

                    case "2":
                        return Category.Hub;

                    case "3":
                        return Category.Short;

                    case "4":
                        return Category.TeamBuilding;

                    default:
                        Console.Clear();
                        Console.WriteLine("Enter valid number 1-4\n");
                        break;
                }
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
