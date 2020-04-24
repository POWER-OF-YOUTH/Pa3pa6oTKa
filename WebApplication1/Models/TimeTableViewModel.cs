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
        public TimeSpan Time;
        public string EventsName;
        public int ID;
    }
}
