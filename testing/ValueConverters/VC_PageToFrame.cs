using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Markup;
using testing.DataModels;
using testing.Pages;

namespace testing
{
    public class VC_PageToFrame : MarkupExtension, IValueConverter
    {
        #region Markup Extension

        // private static T mConverter = null;
        // Provides static instance of value converter...
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //
            // return mConverter ?? (mConverter = new T());
            throw new NotImplementedException();
        }
        #endregion

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((EPageList)value)
            {
                case EPageList.Default:
                    return new Workload_Default();
                case EPageList.CombineTexture:
                    return new CombineTextures_Type1();
                case EPageList.PlayVideo:
                    return new PlayVideo();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // switch (value.ToString().ToLower())
            // {
            //     case "/images/logo/airplane.png":
            //         return "airplane";
            //     default:
            //         return "airplane";
            // }
            throw new NotImplementedException();
        }
    }
}
