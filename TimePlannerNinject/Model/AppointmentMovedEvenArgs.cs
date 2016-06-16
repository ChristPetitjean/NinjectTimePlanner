namespace TimePlannerNinject.Model
{
   using System;

   /// <summary>
   ///    The appointment moved even args.
   /// </summary>
   public class AppointmentMovedEvenArgs : EventArgs
   {
      #region Public Properties

      /// <summary>
      ///    Obtient ou d�finit l'dentifiant du rendez-vous.
      /// </summary>
      public int AppointmentId { get; set; }

      /// <summary>
      ///    Obtient ou d�finit le num�ro du nouveau jour.
      /// </summary>
      public int NewDay { get; set; }

      /// <summary>
      ///    Obtient ou d�finit le num�ro de l'ancien jour.
      /// </summary>
      public int OldDay { get; set; }

      #endregion
   }
}