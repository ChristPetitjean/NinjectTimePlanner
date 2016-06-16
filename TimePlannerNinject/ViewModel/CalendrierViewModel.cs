using GalaSoft.MvvmLight;

namespace TimePlannerNinject.ViewModel
{
   using System;
   using System.Collections.ObjectModel;
   using System.Linq;

   using GalaSoft.MvvmLight.CommandWpf;

   using TimePlannerNinject.Interfaces;
   using TimePlannerNinject.Model;
   using TimePlannerNinject.UserControl;

   using Xceed.Wpf.Toolkit;

   /// <summary>
   /// This class contains properties that a View can data bind to.
   /// <para>
   /// See http://www.galasoft.ch/mvvm
   /// </para>
   /// </summary>
   public class CalendrierViewModel : ViewModelBase
   {
      private readonly ATimePlannerDataService service;

      /// <summary>
      /// Initializes a new instance of the CalendrierViewModel class.
      /// </summary>
      public CalendrierViewModel(ATimePlannerDataService service)
      {
         this.service = service;
         this.dateEnCours = DateTime.Today;
         this.days =
            new ObservableCollection<InputDay>(
               this.service.AllDays.FindAll(i => i.WorkStartTime.HasValue && i.WorkStartTime.Value.Month == this.DateEnCours.Month && i.WorkStartTime.Value.Year == this.DateEnCours.Year));
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
            Days =
               new ObservableCollection<InputDay>(
                  this.service.AllDays.FindAll(
                     i => i.WorkStartTime != null && i.WorkStartTime.Value.Month == value.Month && i.WorkStartTime.Value.Year == value.Year));
         }
      }

      private RelayCommand<AppointmentDblClickedEvenArgs> inputDayDoubleClickedCommand;

      /// <summary>
      /// Gets the InputDayDoubleClickedCommand.
      /// </summary>
      public RelayCommand<AppointmentDblClickedEvenArgs> InputDayDoubleClickedCommand
      {
         get
         {
            return inputDayDoubleClickedCommand
                ?? (inputDayDoubleClickedCommand = new RelayCommand<AppointmentDblClickedEvenArgs>(ExecuteInputDayDoubleClickedCommand));
         }
      }

      private void ExecuteInputDayDoubleClickedCommand(AppointmentDblClickedEvenArgs e)
      {
         if (e != null)
         {
            StatutMessage.SendStatutMessage(string.Format("Double click sur l'évènement d'ID:{0}", e.AppointementId));
         }
         else
         {
            StatutMessage.SendStatutMessage("Le double click sur l'événement à echoué");
         }
      }

      private RelayCommand<NewAppointmentEventArgs> dayBoxDoubleClickedCommand;

      /// <summary>
      /// Gets the DayBoxDoubleClickedCommand.
      /// </summary>
      public RelayCommand<NewAppointmentEventArgs> DayBoxDoubleClickedCommand
      {
         get
         {
            return dayBoxDoubleClickedCommand
                ?? (dayBoxDoubleClickedCommand = new RelayCommand<NewAppointmentEventArgs>(ExecuteDayBoxDoubleClickedCommand));
         }
      }

      private void ExecuteDayBoxDoubleClickedCommand(NewAppointmentEventArgs e)
      {
         if (e != null)
         {
            StatutMessage.SendStatutMessage(string.Format("double click sur le jour:{0}", e.StartDate.Value.ToShortDateString()));
         }
      }

      private RelayCommand<AppointmentMovedEvenArgs> inputDayChangedCommand;

      /// <summary>
      /// Gets the InputDayChangedCommand.
      /// </summary>
      public RelayCommand<AppointmentMovedEvenArgs> InputDayChangedCommand
      {
         get
         {
            return inputDayChangedCommand
                ?? (inputDayChangedCommand = new RelayCommand<AppointmentMovedEvenArgs>(ExecuteInputDayChangedCommand));
         }
      }

      private void ExecuteInputDayChangedCommand(AppointmentMovedEvenArgs e)
      {
         if (e != null)
         {
            StatutMessage.SendStatutMessage(string.Format("deplacement d'événement:{0}, depuis {1} vers {2}", e.AppointmentId, e.OldDay, e.NewDay));
         }
      }
   }
}