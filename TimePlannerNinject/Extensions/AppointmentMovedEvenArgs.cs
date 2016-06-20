// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppointmentMovedEvenArgs.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Argument de déplacement d'évènement.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Extensions
{
   using System;

   /// <summary>
   ///    Argument de déplacement d'évènement.
   /// </summary>
   public class AppointmentMovedEvenArgs : EventArgs
   {
      #region Public Properties

      /// <summary>
      ///    Obtient ou définit l'dentifiant du rendez-vous.
      /// </summary>
      public int AppointmentId { get; set; }

      /// <summary>
      ///    Obtient ou définit le numéro du nouveau jour.
      /// </summary>
      public int NewDay { get; set; }

      /// <summary>
      ///    Obtient ou définit le numéro de l'ancien jour.
      /// </summary>
      public int OldDay { get; set; }

      #endregion
   }
}