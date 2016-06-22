using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Converters
{
   using System.Globalization;
   using System.Windows.Data;

   public class IfNullConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
          return value ?? parameter;
      }

       public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
