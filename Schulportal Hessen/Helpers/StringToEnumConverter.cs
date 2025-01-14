using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schulportal_Hessen.Helpers {
    public class StringToEnumConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value != null && value.GetType().IsEnum) {
                return value.ToString();
            }

            return null; // Rückgabe, falls der Wert kein Enum ist
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            if (value is string stringValue && targetType.IsEnum) {
                return Enum.TryParse(targetType, stringValue, out var result) ? result : Activator.CreateInstance(targetType);
            }

            return Activator.CreateInstance(targetType); // Standardwert des Enums
        }
    }
}
