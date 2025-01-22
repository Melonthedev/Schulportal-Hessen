using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schulportal_Hessen.API.Models.Courses;

namespace Schulportal_Hessen.API.Models.Timetable
{
    public class TimeTableLesson {

        public Guid Id { get; set; }
        public Course Course { get; set; }
        public string Room { get; set; }
        public int Hour { get; set; }
        public int Day { get; set; }
    }
}
