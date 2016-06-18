// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendrierViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using TimePlannerNinject.Interfaces;
    using TimePlannerNinject.Model;

    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class CalendrierViewModel : ViewModelBase
    {
        /// <summary>
        ///     The <see cref="DateEnCours" /> property's name.
        /// </summary>
        public const string DateEnCoursPropertyName = "DateEnCours";

        /// <summary>
        ///     The <see cref="Days" /> property's name.
        /// </summary>
        public const string DaysPropertyName = "Days";

        /// <summary>
        ///     The messagebox service.
        /// </summary>
        private readonly IMessageboxService messageboxService;

        /// <summary>
        ///     The service.
        /// </summary>
        private readonly ATimePlannerDataService service;

        /// <summary>
        ///     The window service.
        /// </summary>
        private readonly IWindowService windowService;

        /// <summary>
        ///     The date en cours.
        /// </summary>
        private DateTime dateEnCours;

        /// <summary>
        ///     The day box double clicked command.
        /// </summary>
        private RelayCommand<NewAppointmentEventArgs> dayBoxDoubleClickedCommand;

        /// <summary>
        ///     The input day changed command.
        /// </summary>
        private RelayCommand<AppointmentMovedEvenArgs> inputDayChangedCommand;

        /// <summary>
        ///     The input day double clicked command.
        /// </summary>
        private RelayCommand<AppointmentDblClickedEvenArgs> inputDayDoubleClickedCommand;

        /// <summary>
        ///     Initializes a new instance of the CalendrierViewModel class.
        /// </summary>
        /// <param name="service">
        ///     The service.
        /// </param>
        /// <param name="windowService">
        ///     The window Service.
        /// </param>
        /// <param name="messageboxService">
        ///     The messagebox Service.
        /// </param>
        public CalendrierViewModel(
            ATimePlannerDataService service, 
            IWindowService windowService, 
            IMessageboxService messageboxService)
        {
            this.service = service;
            this.service.DataReadEnd += this.ServiceDataReadEnd;
            this.windowService = windowService;
            this.messageboxService = messageboxService;
            this.dateEnCours = DateTime.Today;
        }

        /// <summary>
        /// Gets or sets the date en cours.
        /// </summary>
        public DateTime DateEnCours
        {
            get
            {
                return this.dateEnCours;
            }

            set
            {
                this.Set(DateEnCoursPropertyName, ref this.dateEnCours, value);
            }
        }

        /// <summary>
        ///     Gets the DayBoxDoubleClickedCommand.
        /// </summary>
        public RelayCommand<NewAppointmentEventArgs> DayBoxDoubleClickedCommand
        {
            get
            {
                return this.dayBoxDoubleClickedCommand
                       ?? (this.dayBoxDoubleClickedCommand =
                           new RelayCommand<NewAppointmentEventArgs>(this.ExecuteDayBoxDoubleClickedCommand));
            }
        }

        /// <summary>
        /// Gets or sets the days.
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
        ///     Gets the InputDayChangedCommand.
        /// </summary>
        public RelayCommand<AppointmentMovedEvenArgs> InputDayChangedCommand
        {
            get
            {
                return this.inputDayChangedCommand
                       ?? (this.inputDayChangedCommand =
                           new RelayCommand<AppointmentMovedEvenArgs>(this.ExecuteInputDayChangedCommand));
            }
        }

        /// <summary>
        ///     Gets the InputDayDoubleClickedCommand.
        /// </summary>
        public RelayCommand<AppointmentDblClickedEvenArgs> InputDayDoubleClickedCommand
        {
            get
            {
                return this.inputDayDoubleClickedCommand
                       ?? (this.inputDayDoubleClickedCommand =
                           new RelayCommand<AppointmentDblClickedEvenArgs>(this.ExecuteInputDayDoubleClickedCommand));
            }
        }

        /// <summary>
        ///     The execute day box double clicked command.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void ExecuteDayBoxDoubleClickedCommand(NewAppointmentEventArgs e)
        {
            if (!this.service.AllPlaces.Any())
            {
                this.messageboxService.ShowMessagebox(
                    "Aucun lieu de travail n'existe. Merci d'en saisir au moins un", 
                    MessageboxKind.Ok, 
                    "Impossible d'ajouter des inputations");
                return;
            }

            if (e != null)
            {
                var id = this.Days.Any() ? this.Days.Max(d => d.ID) + 1 : 1;
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

        /// <summary>
        ///     The execute input day changed command.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void ExecuteInputDayChangedCommand(AppointmentMovedEvenArgs e)
        {
            if (e != null)
            {
                StatutMessage.SendStatutMessage(
                    string.Format(
                        "deplacement d'événement:{0}, depuis {1} vers {2}", 
                        e.AppointmentId, 
                        e.OldDay, 
                        e.NewDay));
            }
        }

        /// <summary>
        ///     The execute input day double clicked command.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void ExecuteInputDayDoubleClickedCommand(AppointmentDblClickedEvenArgs e)
        {
            if (e != null)
            {
                this.windowService.OpenDialog<InputDayViewModel>(
                    this.Days.First(d => e.Id != null && d.ID == e.Id.Value).Clone());
            }
            else
            {
                StatutMessage.SendStatutMessage("Le double click sur l'événement à echoué");
            }
        }

        /// <summary>
        /// The service data read end.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ServiceDataReadEnd(object sender, EventArgs e)
        {
            // Force a new Rebuild of calendar
            this.Days = this.service.AllDays;
        }
    }
}