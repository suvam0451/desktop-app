using System;
using System.Diagnostics;
using System.Globalization;
using testing.DataModels;
using testing.Pages;

//namespace testing.ValueConverters
namespace testing
{
    // Converts application page to an actual viewpage
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(Object value, Type targetType, object parameter, CultureInfo culture) {
            switch ((Application_Sidebar)value) {

                case Application_Sidebar.TextureCombiner:
                    return new TextureCombine();
                case Application_Sidebar.HomePage:
                    return new Sidebar_Home();
                default:
                    Debugger.Break();
                    return null;
            }

            throw new NotImplementedException(); 
        }

        public override object ConvertBack(Object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
