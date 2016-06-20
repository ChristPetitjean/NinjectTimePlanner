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

    using TimePlannerNinject.Extensions;
    using TimePlannerNinject.Model;
    using TimePlannerNinject.Services;

    /// <summary>
    ///     ViewModel des Calendrier
    /// </summary>
    public class CalendrierViewModel : ViewModelBase
    {
        /// <summary>
        ///     Le nom de la propriété <see cref="DateEnCours" />.
        /// </summary>
        public const string DateEnCoursPropertyName = "DateEnCours";

        /// <summary>
        ///     Le nom de la propriété <see cref="Days" />.
        /// </summary>
        public const string DaysPropertyName = "Days";

        /// <summary>
        ///     Service d'affichage des messagesbox.
        /// </summary>
        private readonly IMessageboxService messageboxService;

        /// <summary>
        ///     Service de données.
        /// </summary>
        private readonly ATimePlannerDataService service;

        /// <summary>
        ///     Service d'affichage de fenêtre utilisateur.
        /// </summary>
        private readonly IWindowService windowService;

        /// <summary>
        ///     La date en cours.
        /// </summary>
        private DateTime dateEnCours;

        /// <summary>
        ///     Commande de double click sur un emplacement vide du calendrier.
        /// </summary>
        private RelayCommand<NewAppointmentEventArgs> dayBoxDoubleClickedCommand;

        /// <summary>
        ///     Commande de modification d'un évènement.
        /// </summary>
        private RelayCommand<AppointmentMovedEvenArgs> inputDayChangedCommand;

        /// <summary>
        ///     Commande de double click sur un évènement.
        /// </summary>
        private RelayCommand<AppointmentDblClickedEvenArgs> inputDayDoubleClickedCommand;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CalendrierViewModel"/>.
        /// </summary>
        /// <param name="service">Le service de données.</param>
        /// <param name="windowService">Le service d'affichage de fenêtre personnalisée.</param>
        /// <param name="messageboxService">Le service d'affichage de messagebox.</param>
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
        /// Obtient ou définit la date en cours.
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
        ///     Obtient la commande de double clcik sur un emplacement vide du calendrier.
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
        /// Obtient ou définit la collection de tous les évènements.
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
        ///     Obtient la commande de mofification d'un évènement.
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
        ///     Execute de la commande de double click sur un évènement.
        /// </summary>
        /// <param name="e">
        ///     Arguments de la commande.
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
        ///     Execute de la commande de modification d'un évènement.
        /// </summary>
        /// <param name="e">
        ///     Arguments de la commande.
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
        ///     Execute de la commande de double click sur un emplacement vide du calendrier.
        /// </summary>
        /// <param name="e">
        ///     Arguments de la commande.
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
        /// Evènement de fin de lecture des données.
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