using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DatabaseModels
{
    public class Homework
    {
        public int ID { get; set; }
        public Group Group { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
        public DateTime Deadline { get; set; }
    }
}
