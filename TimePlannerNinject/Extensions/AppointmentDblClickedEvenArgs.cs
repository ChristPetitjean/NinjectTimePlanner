// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppointmentDblClickedEvenArgs.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Argument de click sur un évènement.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Extensions
{
   using System;

   /// <summary>
   ///    Argument de click sur un évènement.
   /// </summary>
   public class AppointmentDblClickedEvenArgs : EventArgs
   {
      #region Public Properties

      /// <summary>
      ///    Obtient ou définit l'identifiant de l'événement.
      /// </summary>
      public int? Id { get; set; }

      #endregion
   }
}