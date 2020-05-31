using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AccountModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsStudent { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsEventsHolder { get; set; }

        public AccountModel(IRequestCookieCollection cookies)
        {
            if (!cookies.ContainsKey("token"))
                return;
            DatabaseManager.GetMain().FillUser(this, cookies["token"]);
        }

        public bool Exists()
        {
            return FirstName != null;
        }

        public List<Group> GetTeacherGroups()
        {

        }
    }
}
