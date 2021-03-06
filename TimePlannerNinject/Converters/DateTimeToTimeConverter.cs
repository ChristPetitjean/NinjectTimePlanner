﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeToTimeConverter.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    ///     Converti un datetime en time
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class DateTimeToTimeConverter : IValueConverter
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
            var val = value as DateTime?;
            return val?.ToString("t");
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