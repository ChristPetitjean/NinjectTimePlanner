// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputDayViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.ViewModel
{
   using System;
   using System.Collections.Generic;
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
      ///    Le nom de la propriété <see cref="ExtraHours" />.
      /// </summary>
      public const string ExtraHoursPropertyName = "ExtraHours";

      /// <summary>
      ///    Le nom de la propriété <see cref="DatesToEdit" />.
      /// </summary>
      public const string IdsToEditPropertyName = "DatesToEdit";

      /// <summary>
      ///    Le nom de la propriété <see cref="IdWorkPlace" />.
      /// </summary>
      public const string IdWorkPlacePropertyName = "IdWorkPlace";

      /// <summary>
      ///    Le nom de la propriété <see cref="Input" />.
      /// </summary>
      public const string InputPropertyName = "Input";

      /// <summary>
      ///    Le nom de la propriété <see cref="WorkEndTime" />.
      /// </summary>
      public const string WorkEndTimePropertyName = "WorkEndTime";

      /// <summary>
      ///    Le nom de la propriété <see cref="WorkStartTime" />.
      /// </summary>
      public const string WorkStartTimePropertyName = "WorkStartTime";

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
      ///    Dates à modifier
      /// </summary>
      private List<DateTime> datesToEdit;

      /// <summary>
      ///    Commande de suppression de l'évènement.
      /// </summary>
      private RelayCommand deleteInputDayCommand;

      /// <summary>
      ///    Le dialogresult de la page.
      /// </summary>
      private bool? dialogResult;

      /// <summary>
      ///    Les heures supplémentaires
      /// </summary>
      private int extraHours;

      private int idWorkPlace;

      /// <summary>
      ///    Commande de validation.
      /// </summary>
      private RelayCommand okCommand;

      /// <summary>
      ///    L'heure de fin de travail
      /// </summary>
      private DateTime workEndTime;

      /// <summary>
      ///    L'heure de début de travail
      /// </summary>
      private DateTime workStartTime;

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
      ///    LIdentifiant des évènement à charger.
      /// </param>
      public InputDayViewModel(ATimePlannerDataService service, IMessageboxService messageboxService, params DateTime[] model)
      {
         if (model == null || !model.Any())
         {
            return;
         }

         this.service = service;
         this.messageboxService = messageboxService;

         this.datesToEdit = model.ToList();
         IEnumerable<InputDay> inputDays = (from d in this.service.AllDays
                                            where d.WorkStartTime.HasValue && this.datesToEdit.Any(e => e.Date == d.WorkStartTime.Value.Date)
                                            select d).ToList();

         IEnumerable<int> idWorkplaces = inputDays.Select(d => d.IdWorkPlace ?? -1).Distinct().ToList();
         if (idWorkplaces.Count() == 1)
         {
            this.IdWorkPlace = idWorkplaces.First();
         }
         else
         {
            this.IdWorkPlace = -1;
         }

         IEnumerable<int> extraHoursInputs = inputDays.Select(d => d.ExtraHours ?? 0).Distinct().ToList();
         if (extraHoursInputs.Count() == 1)
         {
            this.ExtraHours = extraHoursInputs.First();
         }
         else
         {
            this.ExtraHours = 0;
         }

         IEnumerable<DateTime> startTime = inputDays.Select(d => d.WorkStartTime.Value).Distinct().ToList();
         if (startTime.Count() == 1)
         {
            this.WorkStartTime = startTime.First();
         }
         else
         {
            var dateTime = model.First();
            this.WorkStartTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0);
         }

         IEnumerable<DateTime> endTime = inputDays.Select(d => d.WorkEndTime.Value).Distinct().ToList();
         if (endTime.Count() == 1)
         {
            this.WorkEndTime = endTime.First();
         }
         else
         {
            var dateTime = model.First();
            this.WorkEndTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 0, 0);
         }
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
            if (this.service == null)
            {
               return new ObservableCollection<WorkPlace>();
            }
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
      ///    Obtient ou définit la liste des dates à modifier
      /// </summary>
      public List<DateTime> DatesToEdit
      {
         get
         {
            return this.datesToEdit;
         }
         set
         {
            this.Set(IdsToEditPropertyName, ref this.datesToEdit, value);
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
      ///    Obtient ou définit les heures supplémentaires
      /// </summary>
      public int ExtraHours
      {
         get
         {
            return this.extraHours;
         }
         set
         {
            this.Set(ExtraHoursPropertyName, ref this.extraHours, value);
         }
      }

      /// <summary>
      ///    Obtient ou définit l'identifiant du lieu de travail choisit
      /// </summary>
      public int IdWorkPlace
      {
         get
         {
            return this.idWorkPlace;
         }
         set
         {
            this.Set(IdWorkPlacePropertyName, ref this.idWorkPlace, value);
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

      /// <summary>
      ///    Obtient ou définit l'heure de fin de travail
      /// </summary>
      public DateTime WorkEndTime
      {
         get
         {
            return this.workEndTime;
         }
         set
         {
            this.Set(WorkEndTimePropertyName, ref this.workEndTime, value);
         }
      }

      /// <summary>
      ///    Obtient ou définit l'heure de début de travail
      /// </summary>
      public DateTime WorkStartTime
      {
         get
         {
            return this.workStartTime;
         }
         set
         {
            this.Set(WorkStartTimePropertyName, ref this.workStartTime, value);
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
         return this.service != null
                && this.service.AllDays.Any(
                   d => this.DatesToEdit != null && d.WorkStartTime.HasValue && this.DatesToEdit.Any(e => e.Date == d.WorkStartTime.Value.Date));
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
         if (addedItem != null)
         {
            this.WorkStartTime = new DateTime(
               this.WorkStartTime.Year,
               this.WorkStartTime.Month,
               this.WorkStartTime.Day,
               addedItem.DefaultStartTime.Hour,
               addedItem.DefaultStartTime.Minute,
               addedItem.DefaultStartTime.Second);
            this.WorkEndTime = new DateTime(
               this.WorkEndTime.Year,
               this.WorkEndTime.Month,
               this.WorkEndTime.Day,
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
         var inputDay = from d in this.service.AllDays
                        where d.WorkStartTime.HasValue && this.DatesToEdit.Any(e => e.Date == d.WorkStartTime.Value.Date)
                        select d;
         while (inputDay.Any())
         {
            this.service.AllDays.Remove(inputDay.ElementAt(0));
         }

         this.DialogResult = true;
      }

      /// <summary>
      ///    Execute la commande de validation.
      /// </summary>
      private void ExecuteOkCommand()
      {
         if (this.IdWorkPlace <= 0)
         {
            this.messageboxService.ShowMessagebox("Le lieu est obligatoire pour pouvoir sauvegarder", MessageboxKind.Ok, "Enregistrement impossible");
            return;
         }
         foreach (var date in this.DatesToEdit)
         {
            var day = this.service.AllDays.FirstOrDefault(d => d.WorkStartTime.Value.Date == date);
            if (day == null)
            {
               var idInput = this.service.AllDays.Any() ? this.service.AllDays.Max(d => d.ID) + 1 : 1;
               var input = new InputDay
                              {
                                 ID = idInput,
                                 WorkStartTime =
                                    new DateTime(
                                    date.Year,
                                    date.Month,
                                    date.Day,
                                    this.WorkStartTime.Hour,
                                    this.WorkStartTime.Minute,
                                    this.WorkStartTime.Second),
                                 WorkEndTime =
                                    new DateTime(
                                    date.Year,
                                    date.Month,
                                    date.Day,
                                    this.WorkEndTime.Hour,
                                    this.WorkEndTime.Minute,
                                    this.WorkEndTime.Second),
                                 ExtraHours = this.ExtraHours,
                                 IdWorkPlace = this.IdWorkPlace
                              };
               this.service.AllDays.Add(input);
            }
            else
            {
               day.ExtraHours = this.ExtraHours;
               day.IdWorkPlace = this.IdWorkPlace;
               day.WorkEndTime = new DateTime(
                  date.Year,
                  date.Month,
                  date.Day,
                  this.WorkEndTime.Hour,
                  this.WorkEndTime.Minute,
                  this.WorkEndTime.Second);
               day.WorkStartTime = new DateTime(
                  date.Year,
                  date.Month,
                  date.Day,
                  this.WorkStartTime.Hour,
                  this.WorkStartTime.Minute,
                  this.WorkStartTime.Second);
            }
         }

         this.DialogResult = true;
      }

      #endregion
   }
}