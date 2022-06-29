using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternship2022.Services;

namespace VismaInternship2022.Models
{
    public enum Category
    {
        CodeMonkey,
        Hub,
        Short,
        TeamBuilding
    }

    public enum Type
    {
        Live,
        InPerson
    }

    public class Meeting
    {
        public Meeting(int id, string name, string description, Category category,
                        Type type, DateTime startDate, DateTime endDate, User responsiblePerson)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            MeetingParticipants = new List<User>();
            ResponsiblePerson = UserService.ActiveUser ?? responsiblePerson;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public User ResponsiblePerson { get; set; }
        public string Description { get; private set; }
        public Category Category { get; private set; }
        public Type Type { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public ICollection<User> MeetingParticipants { get; private set; }
    }
}
