using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Models;

namespace VismaInternship2022.Data
{
    public class FileHandler: IDataHandler
    {
        public ICollection<Meeting> Meetings { get; set; }
        public ICollection<User> Users { get; set;}

        private string meetingsFile = "meetings.json";
        private string usersFile = "users.json";

        public FileHandler()
        {
            Meetings = new List<Meeting>();
            Users = new List<User>();
        }

        public void LoadData()
        {
            if (!File.Exists(usersFile))
                File.Create(usersFile).Dispose();

            var users = File.ReadAllText(usersFile);
            var deserializedUsers = JsonConvert.DeserializeObject<List<User>>(users);

            if (deserializedUsers != null)
                Users = deserializedUsers;

            if (!File.Exists(meetingsFile))
                File.Create(meetingsFile).Dispose();

            var meetings = File.ReadAllText(meetingsFile);
            var deserializedMeetings = JsonConvert.DeserializeObject<List<Meeting>>(meetings);

            if (deserializedMeetings != null)
                Meetings = deserializedMeetings;
        }

        public void SaveMeetingsData()
        {
            var serializedMeetings = JsonConvert.SerializeObject(Meetings);
            File.WriteAllText(meetingsFile, serializedMeetings);
        }

        public void SaveUsersData()
        {
            var serializedUsers = JsonConvert.SerializeObject(Users);
            File.WriteAllText(usersFile, serializedUsers);
        }
    }
}
