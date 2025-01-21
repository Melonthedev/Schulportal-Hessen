using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Schulportal_Hessen.Helpers {
    public class ColorUtil {


        public static Color GetColor(int argb) {
            var a = (byte)((argb & -16777216) >> 0x18);
            var r = (byte)((argb & 0xff0000) >> 0x10);
            var g = (byte)((argb & 0xff00) >> 8);
            var b = (byte)(argb & 0xff);
            return Color.FromArgb(a, r, g, b);
        }

    }
}
