namespace TimePlannerNinject.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class IfNullConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}