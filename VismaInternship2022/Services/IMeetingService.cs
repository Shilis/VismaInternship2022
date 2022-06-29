using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Models;
using Type = VismaInternship2022.Models.Type;

namespace VismaInternship2022.Services
{
    public interface IMeetingService
    {
        void AddMeeting(Meeting meeting);
        IEnumerable<Meeting> GetMeetings();
        bool DeleteMeeting(int meetingId);
        void AddPersonToMeeting(int meetingId, string username, DateTime when);
        void RemovePersonFromMeeting(int meetingId, string username);
        IEnumerable<Meeting> FilterMeetingsByDescription(string description);
        IEnumerable<Meeting> FilterMeetingsByResponsiblePerson(string username);
        IEnumerable<Meeting> FilterMeetingsByCategory(Category category);
        IEnumerable<Meeting> FilterMeetingsByType(Type type);
        IEnumerable<Meeting> FilterMeetingsByDates(DateTime from, DateTime to);
        IEnumerable<Meeting> FilterMeetingsByDates(DateTime from);
        IEnumerable<Meeting> FilterMeetingsByEndDate(DateTime to);
        IEnumerable<Meeting> FilterMeetingsByNumberOfAttendeesEqual(int count);
        IEnumerable<Meeting> FilterMeetingsByNumberOfAttendeesMore(int count);
        IEnumerable<Meeting> FilterMeetingsByNumberOfAttendeesLess(int count);
    }
}
