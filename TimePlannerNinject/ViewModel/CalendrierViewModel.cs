using GalaSoft.MvvmLight;

namespace TimePlannerNinject.ViewModel
{
   using System;
   using System.Collections.ObjectModel;
   using System.Linq;

   using GalaSoft.MvvmLight.CommandWpf;

   using TimePlannerNinject.Model;

   /// <summary>
   /// This class contains properties that a View can data bind to.
   /// <para>
   /// See http://www.galasoft.ch/mvvm
   /// </para>
   /// </summary>
   public class CalendrierViewModel : ViewModelBase
   {
      private readonly TimePlannerDataService service;

      /// <summary>
      /// Initializes a new instance of the CalendrierViewModel class.
      /// </summary>
      public CalendrierViewModel(TimePlannerDataService service)
      {
         this.service = service;
         this.dateEnCours = DateTime.Today;
         //this.days =
         //   new ObservableCollection<InputDay>(
         //      this.service.AllDays.FindAll(
         //         i =>
         //         i.WorkStartTime != null && i.WorkStartTime.Value.Month == this.DateEnCours.Month
         //         && i.WorkStartTime.Value.Year == this.DateEnCours.Year));
      }

      /// <summary>
      /// The <see cref="Days" /> property's name.
      /// </summary>
      public const string DaysPropertyName = "Days";

      private ObservableCollection<InputDay> days;

      /// <summary>
      /// Sets and gets the Days property.
      /// Changes to that property's value raise the PropertyChanged event. 
      /// </summary>
      public ObservableCollection<InputDay> Days
      {
         get
         {
            return days;
         }
         set
         {
            Set(DaysPropertyName, ref days, value);
         }
      }

      /// <summary>
      /// The <see cref="DateEnCours" /> property's name.
      /// </summary>
      public const string DateEnCoursPropertyName = "DateEnCours";

      private DateTime dateEnCours;

      /// <summary>
      /// Sets and gets the DateEnCours property.
      /// Changes to that property's value raise the PropertyChanged event. 
      /// </summary>
      public DateTime DateEnCours
      {
         get
         {
            return dateEnCours;
         }
         set
         {
            Set(DateEnCoursPropertyName, ref dateEnCours, value);
         }
      }

      private RelayCommand<MonthChangedEventArgs> displayMonthChangedCommand;

      /// <summary>
      /// Gets the DisplayMonthChangedCommand.
      /// </summary>
      public RelayCommand<MonthChangedEventArgs> DisplayMonthChangedCommand
      {
         get
         {
            return displayMonthChangedCommand
                ?? (displayMonthChangedCommand = new RelayCommand<MonthChangedEventArgs>(ExecuteDisplayMonthChangedCommand));
         }
      }

      private void ExecuteDisplayMonthChangedCommand(MonthChangedEventArgs e)
      {
         Days =
            new ObservableCollection<InputDay>(
               this.service.AllDays.FindAll(
                  i =>
                  i.WorkStartTime != null && i.WorkStartTime.Value.Month == this.DateEnCours.Month
                  && i.WorkStartTime.Value.Year == this.DateEnCours.Year));
      }
   }
}