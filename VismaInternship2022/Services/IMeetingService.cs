using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Models;

namespace VismaInternship2022.Services
{
    public interface IMeetingService
    {
        void AddMeeting(Meeting meeting);
        IEnumerable<Meeting> GetMeetings(int meetingId);
        void DeleteMeeting(int meetingId);
        void AddPersonToMeeting(int meetingId, User person);
        void RemovePersonFromMeeting(int meetingId, User person);
    }
}
