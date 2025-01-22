using System.Drawing;

namespace Schulportal_Hessen.Helpers {
    public class CourseColorProvider {
        private readonly Dictionary<string, Color> courseColors = new() {
            { "M", Color.DarkRed },
            { "E", Color.Green },
            { "D", Color.DarkBlue },
            { "BIO", Color.DarkOliveGreen },
            { "G", Color.SaddleBrown },
            { "CH", Color.DarkViolet },
            { "PH", Color.SteelBlue },
            { "PW", Color.Orange },
            { "SP", Color.LimeGreen },
            { "F", Color.DarkKhaki },
            { "L", Color.DarkKhaki },
            { "MU", Color.DarkTurquoise },
            { "EK", Color.ForestGreen },
            { "DS", Color.MediumVioletRed },
            { "KU", Color.Maroon },
            { "EREL", Color.BlueViolet },
            { "KREL", Color.BlueViolet },
            { "ETH", Color.BlueViolet },

        };

        public Color GetCourseColor(string courseName) {

            KeyValuePair<string, Color> result = courseColors.AsQueryable().Where(x => courseName.ToLower().Equals(x.Key.ToLower())).FirstOrDefault();
            if (result.Key == null) {
                return Color.Gray;
            }
            return result.Value;
        }
    }
}
