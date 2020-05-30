using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class MessageModel : AccountModel
    {
        public string Message { get; set; }

        public MessageModel(string message, IRequestCookieCollection cookies) : base(cookies)
        {
            Message = message;
        }
    }
}
