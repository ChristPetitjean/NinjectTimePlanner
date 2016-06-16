// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Extensions
{
   using System;
   using System.Threading;

   /// <summary>
   /// The date time extensions.
   /// </summary>
   public static class DateTimeExtensions
   {
      #region Public Methods and Operators

      /// <summary>
      /// Retourne le premier jour du mois.
      /// </summary>
      /// <param name="dt">
      /// La date.
      /// </param>
      /// <returns>
      /// Le <see cref="DateTime"/> représentant le premier jour du mois.
      /// </returns>
      public static DateTime FirstDayOfMonth(this DateTime dt)
      {
         return new DateTime(dt.Year, dt.Month, 1);
      }

      /// <summary>
      /// Retourne le premier jour du prochain mois.
      /// </summary>
      /// <param name="dt">
      /// La date.
      /// </param>
      /// <returns>
      /// Le <see cref="DateTime"/> représentant le premier jour du prochain mois.
      /// </returns>
      public static DateTime FirstDayOfNextMonth(this DateTime dt)
      {
         return dt.FirstDayOfMonth().AddMonths(1);
      }

      /// <summary>
      /// Retourne le premier jour de la semaine.
      /// </summary>
      /// <param name="dt">
      /// La date.
      /// </param>
      /// <returns>
      /// Le <see cref="DateTime"/> représentant le premier jour de la semaine.
      /// </returns>
      public static DateTime FirstDayOfWeek(this DateTime dt)
      {
         var culture = Thread.CurrentThread.CurrentCulture;
         var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
         if (diff < 0)
         {
            diff += 7;
         }

         return dt.AddDays(-diff).Date;
      }

      /// <summary>
      /// Retourne le dernier jour du mois.
      /// </summary>
      /// <param name="dt">
      /// La date.
      /// </param>
      /// <returns>
      /// Le <see cref="DateTime"/> représentant le dernier jour du mois.
      /// </returns>
      public static DateTime LastDayOfMonth(this DateTime dt)
      {
         return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
      }

      /// <summary>
      /// Retourne le dernier jour de la semaine.
      /// </summary>
      /// <param name="dt">
      /// La date.
      /// </param>
      /// <returns>
      /// Le <see cref="DateTime"/> réprésentant le dernier jour de la semaine.
      /// </returns>
      public static DateTime LastDayOfWeek(this DateTime dt)
      {
         return dt.FirstDayOfWeek().AddDays(6);
      }

      #endregion
   }
}