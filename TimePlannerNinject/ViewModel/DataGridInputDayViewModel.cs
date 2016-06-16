// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridInputDayViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.ViewModel
{
   using System;
   using System.Collections.Generic;
   using System.Collections.ObjectModel;
   using System.ComponentModel;
   using System.Linq;
   using System.Windows.Data;

   using GalaSoft.MvvmLight;

   using TimePlannerNinject.Interfaces;
   using TimePlannerNinject.Model;

   /// <summary>
   ///    The data grid input day view model.
   /// </summary>
   public class DataGridInputDayViewModel : ViewModelBase
   {
      #region Constants

      /// <summary>
      ///    The <see cref="DisplayDays" /> property's name.
      /// </summary>
      public const string DisplayDaysPropertyName = "DisplayDays";

      /// <summary>
      ///    The <see cref="DisplayWorkplaces" /> property's name.
      /// </summary>
      public const string DisplayWorkplacesPropertyName = "DisplayWorkplaces";

      /// <summary>
      ///    The <see cref="SelectedDisplayMonth" /> property's name.
      /// </summary>
      public const string SelectedDisplayMonthPropertyName = "SelectedDisplayMonth";

      #endregion

      #region Fields

      /// <summary>
      /// The display days.
      /// </summary>
      private ICollectionView displayDays;

      /// <summary>
      /// The display workplaces.
      /// </summary>
      private ObservableCollection<WorkPlace> displayWorkplaces;

      /// <summary>
      /// The selected display month.
      /// </summary>
      private DateTime selectedDisplayMonth;

      /// <summary>
      ///    Service de données du planner
      /// </summary>
      private readonly ATimePlannerDataService service;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="DataGridInputDayViewModel"/>.
      /// </summary>
      /// <param name="service">
      /// The service.
      /// </param>
      public DataGridInputDayViewModel(ATimePlannerDataService service)
      {
         this.service = service;
         this.selectedDisplayMonth = DateTime.Now;
         this.displayDays = CollectionViewSource.GetDefaultView(this.service.AllDays);
         this.displayDays.GroupDescriptions.Add(new PropertyGroupDescription("IdWorkPlace"));
         this.displayDays.Filter = this.FilterDisplayDays;
         this.displayWorkplaces = new ObservableCollection<WorkPlace>(this.service.AllPlaces);
      }

      #endregion


      #region Public Properties

      /// <summary>
      ///    Sets and gets the DisplayDays property.
      ///    Changes to that property's value raise the PropertyChanged event.
      /// </summary>
      public ICollectionView DisplayDays
      {
         get
         {
            return this.displayDays;
         }

         set
         {
            this.Set(DisplayDaysPropertyName, ref this.displayDays, value);
         }
      }

      /// <summary>
      ///    Sets and gets the DisplayWorkplaces property.
      ///    Changes to that property's value raise the PropertyChanged event.
      /// </summary>
      public ObservableCollection<WorkPlace> DisplayWorkplaces
      {
         get
         {
            return this.displayWorkplaces;
         }

         set
         {
            this.Set(DisplayWorkplacesPropertyName, ref this.displayWorkplaces, value);
         }
      }

      /// <summary>
      ///    Sets and gets the SelectedDisplayMonth property.
      ///    Changes to that property's value raise the PropertyChanged event.
      /// </summary>
      public DateTime SelectedDisplayMonth
      {
         get
         {
            return this.selectedDisplayMonth;
         }

         set
         {
            this.Set(SelectedDisplayMonthPropertyName, ref this.selectedDisplayMonth, value);
            this.DisplayDays.Refresh();
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// The filter display days.
      /// </summary>
      /// <param name="o">
      /// The o.
      /// </param>
      /// <returns>
      /// The <see cref="bool"/>.
      /// </returns>
      private bool FilterDisplayDays(object o)
      {
         var inputDay = o as InputDay;
         if (inputDay == null)
         {
            return false;
         }

         return inputDay.WorkStartTime.HasValue && inputDay.WorkStartTime.Value.Month == this.SelectedDisplayMonth.Month && inputDay.WorkStartTime.Value.Year == this.SelectedDisplayMonth.Year;
      }

      #endregion
   }
}