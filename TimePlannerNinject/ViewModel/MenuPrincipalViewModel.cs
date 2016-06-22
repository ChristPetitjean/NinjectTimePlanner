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

   using TimePlannerNinject.Model;
   using TimePlannerNinject.Services;

   /// <summary>
   ///    View model du menu principal.
   /// </summary>
   public class MenuPrincipalViewModel : ViewModelBase
   {
      #region Fields

      /// <summary>
      ///    Le service de données.
      /// </summary>
      private readonly ATimePlannerDataService service;

      /// <summary>
      ///    Commande de sortie de l'application
      /// </summary>
      private RelayCommand exitCommand;

      /// <summary>
      ///    Commande d'ouverture de fichier
      /// </summary>
      private RelayCommand openNewFileCommand;

      /// <summary>
      ///    Commande de sauvegarde
      /// </summary>
      private RelayCommand saveFileCommand;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      ///    Initialise une nouvelle instance de la classe <see cref="MenuPrincipalViewModel" />.
      /// </summary>
      /// <param name="service">Le service de données.</param>
      public MenuPrincipalViewModel(ATimePlannerDataService service)
      {
         this.service = service;
      }

      #endregion

      #region Public Properties

      /// <summary>
      ///    Obtient la commande de sortie de l'application.
      /// </summary>
      public RelayCommand ExitCommand => this.exitCommand ?? (this.exitCommand = new RelayCommand(this.ExecuteExitCommand));

       /// <summary>
      ///    Obtient la commande d'ouverture de fichier.
      /// </summary>
      public RelayCommand OpenNewFileCommand => this.openNewFileCommand ?? (this.openNewFileCommand = new RelayCommand(this.ExecuteOpenNewFileCommand));

       /// <summary>
      ///    Obtient la commande de sauvegarde.
      /// </summary>
      public RelayCommand SaveFileCommand => this.saveFileCommand ?? (this.saveFileCommand = new RelayCommand(this.ExecuteSaveFileCommand));

       #endregion

      #region Methods

      /// <summary>
      ///    Executela commande de sortie de l'application.
      /// </summary>
      private void ExecuteExitCommand()
      {
         Application.Current.Shutdown();
      }

      /// <summary>
      ///    Execute la commande d'ouverture de nouveau fichier.
      /// </summary>
      private void ExecuteOpenNewFileCommand()
      {
         var openFileDialog = new OpenFileDialog();
         openFileDialog.Filter = "Fichier d'évenement | *.tpn";
         openFileDialog.Multiselect = false;
         if (openFileDialog.ShowDialog(Application.Current.MainWindow) == true)
         {
            this.service.ReadDataFromFile(openFileDialog.FileName);
            StatutMessage.SendStatutMessage(string.Format("Fichier \"{0}\" ouvert", openFileDialog.FileName));
         }
      }

      /// <summary>
      ///    Executela commande de sauvegarde.
      /// </summary>
      private void ExecuteSaveFileCommand()
      {
         var saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Fichier d'évenement | *.tpn";
         if (saveFileDialog.ShowDialog(Application.Current.MainWindow) == true)
         {
            this.service.SaveDataToFile(saveFileDialog.FileName);
            StatutMessage.SendStatutMessage(string.Format("Fichier \"{0}\" sauvegardé", saveFileDialog.FileName));
         }
      }

      #endregion
   }
}