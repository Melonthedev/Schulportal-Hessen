using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.Models {
    public enum Attendance {
        Present,
        Excused,
        Late,
        AttendanceObligationSuspended,
        Absent,
        Suspended
    }
}
