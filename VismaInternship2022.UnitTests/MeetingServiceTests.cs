using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Data;
using VismaInternship2022.Models;
using VismaInternship2022.Services;
using Type = VismaInternship2022.Models.Type;

namespace VismaInternship2022.UnitTests
{
    public class MeetingServiceTests
    {
        private User _user;
        private IDataHandler _dataHandler;
        private MeetingService _meetingService;

        [SetUp]
        public void Setup()
        {
            _user = new User("test", "test", "test");
            UserService.ActiveUser = _user;
            _dataHandler = new FakeDataHandler();
            _meetingService = new MeetingService(_dataHandler);
        }

        [Test]
        public void AddMeeting_WhenActiveUserIsSet_AddMeetingToList()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);

            _meetingService.AddMeeting(meeting);

            Assert.That(_dataHandler.Meetings, Is.EquivalentTo(new List<Meeting> {meeting}));
        }

        [Test]
        public void AddMeeting_WhenActiveUserIsNull_MeetingListRemainTheSame()
        {
            UserService.ActiveUser = null;
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);

            _meetingService.AddMeeting(meeting);

            Assert.That(_dataHandler.Meetings, Is.EquivalentTo(new List<Meeting> {}));
        }

        [Test]
        public void DeleteMeeting_ByOwner_ReturnTrue()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            _dataHandler.Meetings.Add(meeting);
            UserService.ActiveUser = _user;

            var result = _meetingService.DeleteMeeting(meeting.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteMeeting_ByAnotherUser_ReturnFalse()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            _dataHandler.Meetings.Add(meeting);
            UserService.ActiveUser = new User("test2", "test2", "test2");

            var result = _meetingService.DeleteMeeting(meeting.Id);

            Assert.IsFalse(result);
        }

        [Test]
        public void AddPersonToMeeting_UserIsNotBusy_UserAddedToMeetingParticipantsList()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            _dataHandler.Meetings.Add(meeting);
            _dataHandler.Users.Add(_user);

            _meetingService.AddPersonToMeeting(meeting.Id, _user.Username, new DateTime(2022, 7, 10, 13, 0, 0));

            Assert.That(meeting.MeetingParticipants, Is.EquivalentTo(new List<User> { _user }));
        }

        [Test]
        public void AddPersonToMeeting_UserAlreadyInTheMeeting_MeetingParticipantsListRemainUnchanged()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            meeting.MeetingParticipants.Add(_user);
            _dataHandler.Meetings.Add(meeting);
            _dataHandler.Users.Add(_user);

            _meetingService.AddPersonToMeeting(meeting.Id, _user.Username, new DateTime(2022, 7, 10, 13, 0, 0));

            Assert.That(meeting.MeetingParticipants, Is.EquivalentTo(new List<User> { _user }));
        }

        [Test]
        public void AddPersonToMeeting_ProvidedDateTimeNotInMeetingInterval_MeetingParticipantsListRemainUnchanged()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            _dataHandler.Meetings.Add(meeting);
            _dataHandler.Users.Add(_user);

            _meetingService.AddPersonToMeeting(meeting.Id, _user.Username, new DateTime(2021, 4, 10, 8, 0, 0));

            Assert.That(meeting.MeetingParticipants, Is.EquivalentTo(new List<User> { }));
        }

        [Test]
        public void AddPersonToMeeting_UserAlreadyInMeetingWhichIntersects_MeetingParticipantsListRemainUnchanged()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            var meeting2 = new Meeting(2, "test2", "test2", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            meeting.MeetingParticipants.Add(_user);
            _dataHandler.Meetings.Add(meeting);
            _dataHandler.Meetings.Add(meeting2);
            _dataHandler.Users.Add(_user);

            _meetingService.AddPersonToMeeting(meeting2.Id, _user.Username, new DateTime(2022, 7, 10, 14, 0, 0));

            Assert.That(meeting2.MeetingParticipants, Is.EquivalentTo(new List<User> {  }));
        }

        [Test]
        public void RemovePersonFromMeeting_UserIsOwnerOfTheMeeting_MeetingParticipantsListRemainUnchanged()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            meeting.MeetingParticipants.Add(_user);
            _dataHandler.Meetings.Add(meeting);
            _dataHandler.Users.Add(_user);

            _meetingService.RemovePersonFromMeeting(meeting.Id, _user.Username);

            Assert.That(meeting.MeetingParticipants, Is.EquivalentTo(new List<User> {_user}));
        }

        [Test]
        public void RemovePersonFromMeeting_UserExist_RemoveUserFromParticipantsList()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            var newUser = new User("a", "a", "a");
            meeting.MeetingParticipants.Add(newUser);
            _dataHandler.Meetings.Add(meeting);
            _dataHandler.Users.Add(newUser);

            _meetingService.RemovePersonFromMeeting(meeting.Id, newUser.Username);

            Assert.That(meeting.MeetingParticipants, Is.EquivalentTo(new List<User> { }));
        }

        [Test]
        public void RemovePersonFromMeeting_UserDoesNotExist_RemoveUserFromParticipantsList()
        {
            var meeting = new Meeting(1, "test", "test", Category.CodeMonkey, Type.Live, new DateTime(2022, 7, 10, 12, 0, 0), new System.DateTime(2022, 7, 10, 15, 0, 0), _user);
            _dataHandler.Meetings.Add(meeting);

            _meetingService.RemovePersonFromMeeting(meeting.Id, new User("a", "a", "a").Username);

            Assert.That(meeting.MeetingParticipants, Is.EquivalentTo(new List<User> { }));
        }
    }
}
