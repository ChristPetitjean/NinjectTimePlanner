// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Extensions
{
   using System.Collections.Generic;
   using System.Collections.ObjectModel;

   /// <summary>
   /// Extension de la classe enumerable.
   /// </summary>
   // ReSharper disable once InconsistentNaming
   public static class IEnumerableExtensions
   {
      #region Public Methods and Operators

      /// <summary>
      /// Transforme un Ienumerable en ObservableCollection.
      /// </summary>
      /// <param name="list">
      /// Liste a convertir.
      /// </param>
      /// <typeparam name="T">
      /// Type d'objet de la liste
      /// </typeparam>
      /// <returns>
      /// L'<see cref="ObservableCollection{T}"/> correspondante.
      /// </returns>
      public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list) where T : class
      {
         return new ObservableCollection<T>(list);
      }

      #endregion
   }
}