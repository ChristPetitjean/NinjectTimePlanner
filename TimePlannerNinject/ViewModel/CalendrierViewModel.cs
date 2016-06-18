using GalaSoft.MvvmLight;

namespace TimePlannerNinject.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using GalaSoft.MvvmLight.CommandWpf;

    using TimePlannerNinject.Extensions;
    using TimePlannerNinject.Interfaces;
    using TimePlannerNinject.Kernel;
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

        private readonly IWindowService windowService;

        /// <summary>
        /// Initializes a new instance of the CalendrierViewModel class.
        /// </summary>
        public CalendrierViewModel(ATimePlannerDataService service, IWindowService windowService)
        {
            this.service = service;
            this.windowService = windowService;
            this.dateEnCours = DateTime.Today;
        }

        /// <summary>
        /// The <see cref="Days" /> property's name.
        /// </summary>
        public const string DaysPropertyName = "Days";

        /// <summary>
        /// Sets and gets the Days property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<InputDay> Days
        {
            get
            {
                return this.service.AllDays;
            }
            set
            {
                this.service.AllDays = value;
                this.RaisePropertyChanged();
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
                this.windowService.OpenDialog<InputDayViewModel>(this.Days.First(d => e.Id != null && d.ID == e.Id.Value).Clone());
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
                int id = this.Days.Any() ? this.Days.Max(d => d.ID) + 1 : 1;
                var newInput = new InputDay
                                   {
                                       ID = id,
                                       WorkStartTime = e.StartDate,
                                       WorkEndTime = e.EndDate,
                                       ExtraHours = 0
                                   };

                this.windowService.OpenDialog<InputDayViewModel>(newInput);
            }
            else
            {
                StatutMessage.SendStatutMessage("Le double click sur l'événement à echoué");
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