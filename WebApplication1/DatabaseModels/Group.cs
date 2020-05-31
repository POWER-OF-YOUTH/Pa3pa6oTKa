using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DatabaseModels
{
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public Course Course { get; set; }

        public string GetCourseName()
        {
            if (Course == null)
                return $"Академическая группа";
            return Course.CourseName;
        }
    }
}
