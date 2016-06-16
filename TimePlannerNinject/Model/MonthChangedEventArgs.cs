using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Model
{
   public struct MonthChangedEventArgs
   {
      public DateTime OldDisplayStartDate;

      public DateTime NewDisplayStartDate;
   }
}
