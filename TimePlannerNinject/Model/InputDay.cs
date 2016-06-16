// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputDay.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Model
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Runtime.Serialization;
   using System.Security.Permissions;
   using System.Windows.Media.Effects;

   using GalaSoft.MvvmLight;

   using TimePlannerNinject.Extensions;

   /// <summary>
   ///    The input day.
   /// </summary>
   [Serializable]
   public class InputDay : ObservableObject, ISerializable
   {
      #region Fields

      /// <summary>
      ///    Identifiant.
      /// </summary>
      private int id;

      /// <summary>
      ///    jour de la semaine.
      /// </summary>
      private DateTime date;

      /// <summary>
      ///    Heures supplémentaires.
      /// </summary>
      private int? extraHours;

      /// <summary>
      ///    Jour travaillé.
      /// </summary>
      private bool? isWorked;

      /// <summary>
      ///    Heure de fin du travail quotidien.
      /// </summary>
      private DateTime? workEndTime;

      /// <summary>
      ///    Lieux de travail.
      /// </summary>
      private int? idWorkPlace;

      /// <summary>
      ///    Heure de début du travail quotidien.
      /// </summary>
      private DateTime? workStartTime;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="InputDay"/>.
      /// </summary>
      /// <param name="date">
      /// Date pour laquelle créer l'entrée.
      /// </param>
      public InputDay(DateTime date)
      {
         this.Date = date;
      }

      /// <summary>
      ///    Initialise une nouvelle instance de la classe <see cref="InputDay" />.
      /// </summary>
      public InputDay()
      {
      }

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="InputDay"/>.
      /// </summary>
      /// <param name="info">
      /// Les infos de sérilisation.
      /// </param>
      /// <param name="context">
      /// le context de sérialisation.
      /// </param>
      public InputDay(SerializationInfo info, StreamingContext context)
      {
         this.Date = info.GetDateTime("Date");
         this.ExtraHours = (int?)info.GetValue("ExtraHours", typeof(int?));
         this.IsWorked = (bool?)info.GetValue("IsWorked", typeof(bool?));
         this.WorkEndTime = (DateTime?)info.GetValue("WorkEndTime", typeof(DateTime?));
         this.WorkStartTime = (DateTime?)info.GetValue("WorkStartTime", typeof(DateTime?));
         this.IdWorkPlace = (int?)info.GetValue("IdWorkPlace", typeof(int?));
      }

      #endregion

      #region Public Properties

      /// <summary>
      ///    Obtient ou définit je jour de la semaine.
      /// </summary>
      public DateTime Date
      {
         get
         {
            return this.date;
         }

         set
         {
            this.Set("Date", ref this.date, value);
         }
      }

      /// <summary>
      ///    Obtient ou définit les heures supplémentaires.
      /// </summary>
      public int? ExtraHours
      {
         get
         {
            return this.extraHours;
         }

         set
         {
            this.Set("ExtraHours", ref this.extraHours, value);
         }
      }

      /// <summary>
      ///    Obtient ou définit une valeur indiquant si l'entrée concerne un jour travaillé ou non.
      /// </summary>
      public bool? IsWorked
      {
         get
         {
            return this.isWorked;
         }

         set
         {
            this.Set("IsWorked", ref this.isWorked, value);
            if (this.isWorked.HasValue && !this.isWorked.Value)
            {
               this.WorkEndTime = null;
               this.WorkStartTime = null;
               this.ExtraHours = null;
               this.IdWorkPlace = null;
            }
         }
      }

      /// <summary>
      ///    Obtient ou définit l'heure de fin du travail quotidien.
      /// </summary>
      public DateTime? WorkEndTime
      {
         get
         {
            return this.workEndTime;
         }

         set
         {
            this.Set("WorkEndTime", ref this.workEndTime, value);
         }
      }

      /// <summary>
      ///    Obtient ou définit le lieux de travail pour l'entrée.
      /// </summary>
      public int? IdWorkPlace
      {
         get
         {
            return this.idWorkPlace;
         }

         set
         {
            this.Set("IdWorkPlace", ref this.idWorkPlace, value);
         }
      }

      /// <summary>
      ///    Obtient ou définit l'heure de début du travail quotidien.
      /// </summary>
      public DateTime? WorkStartTime
      {
         get
         {
            return this.workStartTime;
         }

         set
         {
            this.Set("WorkStartTime", ref this.workStartTime, value);
         }
      }

      /// <summary>
      ///    Obtient ou définit l'identifiant.
      /// </summary>
      // ReSharper disable once InconsistentNaming
      public int ID
      {
         get
         {
            return this.id;
         }

         set
         {
            this.Set("ID", ref this.id, value);
         }
      }

      #endregion

      #region Public Methods and Operators

      /// <summary>
      /// Initialise l'ensemble des entrée pour le mois donnée.
      /// </summary>
      /// <param name="year">
      /// Année de génération.
      /// </param>
      /// <param name="month">
      /// Mois de génération.
      /// </param>
      /// <param name="days">
      /// Les jours existant.
      /// </param>
      /// <returns>
      /// Un <see cref="IEnumerable{InputDay}"/> contenant l'ensemble des jours du mois.
      /// </returns>
      public static IEnumerable<InputDay> InitializeMonthInputDays(int year, int month, params InputDay[] days)
      {
         var date = new DateTime(year, month, 1);
         var firstDay = date.FirstDayOfWeek();
         var lastDay = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month));
         while (firstDay < lastDay)
         {
            var monday = days.Any(d => d.Date == firstDay) ? days.First(d => d.Date == firstDay) : new InputDay(firstDay);
            yield return monday;
            firstDay = firstDay.AddDays(1);
            var tuesday = days.Any(d => d.Date == firstDay) ? days.First(d => d.Date == firstDay) : new InputDay(firstDay);
            yield return tuesday;
            firstDay = firstDay.AddDays(1);
            var wednesday = days.Any(d => d.Date == firstDay) ? days.First(d => d.Date == firstDay) : new InputDay(firstDay);
            yield return wednesday;
            firstDay = firstDay.AddDays(1);
            var thursday = days.Any(d => d.Date == firstDay) ? days.First(d => d.Date == firstDay) : new InputDay(firstDay);
            yield return thursday;
            firstDay = firstDay.AddDays(1);
            var friday = days.Any(d => d.Date == firstDay) ? days.First(d => d.Date == firstDay) : new InputDay(firstDay);
            yield return friday;
            firstDay = firstDay.AddDays(1);
            var saturday = days.Any(d => d.Date == firstDay) ? days.First(d => d.Date == firstDay) : new InputDay(firstDay);
            yield return saturday;
            firstDay = firstDay.AddDays(1);
            var sunday = days.Any(d => d.Date == firstDay) ? days.First(d => d.Date == firstDay) : new InputDay(firstDay);
            yield return sunday;
            firstDay = firstDay.AddDays(1);
         }
      }

      /// <summary>
      /// Calcul l'ensemble des jours disponible pour le mois donné.
      /// </summary>
      /// <param name="year">L'année concernée.</param>
      /// <param name="month">Le mois dont on souhaite récupérer les jours.</param>
      /// <returns>Une collection de jour correspondant au mois</returns>
      public static IEnumerable<InputDay> GetDatesInMonth(int year, int month)
      {
         return from j in Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                select new InputDay(new DateTime(year, month, j));
      } 

      /// <summary>
      /// Remplit <see cref="T:System.Runtime.Serialization.SerializationInfo"/> avec les données nécessaires pour sérialiser
      ///    l'objet cible.
      /// </summary>
      /// <param name="info">
      /// <see cref="T:System.Runtime.Serialization.SerializationInfo"/> à remplir de données.
      /// </param>
      /// <param name="context">
      /// Destination (consultez <see cref="T:System.Runtime.Serialization.StreamingContext"/>) de cette
      ///    sérialisation.
      /// </param>
      [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("Date", this.Date);
         info.AddValue("ExtraHours", this.ExtraHours);
         info.AddValue("IsWorked", this.IsWorked);
         info.AddValue("WorkEndTime", this.WorkEndTime);
         info.AddValue("WorkStartTime", this.WorkStartTime);
         info.AddValue("IdWorkPlace", this.IdWorkPlace);
      }

      #endregion
   }
}