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
            switch ((EPageList)value) {

                //  Workload pages
                case EPageList.CombineTexture:
                    return new CombineTextures_Type1();
                case EPageList.PlayVideo:
                    return new PlayVideo();
                case EPageList.HomePage:
                    return new Workload_Default();
                case EPageList.TrafficAnalysis:
                    return new TrafficAnalysis();

                // Sidebars
                case EPageList.Sidebar:
                    return new Sidebar_Home();
                case EPageList.Settings:
                    return new Settings();

                // Defaults
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(Object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
