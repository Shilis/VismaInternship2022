using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Data;
using VismaInternship2022.Models;

namespace VismaInternship2022.UnitTests
{
    public class FakeDataHandler : IDataHandler
    {
        public ICollection<Meeting> Meetings { get ; set; }
        public ICollection<User> Users { get; set; }

        public FakeDataHandler()
        {
            Meetings = new List<Meeting>();
            Users = new List<User>();
        }

        public void LoadData()
        {
        }

        public void SaveMeetingsData()
        {
        }

        public void SaveUsersData()
        {
        }
    }
}
