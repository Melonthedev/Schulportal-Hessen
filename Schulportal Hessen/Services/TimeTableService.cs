using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Schulportal_Hessen.Helpers;
using Schulportal_Hessen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Schulportal_Hessen.Services;
public class TimeTableService {
    
    private List<TimeTableLesson> lessons = new();
    public SpWrapper _SpWrapper { get; set; }

    public TimeTableService(SpWrapper SpWrapper) {
        _SpWrapper = SpWrapper;
    }

    public async Task SyncWithSchulPortal() {
        List<TimeTableLesson> spLessons = await _SpWrapper.GetTimetableAsync();
        CourseColorProvider courseColorProvider = new();
        if (lessons.Count == 0) {
            spLessons.ForEach(lesson => {
                lesson.Course.Color = courseColorProvider.GetCourseColor(lesson.Course.SchulPortalTimeTableName);
                lesson.Course.CourseEntries = new();
                lesson.Course.Abbreviation = lesson.Course.SchulPortalTimeTableName;
            });
            lessons = spLessons;
            return;
        }
    }

    public List<TimeTableLesson> GetLessons() {
        return lessons;
    }

}
