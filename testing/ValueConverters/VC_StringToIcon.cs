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
                case: 
            }
        }
    }
}

Do it like this```
2                                       do nothing
2 -> 2                                  do nothing
2 -> 2 -> 2                             do nothing
2 -> 2 -> 2 -> 2                        do nothing
2 -> 2 -> 2 -> 2 -> 2                   exceeds 9 (delete smallest {2} )
2 -> 2 -> 2 -> 3                        MATCH (4 combinations)
2 -> 2 -> 2 -> 3 -> 3                   exceeds 9 (delete smallest {2} )
2 -> 2 -> 3 -> 3                        exceeds 9 (delete smallest {2} )
2 -> 3 -> 3                             do nothing
2 -> 3 -> 3 -> 3                        exceeds 0 (delete smallest {2} )
3 -> 3 -> 3                             MATCH(1 combinations)

