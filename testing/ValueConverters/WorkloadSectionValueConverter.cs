using System;
using System.Diagnostics;
using System.Globalization;
using testing.DataModels;
using testing.Pages;
using testing.Pages.Workloads;

namespace testing
{
    // Converts application page to an actual viewpage
    public class WorkloadSectionValueConverter : BaseValueConverter<WorkloadSectionValueConverter>
    {
        public override object Convert(Object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Application_Workload)value)
            {

                case Application_Workload.None:
                    return null;
                case Application_Workload.Default:
                    return new Workload_Default();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(Object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}