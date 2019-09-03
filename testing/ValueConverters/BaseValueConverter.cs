using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace testing
{
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        #region Private Members
        private static T mConverter = null;
        #endregion

        #region Markup Extension
        // Provides static instance of value converter...
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //
            return mConverter ?? (mConverter = new T());
            throw new NotImplementedException();
        }
        #endregion

        #region Value Converter Methods

        // The method to convert one type to another
        public abstract object Convert(Object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(Object value, Type targetType, object parameter, CultureInfo culture);
        #endregion
    }
}
