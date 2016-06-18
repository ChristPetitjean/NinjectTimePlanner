// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditWorkPlacesViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   The edit work places view model.
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

    using TimePlannerNinject.Interfaces;
    using TimePlannerNinject.Model;

    /// <summary>
    /// The edit work places view model.
    /// </summary>
    public class EditWorkPlacesViewModel : ViewModelBase
    {
        /// <summary>
        /// The service.
        /// </summary>
        private readonly ATimePlannerDataService service;

        /// <summary>
        /// The add new workplace command.
        /// </summary>
        private RelayCommand addNewWorkplaceCommand;

        /// <summary>
        /// The delete work place command.
        /// </summary>
        private RelayCommand<int> deleteWorkPlaceCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditWorkPlacesViewModel"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public EditWorkPlacesViewModel(ATimePlannerDataService service)
        {
            this.service = service;
            this.service.DataReadEnd += this.ServiceDataReadEnd;
        }

        private void ServiceDataReadEnd(object sender, EventArgs e)
        {
            this.AllPlaces = new ObservableCollection<WorkPlace>(this.service.AllPlaces);
        }

        /// <summary>
        /// Gets the add new workplace command.
        /// </summary>
        public RelayCommand AddNewWorkplaceCommand
        {
            get
            {
                return this.addNewWorkplaceCommand
                       ?? (this.addNewWorkplaceCommand = new RelayCommand(this.ExecuteAddNewWorkplaceCommand));
            }
        }

        /// <summary>
        /// Gets or sets the all places.
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
        ///     Gets the DeleteWorkPlaceCommand.
        /// </summary>
        public RelayCommand<int> DeleteWorkPlaceCommand
        {
            get
            {
                return this.deleteWorkPlaceCommand
                       ?? (this.deleteWorkPlaceCommand = new RelayCommand<int>(this.ExecuteDeleteWorkPlaceCommand));
            }
        }

        /// <summary>
        /// The execute add new workplace command.
        /// </summary>
        private void ExecuteAddNewWorkplaceCommand()
        {
            var id = this.AllPlaces.Any() ? (from p in this.AllPlaces select p.Id).Max() + 1 : 1;
            var newWorkPlace = new WorkPlace
                                   {
                                       Id = id, 
                                       DefaultStartTime = new DateTime(1, 1, 1, 8, 0, 0), 
                                       DefaultEndTime = new DateTime(1, 1, 1, 17, 0, 0), 
                                       Color = Colors.White, 
                                       OneWayKilometers = 0, 
                                       ReturnKilometers = 0, 
                                       Name = id.ToString()
                                   };
            this.AllPlaces.Add(newWorkPlace);
        }

        /// <summary>
        /// The execute delete work place command.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        private void ExecuteDeleteWorkPlaceCommand(int id)
        {
            var workPlace = this.service.AllPlaces.First(d => d.Id == id);
            this.service.AllPlaces.Remove(workPlace);
        }
    }
}