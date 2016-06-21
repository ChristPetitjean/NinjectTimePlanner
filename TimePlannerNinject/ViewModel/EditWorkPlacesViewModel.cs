// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditWorkPlacesViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   ViewModel d'édition des lieux de travail.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.ViewModel
{
   using System;
   using System.Collections.ObjectModel;
   using System.Linq;
   using System.Windows.Media;

   using GalaSoft.MvvmLight;
   using GalaSoft.MvvmLight.CommandWpf;

   using TimePlannerNinject.Model;
   using TimePlannerNinject.Services;

   /// <summary>
   ///    ViewModel d'édition des lieux de travail.
   /// </summary>
   public class EditWorkPlacesViewModel : ViewModelBase
   {
      #region Fields

      /// <summary>
      ///    Service de données.
      /// </summary>
      private readonly ATimePlannerDataService service;

      /// <summary>
      ///    Commande d'ajout de nouveau lieu.
      /// </summary>
      private RelayCommand addNewWorkplaceCommand;

      /// <summary>
      ///    Command de suppression de lieu.
      /// </summary>
      private RelayCommand<int> deleteWorkPlaceCommand;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      ///    Initialise une nouvelle instance de la classe <see cref="EditWorkPlacesViewModel" />.
      /// </summary>
      /// <param name="service">
      ///    Le service de données.
      /// </param>
      public EditWorkPlacesViewModel(ATimePlannerDataService service)
      {
         this.service = service;
         this.service.DataReadEnd += this.ServiceDataReadEnd;
      }

      #endregion

      #region Public Properties

      /// <summary>
      ///    Obtient la commande d'ajout de nouveau lieu.
      /// </summary>
      public RelayCommand AddNewWorkplaceCommand
      {
         get
         {
            return this.addNewWorkplaceCommand ?? (this.addNewWorkplaceCommand = new RelayCommand(this.ExecuteAddNewWorkplaceCommand));
         }
      }

      /// <summary>
      ///    Obitent ou définit la collection de lieu
      /// </summary>
      public ObservableCollection<WorkPlace> AllPlaces
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
      ///    Obtient la commande de suppression de lieu.
      /// </summary>
      public RelayCommand<int> DeleteWorkPlaceCommand
      {
         get
         {
            return this.deleteWorkPlaceCommand ?? (this.deleteWorkPlaceCommand = new RelayCommand<int>(this.ExecuteDeleteWorkPlaceCommand));
         }
      }

      #endregion

      #region Methods

      /// <summary>
      ///    Execute la commande d'ajout de nouveau lieu.
      /// </summary>
      private void ExecuteAddNewWorkplaceCommand()
      {
         var id = this.AllPlaces.Any()
                     ? (from p in this.AllPlaces
                        select p.Id).Max() + 1
                     : 1;
         var newWorkPlace = new WorkPlace
                               {
                                  Id = id,
                                  DefaultStartTime = new DateTime(1, 1, 1, 8, 0, 0),
                                  DefaultEndTime = new DateTime(1, 1, 1, 17, 0, 0),
                                  Color = this.GenerateRandomColor(),
                                  OneWayKilometers = 0,
                                  ReturnKilometers = 0,
                                  Name = id.ToString()
                               };
         this.AllPlaces.Add(newWorkPlace);
      }

      /// <summary>
      /// Génère une coleur aléatoire.
      /// </summary>
      /// <returns>
      ///   La couleur générée
      /// </returns>
      private Color GenerateRandomColor()
      {
         Random rnd = new Random();
         return new Color
                   {
                      A = 50,
                      B = Convert.ToByte(rnd.Next(1, 255)),
                      G = Convert.ToByte(rnd.Next(1, 255)),
                      R = Convert.ToByte(rnd.Next(1, 255)),
                   };
      }

      /// <summary>
      ///    Execute la command de suppression de lieu.
      /// </summary>
      /// <param name="id">
      ///    Identifiant du lieu.
      /// </param>
      private void ExecuteDeleteWorkPlaceCommand(int id)
      {
         var workPlace = this.service.AllPlaces.First(d => d.Id == id);
         this.service.AllPlaces.Remove(workPlace);
      }

      /// <summary>
      ///    Evènement de fin de lecture d'un nouveau fichier.
      /// </summary>
      /// <param name="sender">Objet ayant levé l'évènement.</param>
      /// <param name="e">The <see cref="EventArgs" />Arguments de l'évènement.</param>
      private void ServiceDataReadEnd(object sender, EventArgs e)
      {
         this.AllPlaces = new ObservableCollection<WorkPlace>(this.service.AllPlaces);
      }

      #endregion
   }
}