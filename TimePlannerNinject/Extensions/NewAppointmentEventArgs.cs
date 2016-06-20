// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewAppointmentEventArgs.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Arguments de nouvel événement.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Extensions
{
   using System;

   /// <summary>
   ///    Arguments de nouvel événement.
   /// </summary>
   public class NewAppointmentEventArgs : EventArgs
   {
      #region Public Properties

      /// <summary>
      ///    Obtient ou définit la date de fn.
      /// </summary>
      public DateTime? EndDate { get; set; }

      /// <summary>
      ///    Obtient ou définit la date de début.
      /// </summary>
      public DateTime? StartDate { get; set; }

      #endregion
   }
}