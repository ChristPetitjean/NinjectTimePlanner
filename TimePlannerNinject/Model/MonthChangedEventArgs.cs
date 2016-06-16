using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Model
{
   /// <summary>
   /// Evénement de changement de mois
   /// </summary>
   /// <seealso cref="System.EventArgs" />
   public class MonthChangedEventArgs : EventArgs
   {
      /// <summary>
      /// Obtient ou définit l'ancienne date
      /// </summary>
      public DateTime OldDisplayStartDate {get; set; }

      /// <summary>
      /// Obtient ou définit la nouvelle date
      /// </summary>
      public DateTime NewDisplayStartDate { get; set; }
   }
}
