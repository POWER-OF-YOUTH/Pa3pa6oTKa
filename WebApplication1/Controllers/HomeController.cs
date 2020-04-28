using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult TimeTable()
        {
            ViewData["Message"] = "Страница с расписанием";
            var model = new TimeTableViewModel();
            model.Days.Add(new Day()
            {
                Name = "Понедельник",
                Date = DateTime.Now.Date
            });
            model.Days[0].HomeWorks.Add(new HomeWork()
            {
                ID = 1,
                HomeworkName = "Выжить",
                Discipline = "Математика",
                Time = new TimeSpan(16, 0, 0)
            });
            model.Days[0].Events.Add(new Event()
            {
                ID = 1,
                EventsName = "Введение в специальность (Мероприятие)",
                Time = new TimeSpan(16, 30, 0)
            });
            model.Days.Add(new Day()
            {
                Name = "Вторник",
                Date = DateTime.Now
            });
            model.Days[1].HomeWorks.Add(new HomeWork()
            {
                ID = 1,
                HomeworkName = "Выжить",
                Discipline = "Математика",
                Time = new TimeSpan(16, 0, 0)
            });
            model.Days[1].Events.Add(new Event()
            {
                ID = 1,
                EventsName = "Введение в специальность (Мероприятие)",
                Time = new TimeSpan(16, 30, 0)
            });
            model.Days.Add(new Day()
            {
                Name = "Среда",
                Date = DateTime.Now
            });
            model.Days[2].HomeWorks.Add(new HomeWork()
            {
                ID = 1,
                HomeworkName = "Выжить",
                Discipline = "Математика",
                Time = new TimeSpan(16, 0, 0)
            });
            model.Days[2].Events.Add(new Event()
            {
                ID = 1,
                EventsName = "Введение в специальность (Мероприятие)",
                Time = new TimeSpan(16, 30, 0)
            });
            model.Days.Add(new Day()
            {
                Name = "Четверг",
                Date = DateTime.Now
            });
            model.Days[3].HomeWorks.Add(new HomeWork()
            {
                ID = 1,
                HomeworkName = "Выжить",
                Discipline = "Математика",
                Time = new TimeSpan(16, 0, 0)
            });
            model.Days[3].Events.Add(new Event()
            {
                ID = 1,
                EventsName = "Введение в специальность (Мероприятие)",
                Time = new TimeSpan(16, 30, 0)
            });
            model.Days.Add(new Day()
            {
                Name = "Пятница",
                Date = DateTime.Now
            });
            model.Days[4].HomeWorks.Add(new HomeWork()
            {
                ID = 1,
                HomeworkName = "Выжить",
                Discipline = "Математика",
                Time = new TimeSpan(16, 0, 0)
            });
            model.Days[4].Events.Add(new Event()
            {
                ID = 1,
                EventsName = "Введение в специальность (Мероприятие)",
                Time = new TimeSpan(16, 30, 0)
            });
            model.Days.Add(new Day()
            {
                Name = "Суббота",
                Date = DateTime.Now
            });
            model.Days[5].HomeWorks.Add(new HomeWork()
            {
                ID = 1,
                HomeworkName = "Выжить",
                Discipline = "Математика",
                Time = new TimeSpan(16, 0, 0)
            });
            model.Days[5].Events.Add(new Event()
            {
                ID = 1,
                EventsName = "Введение в специальность (Мероприятие)",
                Time = new TimeSpan(16, 30, 0)
            });
            model.Days.Add(new Day()
            {
                Name = "Воскресенье",
                Date = DateTime.Now
            });
            model.Days[6].Events.Add(new Event()
            {
                ID = 1,
                EventsName = "Введение в специальность (Мероприятие)",
                Time = new TimeSpan(16, 30, 0)
            });
            return View(model);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
