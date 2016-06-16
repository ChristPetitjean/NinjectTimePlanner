using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Converters
{
   using System.Globalization;
   using System.Windows.Data;

   public class DateTimeToTimeConverter:IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var val = value as DateTime?;
         if (val != null)
         {
            return val.Value.ToString("t");
         }

         return string.Empty;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
