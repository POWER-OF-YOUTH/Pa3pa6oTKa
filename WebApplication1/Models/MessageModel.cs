using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class MessageModel
    {
        public string Message { get; set; }

        public MessageModel(string message)
        {
            Message = message;
        }
    }
}
