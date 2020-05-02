using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public List<HomeWork> HomeWorks = new List<HomeWork>();
        public List<Event> Events = new List<Event>();
    }

    public class HomeWork
    {
        public TimeSpan Time;
        public string Discipline;
        public string HomeworkName;
        public int ID;
    }

    public class Event
    {
        public int ID;
        public DateTime StartTime;
        public string Name;
        public string Description;
    }
}
