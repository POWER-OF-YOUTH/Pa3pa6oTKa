using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/Home/");
        }

        public IActionResult Register()
        {
            return View(new MessageModel(""));
        }

        [HttpPost]
        public IActionResult Register(string login, string password, string firstName, string lastName, string otchestvo)
        {
            if (DatabaseManager.GetMain().ContainsLogin(login))
                return View("Register", new MessageModel("Такой логин уже есть"));
            if (login == null || password == null || password.Length < 7 || firstName == null)
                return View("Register", new MessageModel("Не все данные указаны корректно"));
            DatabaseManager.GetMain().RegisterUser(login, firstName, lastName, otchestvo, password.GetSHA256());
            return Redirect("/");
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}