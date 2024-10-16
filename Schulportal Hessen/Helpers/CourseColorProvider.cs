using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using System.Diagnostics;

namespace Schulportal_Hessen.Helpers {
    public class CourseColorProvider {
        private readonly Dictionary<string, Color> courseColors = new() {
            { "M", Colors.DarkRed },
            { "E", Colors.Green },
            { "D", Colors.DarkBlue },
            { "BIO", Colors.DarkOliveGreen },
            { "G", Colors.SaddleBrown },
            { "CH", Colors.DarkViolet },
            { "PH", Colors.SteelBlue },
            { "PW", Colors.Orange },
            { "SP", Colors.LimeGreen },
            { "F", Colors.DarkKhaki },
            { "L", Colors.DarkKhaki },
            { "MU", Colors.DarkTurquoise },
            { "EK", Colors.ForestGreen },
            { "DS", Colors.MediumVioletRed },
            { "KU", Colors.Maroon },
            { "EREL", Colors.BlueViolet },
            { "KREL", Colors.BlueViolet },
            { "ETH", Colors.BlueViolet },

        };

        public Color GetCourseColor(string courseName) {

            KeyValuePair<string, Color> result = courseColors.AsQueryable().Where(x => courseName.ToLower().Equals(x.Key.ToLower())).FirstOrDefault();
            if (result.Key == null) {
                return Colors.Gray;
            }
            return result.Value;
        }
    }
}
