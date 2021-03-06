﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkplaceLookupConverter.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    using TimePlannerNinject.Kernel;
    using TimePlannerNinject.Services;

    /// <summary>
    ///     Convertis un id de lieu de travail en la valeur du champs correpondant au parametre
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class WorkplaceLookupConverter : IValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Convertit une valeur.
        /// </summary>
        /// <param name="value">
        ///     Valeur produite par la source de liaison.
        /// </param>
        /// <param name="targetType">
        ///     Type de la propriété de cible de liaison.
        /// </param>
        /// <param name="parameter">
        ///     Paramètre de convertisseur à utiliser.
        /// </param>
        /// <param name="culture">
        ///     Culture à utiliser dans le convertisseur.
        /// </param>
        /// <returns>
        ///     Une valeur convertie. Si la méthode retourne null, la valeur Null valide est utilisée.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var workPlace = (from p in KernelTimePlanner.Get<ATimePlannerDataService>().AllPlaces
                             where p.Id.Equals(value)
                             select p).FirstOrDefault();

            var property = workPlace?.GetType().GetProperty(parameter.ToString());
            var val = property?.GetValue(workPlace);
            return val;
        }

        /// <summary>
        ///     Convertit une valeur.
        /// </summary>
        /// <param name="value">
        ///     Valeur produite par la cible de liaison.
        /// </param>
        /// <param name="targetType">
        ///     Type dans lequel convertir.
        /// </param>
        /// <param name="parameter">
        ///     Paramètre de convertisseur à utiliser.
        /// </param>
        /// <param name="culture">
        ///     Culture à utiliser dans le convertisseur.
        /// </param>
        /// <returns>
        ///     Une valeur convertie. Si la méthode retourne null, la valeur Null valide est utilisée.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        #endregion
    }
}