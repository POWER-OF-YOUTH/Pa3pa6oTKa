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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
