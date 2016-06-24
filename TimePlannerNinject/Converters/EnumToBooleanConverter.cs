using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Converters
{
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Convertie une enum en bool.
    /// </summary>
    public class EnumToBooleanConverter : IValueConverter
    {
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
            return value.Equals(parameter);
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
            return (bool)value ? parameter : Binding.DoNothing;
        }
    }
}
