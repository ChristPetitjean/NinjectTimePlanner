// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputDayViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.ViewModel
{
   using System;
   using System.Collections.ObjectModel;
   using System.Linq;
   using System.Windows.Controls;

   using GalaSoft.MvvmLight;
   using GalaSoft.MvvmLight.CommandWpf;

   using TimePlannerNinject.Extensions;
   using TimePlannerNinject.Model;
   using TimePlannerNinject.Services;

   /// <summary>
   ///    ViewModel de saisie des évènement
   /// </summary>
   public class InputDayViewModel : ViewModelBase
   {
      #region Constants

      /// <summary>
      ///    Le nom de la propriété <see cref="DialogResult" />.
      /// </summary>
      public const string DialogResultPropertyName = "DialogResult";

      /// <summary>
      ///    Le nom de la propriété <see cref="Input" />.
      /// </summary>
      public const string InputPropertyName = "Input";

      #endregion

      #region Fields

      /// <summary>
      ///    Le service d'affichage de messagebox.
      /// </summary>
      private readonly IMessageboxService messageboxService;

      /// <summary>
      ///    Le service de données.
      /// </summary>
      private readonly ATimePlannerDataService service;

      /// <summary>
      ///    Commande de changement de sélection dans la liste des lieux de travails.
      /// </summary>
      private RelayCommand<SelectionChangedEventArgs> allPlacesSelectionChangeCommand;

      /// <summary>
      ///    Commande d'annulation.
      /// </summary>
      private RelayCommand cancelCommand;

      /// <summary>
      ///    Commande de suppression de l'évènement.
      /// </summary>
      private RelayCommand deleteInputDayCommand;

      /// <summary>
      ///    Le dialogresult de la page.
      /// </summary>
      private bool? dialogResult;

      /// <summary>
      ///    L'inputation.
      /// </summary>
      private InputDay input;

      /// <summary>
      ///    Commande de validation.
      /// </summary>
      private RelayCommand okCommand;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      ///    Initialise une nouvelle instance de la classe <see cref="InputDayViewModel" />.
      /// </summary>
      /// <param name="service">
      ///    Le service de données.
      /// </param>
      /// <param name="messageboxService">
      ///    Le service d'affichage de messagebox
      /// </param>
      /// <param name="model">
      ///    L'évènement a chargé.
      /// </param>
      public InputDayViewModel(ATimePlannerDataService service, IMessageboxService messageboxService, InputDay model)
      {
         this.service = service;
         this.messageboxService = messageboxService;
         this.input = (InputDay)model.Clone();
      }

      #endregion

      #region Public Properties

      /// <summary>
      ///    Obtient la commande de changement de sélection dans la liste des lieux de travails.
      /// </summary>
      public RelayCommand<SelectionChangedEventArgs> AllPlacesSelectionChangeCommand
      {
         get
         {
            return this.allPlacesSelectionChangeCommand
                   ?? (this.allPlacesSelectionChangeCommand = new RelayCommand<SelectionChangedEventArgs>(this.ExecuteAllPlacesSelectionChangeCommand));
         }
      }

      /// <summary>
      ///    Obtietn ou définit les lieux de travails.
      /// </summary>
      public ObservableCollection<WorkPlace> AllWorkPlace
      {
         get
         {
            return this.service.AllPlaces;
         }

         set
         {
            this.service.AllPlaces = value;
            this.RaisePropertyChanged();
         }
      }

      /// <summary>
      ///    Obtient la commande d'annulation.
      /// </summary>
      public RelayCommand CancelCommand
      {
         get
         {
            return this.cancelCommand ?? (this.cancelCommand = new RelayCommand(this.ExecuteCancelCommand));
         }
      }

      /// <summary>
      ///    Obtient la commande de suppression.
      /// </summary>
      public RelayCommand DeleteInputDayCommand
      {
         get
         {
            return this.deleteInputDayCommand
                   ?? (this.deleteInputDayCommand = new RelayCommand(this.ExecuteDeleteInputDayCommand, this.CanExecuteDeleteInputDayCommand));
         }
      }

      /// <summary>
      ///    Obtient ou définit le dialogresult.
      /// </summary>
      public bool? DialogResult
      {
         get
         {
            return this.dialogResult;
         }

         set
         {
            this.Set(DialogResultPropertyName, ref this.dialogResult, value);
         }
      }

      /// <summary>
      ///    Obtient ou definit l'imputaton.
      /// </summary>
      public InputDay Input
      {
         get
         {
            return this.input;
         }

         set
         {
            this.Set(InputPropertyName, ref this.input, value);
         }
      }

      /// <summary>
      ///    Obtient la commande de validatino.
      /// </summary>
      public RelayCommand OkCommand
      {
         get
         {
            return this.okCommand ?? (this.okCommand = new RelayCommand(this.ExecuteOkCommand));
         }
      }

      #endregion

      #region Methods

      /// <summary>
      ///    Methode de verification de la commande de suppression.
      /// </summary>
      /// <returns>
      ///    True si la command peut s'executer, false sinon.
      /// </returns>
      private bool CanExecuteDeleteInputDayCommand()
      {
         return this.service.AllDays.Any(d => d.ID == this.Input.ID);
      }

      /// <summary>
      ///    Execute la commande de changement de sélection.
      /// </summary>
      /// <param name="e">
      ///    L'argument de changement de sélection.
      /// </param>
      private void ExecuteAllPlacesSelectionChangeCommand(SelectionChangedEventArgs e)
      {
         var addedItem = e.AddedItems[0] as WorkPlace;
         if (addedItem != null && this.Input.WorkStartTime.HasValue && this.Input.WorkEndTime.HasValue)
         {
            this.Input.WorkStartTime = new DateTime(
               this.Input.WorkStartTime.Value.Year,
               this.Input.WorkStartTime.Value.Month,
               this.Input.WorkStartTime.Value.Day,
               addedItem.DefaultStartTime.Hour,
               addedItem.DefaultStartTime.Minute,
               addedItem.DefaultStartTime.Second);
            this.Input.WorkEndTime = new DateTime(
               this.Input.WorkEndTime.Value.Year,
               this.Input.WorkEndTime.Value.Month,
               this.Input.WorkEndTime.Value.Day,
               addedItem.DefaultEndTime.Hour,
               addedItem.DefaultEndTime.Minute,
               addedItem.DefaultEndTime.Second);
         }
      }

      /// <summary>
      ///    Execute la commande d'annulation.
      /// </summary>
      private void ExecuteCancelCommand()
      {
         this.DialogResult = false;
      }

      /// <summary>
      ///    Execute la commande de suppression.
      /// </summary>
      private void ExecuteDeleteInputDayCommand()
      {
         var inputDay = this.service.AllDays.FirstOrDefault(d => d.ID == this.Input.ID);
         if (inputDay != null)
         {
            this.service.AllDays.Remove(inputDay);
         }

         this.DialogResult = true;
      }

      /// <summary>
      ///    Execute la commande de validation.
      /// </summary>
      private void ExecuteOkCommand()
      {
         if (!this.Input.IdWorkPlace.HasValue)
         {
            this.messageboxService.ShowMessagebox("Le lieu est obligatoire pour pouvoir sauvegarder", MessageboxKind.Ok, "Enregistrement impossible");
            return;
         }

         var day = this.service.AllDays.FirstOrDefault(d => d.ID == this.Input.ID);
         if (day == null)
         {
            this.service.AllDays.Add(this.Input);
         }
         else
         {
            day.ExtraHours = this.Input.ExtraHours;
            day.IdWorkPlace = this.Input.IdWorkPlace;
            day.IsWorked = this.Input.IsWorked;
            day.WorkEndTime = this.Input.WorkEndTime;
            day.WorkStartTime = this.Input.WorkStartTime;
         }

         this.DialogResult = true;
      }

      #endregion
   }
}