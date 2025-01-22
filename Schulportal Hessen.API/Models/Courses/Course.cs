using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.API.Models.Courses {
    public class Course
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Abbreviation { get; set; }
        public string SchulPortalTimeTableName { get; set; }
        public string SchulPortalCourseName { get; set; }
        public string Teacher { get; set; }
        public Color Color { get; set; }
        public Collection<CourseEntry> CourseEntries { get; set; }
    }
}
