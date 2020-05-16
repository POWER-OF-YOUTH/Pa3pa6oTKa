using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Главная страница";
            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Страница входа";

            return View();
        }

        public IActionResult Register()
        {
            ViewData["Message"] = "Страница регистрации";

            return View();
        }

        public IActionResult AddHomework()
        {
            ViewData["Message"] = "Страница добавления дАмашки";
            return View();
        }

        public IActionResult AddEvents()
        {
            ViewData["Message"] = "Страница добавления мероприятий";
            return View();
        }
        public IActionResult TestMethod()
        {
            ViewData["Message"] = "Тестовое сообщение.";

            return View();
        }
        

        public IActionResult TimeTable(bool eventCheckbox, bool homeworkCheckbox)
        {
            ViewData["Message"] = "Страница с расписанием";
            var model = new TimeTableViewModel();
            var date = DateTime.Now.Date;
            while (date.DayOfWeek != DayOfWeek.Monday)
                date -= new TimeSpan(1, 0, 0, 0);
            if (!eventCheckbox && !homeworkCheckbox)
                eventCheckbox = homeworkCheckbox = true;
            if(eventCheckbox )
            {
                var events = DatabaseManager.GetMain().GetEventsByTime(date, date + new TimeSpan(14, 0, 0, 0));
                foreach (var item in events)
                {
                    CreateDayIfNotExists(model.Days, item.StartTime.Date);
                    model.Days[item.StartTime.Date].Events.Add(item);
                }
            }
            if(homeworkCheckbox)
            {
                var homeworkList = DatabaseManager.GetMain().GetHomeworksByTime(date, date + new TimeSpan(14, 0, 0, 0));
                foreach (var item in homeworkList)
                {
                    CreateDayIfNotExists(model.Days, item.Deadline.Date);
                    model.Days[item.Deadline.Date].HomeWorks.Add(item);
                }
            }
            model.EventChecked = eventCheckbox;
            model.HomeworkChecked = homeworkCheckbox;
            
            model.Days = model.Days.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
            return View(model);
        }

        private void CreateDayIfNotExists(Dictionary<DateTime, Day> days, DateTime date)
        {
            if (!days.ContainsKey(date))
                days.Add(date, new Day(date)
                {
                    Name = date.DayOfWeek.ToString()
                });
        }

#region Data
        [HttpPost]
        public ActionResult CreateHomework(string title, string description, string Ffile)
        {
            //TODO: Добавление информации в базу данных
            return Redirect("/");
        }
#endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
