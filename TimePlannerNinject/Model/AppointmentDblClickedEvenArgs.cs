// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppointmentDblClickedEvenArgs.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Model
{
   using System;

   /// <summary>
   /// The appointment dbl clicked even args.
   /// </summary>
   public class AppointmentDblClickedEvenArgs : EventArgs
   {
      #region Public Properties

      /// <summary>
      /// Obtient ou définit l'identifiant de l'événement.
      /// </summary>
      public int? Id { get; set; }

      #endregion
   }
}