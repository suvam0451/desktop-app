using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace testing.ValueConverters
{
    public class VC_StringToIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            switch (value.ToString().ToLower()) {
                case "airplane":
                    return "/images/logo/airplane.png";
                default:
                    return "/images/logo/airplane.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            switch (value.ToString().ToLower()) {
                case "/images/logo/airplane.png":
                    return "airplane";
                default:
                    return "airplane";
            }
        }
    }
}