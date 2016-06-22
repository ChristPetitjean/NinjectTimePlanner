// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonthChangedEventArgs.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Argument de changement de mois
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Extensions
{
    using System;

    /// <summary>
    ///     Argument de changement de mois
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class MonthChangedEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        ///     Obtient ou définit la nouvelle date
        /// </summary>
        public DateTime NewDisplayStartDate { get; set; }

        /// <summary>
        ///     Obtient ou définit l'ancienne date
        /// </summary>
        public DateTime OldDisplayStartDate { get; set; }

        #endregion
    }
}