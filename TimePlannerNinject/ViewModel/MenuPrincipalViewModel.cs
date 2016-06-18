// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuPrincipalViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using GalaSoft.MvvmLight.CommandWpf;
using TimePlannerNinject.Kernel;

namespace TimePlannerNinject.ViewModel
{
    using System.Windows;
    using System.Xml.Serialization;

    using GalaSoft.MvvmLight;

    using Microsoft.Win32;

    using TimePlannerNinject.Interfaces;
    using TimePlannerNinject.Model;

    /// <summary>
    /// The menu view model.
    /// </summary>
    public class MenuPrincipalViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The service.
        /// </summary>
        private readonly ATimePlannerDataService service;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuPrincipalViewModel"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public MenuPrincipalViewModel(ATimePlannerDataService service)
        {
            this.service = service;
        }

        private RelayCommand openNewFileCommand;

        /// <summary>
        /// Gets the OpenNewFileCommand.
        /// </summary>
        public RelayCommand OpenNewFileCommand
        {
            get
            {
                return openNewFileCommand
                    ?? (openNewFileCommand = new RelayCommand(ExecuteOpenNewFileCommand));
            }
        }

        private void ExecuteOpenNewFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichier d'évenement | *.tpn";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                this.service.ReadDataFromFile(openFileDialog.FileName);
                StatutMessage.SendStatutMessage(string.Format("Fichier \"{0}\" ouvert", openFileDialog.FileName));
            }
        }

        private RelayCommand saveFileCommand;

        /// <summary>
        /// Gets the SaveFileCommand.
        /// </summary>
        public RelayCommand SaveFileCommand
        {
            get
            {
                return saveFileCommand
                    ?? (saveFileCommand = new RelayCommand(ExecuteSaveFileCommand));
            }
        }

        private void ExecuteSaveFileCommand()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Fichier d'évenement | *.tpn";
            if (saveFileDialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                this.service.SaveDataToFile(saveFileDialog.FileName);
                StatutMessage.SendStatutMessage(string.Format("Fichier \"{0}\" sauvegardé", saveFileDialog.FileName));
            }
        }

        private RelayCommand exitCommand;

        /// <summary>
        /// Gets the ExitCommand.
        /// </summary>
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand
                    ?? (exitCommand = new RelayCommand(ExecuteExitCommand));
            }
        }

        private void ExecuteExitCommand()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}