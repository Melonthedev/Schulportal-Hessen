﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Schulportal_Hessen.Helpers {
    public class ColorUtil {

        public static Color GetWindowsColor(System.Drawing.Color color) {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

    }
}
