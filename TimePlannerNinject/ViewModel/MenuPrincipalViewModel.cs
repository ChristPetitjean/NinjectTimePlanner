// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuPrincipalViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.ViewModel
{
    using System.Windows;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Microsoft.Win32;

    using TimePlannerNinject.Kernel;
    using TimePlannerNinject.Model;
    using TimePlannerNinject.Services;

    /// <summary>
    ///     View model du menu principal.
    /// </summary>
    public class MenuPrincipalViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Le service de données.
        /// </summary>
        private readonly ATimePlannerDataService service;

        /// <summary>
        ///     Le service d'affichage de popup.
        /// </summary>
        private readonly IWindowService windowService;

        /// <summary>
        ///     Commande de sortie de l'application
        /// </summary>
        private RelayCommand exitCommand;

        /// <summary>
        ///     Commande d'ouverture de fichier
        /// </summary>
        private RelayCommand openNewFileCommand;

        /// <summary>
        ///     Commande de sauvegarde
        /// </summary>
        private RelayCommand saveFileCommand;

        /// <summary>
        ///     Commande de sélection de tous les éléments
        /// </summary>
        private RelayCommand selectAllCommand;

        /// <summary>
        ///     Commande de dé-sélection de tous les éléments
        /// </summary>
        private RelayCommand unSelectAllCommand;

        /// <summary>
        ///     Commande de lancement de l'aperçu avant impression
        /// </summary>
        private RelayCommand previewPrintCommand;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="MenuPrincipalViewModel" />.
        /// </summary>
        /// <param name="service">Le service de données.</param>
        /// <param name="windowService">Service de gestion des fenêtres</param>
        public MenuPrincipalViewModel(ATimePlannerDataService service, IWindowService windowService)
        {
            this.service = service;
            this.windowService = windowService;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Obtient la commande de sortie de l'application.
        /// </summary>
        public RelayCommand ExitCommand => this.exitCommand ?? (this.exitCommand = new RelayCommand(this.ExecuteExitCommand));

        /// <summary>
        ///     Obtient la commande d'ouverture de fichier.
        /// </summary>
        public RelayCommand OpenNewFileCommand
            => this.openNewFileCommand ?? (this.openNewFileCommand = new RelayCommand(this.ExecuteOpenNewFileCommand));

        /// <summary>
        ///     Obtient la commande de sauvegarde.
        /// </summary>
        public RelayCommand SaveFileCommand => this.saveFileCommand ?? (this.saveFileCommand = new RelayCommand(this.ExecuteSaveFileCommand));

        /// <summary>
        ///     Obtient la commande de selection de toutes les inputations.
        /// </summary>
        public RelayCommand SelectAllCommand => this.selectAllCommand ?? (this.selectAllCommand = new RelayCommand(this.ExecuteSelectAllCommand, this.CanExecuteSelectAllCommand));

        /// <summary>
        ///     Obtient la commande de dé-selection de toutes les inputations.
        /// </summary>
        public RelayCommand UnSelectAllCommand => this.unSelectAllCommand ?? (this.unSelectAllCommand = new RelayCommand(this.ExecuteUnSelectAllCommand, this.CanExecuteUnSelectAllCommand));

        /// <summary>
        ///     Obtient la commande d'aperçu avant impression.
        /// </summary>
        public RelayCommand PreviewPrintCommand => this.previewPrintCommand ?? (this.previewPrintCommand = new RelayCommand(this.ExecutePreviewPrintCommand));

        #endregion

        #region Methods

        /// <summary>
        ///     Executela commande de sortie de l'application.
        /// </summary>
        private void ExecuteExitCommand()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        ///     Execute la commande d'ouverture de nouveau fichier.
        /// </summary>
        private void ExecuteOpenNewFileCommand()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichier d'évenement | *.tpn";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                this.service.ReadDataFromFile(openFileDialog.FileName);
                StatutMessage.SendStatutMessage($"Fichier \"{openFileDialog.FileName}\" ouvert");
            }
        }

        /// <summary>
        ///     Execute la commande d'aperçu avant impression.
        /// </summary>
        private void ExecutePreviewPrintCommand()
        {
            this.windowService.OpenDialog<PreviewPrintViewModel>();
        }

        /// <summary>
        ///     Execute la commande de sauvegarde.
        /// </summary>
        private void ExecuteSaveFileCommand()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Fichier d'évenement | *.tpn";
            if (saveFileDialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                this.service.SaveDataToFile(saveFileDialog.FileName);
                StatutMessage.SendStatutMessage($"Fichier \"{saveFileDialog.FileName}\" sauvegardé");
            }
        }

        /// <summary>
        ///     Execute la commande de sélection de tous les Input.
        /// </summary>
        private void ExecuteSelectAllCommand()
        {
            KernelTimePlanner.Get<CalendrierViewModel>().IsAllInputSelected = true;
        }

        /// <summary>
        ///     Execute la commande de dé-sélection de tous les Input.
        /// </summary>
        private void ExecuteUnSelectAllCommand()
        {
            KernelTimePlanner.Get<CalendrierViewModel>().IsAllInputSelected = false;
        }

        /// <summary>
        /// Définit si la méthode de sélection de  tous les Input pêut s'executer
        /// </summary>
        /// <returns>True si oui, sinon false</returns>
        private bool CanExecuteSelectAllCommand()
        {
            return !KernelTimePlanner.Get<CalendrierViewModel>().IsAllInputSelected;
        }

        /// <summary>
        /// Définit si la méthode de dé-sélection de  tous les Input pêut s'executer
        /// </summary>
        /// <returns>True si oui, sinon false</returns>
        private bool CanExecuteUnSelectAllCommand()
        {
            return KernelTimePlanner.Get<CalendrierViewModel>().IsAllInputSelected;
        }
        #endregion
    }
}