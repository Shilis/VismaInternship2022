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

        public FileHandler()
        {
            Meetings = new List<Meeting>();
            Users = new List<User>();
        }

        public void LoadData()
        {
            if (!File.Exists("users.txt"))
                File.Create("users.txt").Dispose();

            var users = File.ReadAllText("users.txt");
            var deserializedUsers = JsonConvert.DeserializeObject<List<User>>(users);

            if (deserializedUsers != null)
                Users = deserializedUsers;

            if (!File.Exists("meetings.txt"))
                File.Create("meetings.txt").Dispose();

            var meetings = File.ReadAllText("meetings.txt");
            var deserializedMeetings = JsonConvert.DeserializeObject<List<Meeting>>(meetings);

            if (deserializedMeetings != null)
                Meetings = deserializedMeetings;
        }

        public void SaveMeetingsData()
        {
            var serializedMeetings = JsonConvert.SerializeObject(Meetings);
            File.WriteAllText("meetings.txt", serializedMeetings);
        }

        public void SaveUsersData()
        {
            var serializedUsers = JsonConvert.SerializeObject(Users);
            File.WriteAllText("users.txt", serializedUsers);
        }
    }
}
