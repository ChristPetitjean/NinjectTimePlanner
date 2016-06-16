// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkplaceLookupConverter.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Converters
{
   using System;
   using System.Globalization;
   using System.Linq;
   using System.Reflection;
   using System.Windows.Data;

   using TimePlannerNinject.Interfaces;
   using TimePlannerNinject.Kernel;

   /// <summary>
   /// Convertis un id de lieu de travail en la valeur du champs correpondant au parametre
   /// </summary>
   /// <seealso cref="System.Windows.Data.IValueConverter" />
   public class WorkplaceLookupConverter : IValueConverter
   {
      /// <summary>
      /// Convertit une valeur.
      /// </summary>
      /// <param name="value">
      /// Valeur produite par la source de liaison.
      /// </param>
      /// <param name="targetType">
      /// Type de la propriété de cible de liaison.
      /// </param>
      /// <param name="parameter">
      /// Paramètre de convertisseur à utiliser.
      /// </param>
      /// <param name="culture">
      /// Culture à utiliser dans le convertisseur.
      /// </param>
      /// <returns>
      /// Une valeur convertie. Si la méthode retourne null, la valeur Null valide est utilisée.
      /// </returns>
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var workPlace = (from p in KernelTimePlanner.Get<ATimePlannerDataService>().AllPlaces
                          where p.Id.Equals(value)
                          select p).FirstOrDefault();

         if (workPlace != null)
         {
            PropertyInfo property = workPlace.GetType().GetProperty(parameter.ToString());
            if (property != null)
            {
               var val = property.GetValue(workPlace);
               return val;
            }
         }

         return null;
      }

      /// <summary>
      /// Convertit une valeur.
      /// </summary>
      /// <param name="value">
      /// Valeur produite par la cible de liaison.
      /// </param>
      /// <param name="targetType">
      /// Type dans lequel convertir.
      /// </param>
      /// <param name="parameter">
      /// Paramètre de convertisseur à utiliser.
      /// </param>
      /// <param name="culture">
      /// Culture à utiliser dans le convertisseur.
      /// </param>
      /// <returns>
      /// Une valeur convertie. Si la méthode retourne null, la valeur Null valide est utilisée.
      /// </returns>
      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return Binding.DoNothing;
      }
   }
}
