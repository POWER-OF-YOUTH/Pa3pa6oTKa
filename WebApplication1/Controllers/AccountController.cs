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
            return View(new AccountModel(HttpContext.Request.Cookies));
        }

        public IActionResult Register()
        {
            return View(new MessageModel("", HttpContext.Request.Cookies));
        }

        [HttpPost]
        public IActionResult Register(string login, string password, string firstName, string lastName, string otchestvo)
        {
            if (DatabaseManager.GetMain().ContainsLogin(login))
                return View("Register", new MessageModel("Такой логин уже есть", HttpContext.Request.Cookies));
            if (login == null || password == null || password.Length < 6 || firstName == null)
                return View("Register", new MessageModel("Не все данные указаны корректно", HttpContext.Request.Cookies));
            if (!DatabaseManager.GetMain().RegisterUser(login, firstName, lastName, otchestvo, password.GetSHA256()))
                return View("Register", new MessageModel("Произошла неведомая ошибка. Повторите попытку регистрации.", HttpContext.Request.Cookies));
            var token = DatabaseManager.GetMain().GetToken(login, password.GetSHA256());
            if (token == null)
                return View(new MessageModel("Логин или пароль неверные", HttpContext.Request.Cookies));
            HttpContext.Response.Cookies.Append("token", token);
            return Redirect("/");
        }

        public IActionResult Login()
        {
            return View(new MessageModel("", HttpContext.Request.Cookies));
        }

        public IActionResult Logout()
        {
            if (HttpContext.Request.Cookies.ContainsKey("token"))
                HttpContext.Response.Cookies.Delete("token");
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var token = DatabaseManager.GetMain().GetToken(login, password.GetSHA256());
            if (token == null)
            {
                return View(new MessageModel("Логин или пароль неверные", HttpContext.Request.Cookies));
            }
            HttpContext.Response.Cookies.Append("token", token);
            return Redirect("/");
        }
    }
}