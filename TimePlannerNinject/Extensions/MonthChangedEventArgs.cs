namespace TimePlannerNinject.Extensions
{
    using System;

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
