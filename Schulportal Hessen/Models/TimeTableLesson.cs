using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.Models {
    public class TimeTableLesson {

        public Guid Id { get; set; }
        public Course Course { get; set; }
        public string Room { get; set; }
        public int Hour { get; set; }
        public int Day { get; set; }
    }
}
