using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DatabaseModels;

namespace WebApplication1.Models
{
    public class TimeTableViewModel
    {
        public List<Day> Days = new List<Day>();
    }

    public class Day
    {
        public string Name;
        public DateTime Date;
        public List<Homework> HomeWorks = new List<Homework>();
        public List<Event> Events = new List<Event>();
    }

    public class Event
    {
        public int ID;
        public DateTime StartTime;
        public string Name;
        public string Description;
    }
}
