using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.API.Models.Courses
{
    public class CourseEntry
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Homework { get; set; }
        public bool IsHomeworkMarkedAsCompleted { get; set; }
        public DateTime Date { get; set; }
        public List<string> FileAttatchmentUrls { get; set; }
        public Attendance Attendance { get; set; }
    }
}
