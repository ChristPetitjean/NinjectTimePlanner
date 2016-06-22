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
    using System.Windows.Media.Converters;

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
        #region Fields

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
        ///     Les dates selectionnées
        /// </summary>
        private ObservableCollection<DateTime> selectedInputs = new ObservableCollection<DateTime>();

        /// <summary>
        /// Le fait que toutes les dates soit sélectionnées ou non
        /// </summary>
        private bool isAllInputSelected;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="CalendrierViewModel" />.
        /// </summary>
        /// <param name="service">Le service de données.</param>
        /// <param name="windowService">Le service d'affichage de fenêtre personnalisée.</param>
        /// <param name="messageboxService">Le service d'affichage de messagebox.</param>
        public CalendrierViewModel(ATimePlannerDataService service, IWindowService windowService, IMessageboxService messageboxService)
        {
            this.service = service;
            this.service.DataReadEnd += this.ServiceDataReadEnd;
            this.windowService = windowService;
            this.messageboxService = messageboxService;
            this.dateEnCours = DateTime.Today;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Obtient ou définit la date en cours.
        /// </summary>
        public DateTime DateEnCours
        {
            get
            {
                return this.dateEnCours;
            }

            set
            {
                this.Set(nameof(this.DateEnCours), ref this.dateEnCours, value);
            }
        }

        /// <summary>
        ///     Obtient la commande de double clcik sur un emplacement vide du calendrier.
        /// </summary>
        public RelayCommand<NewAppointmentEventArgs> DayBoxDoubleClickedCommand
            =>
                this.dayBoxDoubleClickedCommand
                ?? (this.dayBoxDoubleClickedCommand = new RelayCommand<NewAppointmentEventArgs>(this.ExecuteDayBoxDoubleClickedCommand));

        /// <summary>
        ///     Obtient ou définit la collection de tous les évènements.
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
            =>
                this.inputDayChangedCommand
                ?? (this.inputDayChangedCommand = new RelayCommand<AppointmentMovedEvenArgs>(this.ExecuteInputDayChangedCommand));

        /// <summary>
        ///     Gets the InputDayDoubleClickedCommand.
        /// </summary>
        public RelayCommand<AppointmentDblClickedEvenArgs> InputDayDoubleClickedCommand
            =>
                this.inputDayDoubleClickedCommand
                ?? (this.inputDayDoubleClickedCommand = new RelayCommand<AppointmentDblClickedEvenArgs>(this.ExecuteInputDayDoubleClickedCommand));

        /// <summary>
        ///     Obtient ou définit les dates selectionnées
        /// </summary>
        public ObservableCollection<DateTime> SelectedInputsId
        {
            get
            {
                return this.selectedInputs;
            }

            set
            {
                this.Set(nameof(this.SelectedInputsId), ref this.selectedInputs, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit une valeur indiquant si toutes les dates sont selectionnées
        /// </summary>
        public bool IsAllInputSelected
        {
            get
            {
                return this.isAllInputSelected;
            }

            set
            {
                this.Set(nameof(this.IsAllInputSelected), ref this.isAllInputSelected, value);
            }
        }

        #endregion

        #region Methods

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
                this.windowService.OpenDialog<InputDayViewModel>(this.SelectedInputsId.ToArray());
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
                StatutMessage.SendStatutMessage($"deplacement d'événement:{e.AppointmentId}, depuis {e.OldDay} vers {e.NewDay}");
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
                this.windowService.OpenDialog<InputDayViewModel>(this.SelectedInputsId.ToArray());
            }
            else
            {
                StatutMessage.SendStatutMessage("Le double click sur l'événement à echoué");
            }
        }

        /// <summary>
        ///     Evènement de fin de lecture des données.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void ServiceDataReadEnd(object sender, EventArgs e)
        {
            // Force a new Rebuild of calendar
            this.Days = this.service.AllDays;
        }

        #endregion
    }
}