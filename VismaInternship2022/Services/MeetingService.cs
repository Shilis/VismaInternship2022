using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Data;
using VismaInternship2022.Models;
using Type = VismaInternship2022.Models.Type;

namespace VismaInternship2022.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IDataHandler _dataHandler;

        public MeetingService(IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
        }

        public void AddMeeting(Meeting meeting)
        {
            if (UserService.ActiveUser != null)
            {
                meeting.MeetingParticipants.Add(UserService.ActiveUser);
                _dataHandler.Meetings.Add(meeting);
                _dataHandler.SaveMeetingsData();
            }
        }

        public void AddPersonToMeeting(int meetingId, string username, DateTime when)
        {
            var meeting = _dataHandler.Meetings.FirstOrDefault(meet => meet.Id == meetingId);
            var user = _dataHandler.Users.FirstOrDefault(u => u.Username == username);

            if (meeting != null && user != null)
            {
                if(!(meeting.StartDate <= when && when <= meeting.EndDate))
                {
                    Console.WriteLine("Provided date time is not in meeting time interval");
                    return;
                }

                if (_dataHandler.Meetings.Any(meet => meet.MeetingParticipants.Any(par => par.Username == user.Username) &&
                        ((meet.StartDate < when && meet.EndDate > when) || (meet.StartDate < meeting.EndDate && meet.EndDate > meeting.EndDate))
                        ||
                        ((when < meet.StartDate && meeting.EndDate > meet.StartDate) || (when < meet.EndDate && meeting.EndDate > meet.EndDate))
                        )
                    )
                {
                    Console.WriteLine("WARNING: person is already in a meeting which intersects with this meeting");
                    return;
                }

                if (!meeting.MeetingParticipants.Any(u => u == user))
                {
                    meeting.MeetingParticipants.Add(user);
                    _dataHandler.SaveMeetingsData();
                    Console.WriteLine("User successfully added");
                    return;
                }
                Console.WriteLine("User is already added to the meeting");
                return;
            }
            Console.WriteLine($"Meeting with {meetingId} id or user with {username} username doesn't exist!");
        }

        public void RemovePersonFromMeeting(int meetingId, string username)
        {
            var meeting = _dataHandler.Meetings.FirstOrDefault(meet => meet.Id == meetingId);
            var user = _dataHandler.Users.FirstOrDefault(u => u.Username == username);

            if (meeting != null && user != null)
            {
                if(meeting.ResponsiblePerson.Username != user.Username && meeting.MeetingParticipants.Any(par => par.Username == user.Username))
                {
                    var userToRemove = meeting.MeetingParticipants.FirstOrDefault(u => u.Username == user.Username);
                    if(userToRemove != null)
                    {
                        meeting.MeetingParticipants.Remove(userToRemove);
                        _dataHandler.SaveMeetingsData();
                        Console.WriteLine("User succesfully removed");
                    }
                    
                }
                else
                {
                    Console.WriteLine($"User is owner of the meeting or there aren't user with username {username}");
                }
            }
            Console.WriteLine($"Meeting with {meetingId} id or user with {username} username doesn't exist!");
        }

        public bool DeleteMeeting(int meetingId)
        {
            var meeting = _dataHandler.Meetings.FirstOrDefault(meet => meet.Id == meetingId);
            if (meeting != null && meeting.ResponsiblePerson.Username == UserService.ActiveUser?.Username)
            {
                _dataHandler.Meetings.Remove(meeting);
                _dataHandler.SaveMeetingsData();
                Console.WriteLine("Meeting successfully deleted");
                return true;
            }
            Console.WriteLine($"Meeting with {meetingId} id doesn't exist or you don't have permission to do that!");
            return false;
        }

        public IEnumerable<Meeting> GetMeetings()
        {
            return _dataHandler.Meetings;
        }

        public IEnumerable<Meeting> FilterMeetingsByDescription(string description)
        {
            return _dataHandler.Meetings.Where(meet => meet.Description.ToLower().Contains(description.ToLower()));
        }

        public IEnumerable<Meeting> FilterMeetingsByResponsiblePerson(string username)
        {
            return _dataHandler.Meetings.Where(meet => meet.ResponsiblePerson.Username == username);
        }

        public IEnumerable<Meeting> FilterMeetingsByCategory(Category category)
        {
            return _dataHandler.Meetings.Where(meet => meet.Category == category);
        }

        public IEnumerable<Meeting> FilterMeetingsByType(Type type)
        {
            return _dataHandler.Meetings.Where(meet => meet.Type == type);
        }

        public IEnumerable<Meeting> FilterMeetingsByDates(DateTime from, DateTime to)
        {
            return _dataHandler.Meetings.Where(meet => DateTime.Compare(meet.StartDate, from) > 0 && DateTime.Compare(meet.EndDate, to) < 0);
        }

        public IEnumerable<Meeting> FilterMeetingsByDates(DateTime from)
        {
            return _dataHandler.Meetings.Where(meet => DateTime.Compare(meet.StartDate, from) > 0);
        }

        public IEnumerable<Meeting> FilterMeetingsByEndDate(DateTime to)
        {
            return _dataHandler.Meetings.Where(meet => DateTime.Compare(meet.EndDate, to) < 0);
        }

        public IEnumerable<Meeting> FilterMeetingsByNumberOfAttendeesEqual(int count)
        {
            return _dataHandler.Meetings.Where(meet => meet.MeetingParticipants.Count() == count);
        }

        public IEnumerable<Meeting> FilterMeetingsByNumberOfAttendeesMore(int count)
        {
            return _dataHandler.Meetings.Where(meet => meet.MeetingParticipants.Count() > count);
        }
        public IEnumerable<Meeting> FilterMeetingsByNumberOfAttendeesLess(int count)
        {
            return _dataHandler.Meetings.Where(meet => meet.MeetingParticipants.Count() < count);
        }
    }
}
