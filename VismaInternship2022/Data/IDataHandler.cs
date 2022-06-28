using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Models;

namespace VismaInternship2022.Data
{
    public interface IDataHandler
    {
        ICollection<Meeting> Meetings { get; set; }
        ICollection<User> Users { get; set; }
        void LoadData();
        void SaveMeetingsData();
        void SaveUsersData();
    }
}
