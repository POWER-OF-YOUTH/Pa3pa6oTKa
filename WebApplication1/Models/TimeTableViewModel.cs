using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DatabaseModels;

namespace WebApplication1.Models
{
    public class TimeTableViewModel
    {
        public Dictionary<DateTime, Day> Days = new Dictionary<DateTime, Day>();
    }

    public class Day
    {
        public string Name;
        public DateTime Date;
        public List<Homework> HomeWorks = new List<Homework>();
        public List<Event> Events = new List<Event>();

        public Day(DateTime date)
        {
            Date = date;
        }

        public override bool Equals(object obj)
        {
            if (obj is Day)
                return Equals(obj as Day);
            return ReferenceEquals(obj, this);
        }

        public bool Equals(DateTime date)
        {
            return Date == date;
        }
    }

    public class Event
    {
        public int ID;
        public DateTime StartTime;
        public string Name;
        public string Description;
    }
}
