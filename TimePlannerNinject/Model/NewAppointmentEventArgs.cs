// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewAppointmentEventArgs.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Model
{
   using System;

   /// <summary>
   /// Arguments de nouvel événement.
   /// </summary>
   public class NewAppointmentEventArgs : EventArgs
   {
      #region Fields

      /// <summary>
      /// Obtient ou définit l'indentifiant du candidat.
      /// </summary>
      public int? CandidateId { get; set; }

      /// <summary>
      /// Obtient ou définit la date de fn.
      /// </summary>
      public DateTime? EndDate { get; set; }

      /// <summary>
      /// Obtient ou définit la date de début.
      /// </summary>
      public DateTime? StartDate { get; set; }

      #endregion
   }
}