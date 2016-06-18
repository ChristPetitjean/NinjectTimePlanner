namespace TimePlannerNinject.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Media;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using TimePlannerNinject.Extensions;
    using TimePlannerNinject.Interfaces;
    using TimePlannerNinject.Model;

    public class EditWorkPlacesViewModel : ViewModelBase
    {
        public const string AllPlacesPropertyName = "AllPlaces";
        public const string SelectedWorkplacePropertyName = "SelectedWorkplace";

        private readonly ATimePlannerDataService service;

        private RelayCommand addNewWorkplaceCommand;

        private ObservableCollection<WorkPlace> allPlaces;

        private WorkPlace selectedWorkplace;

        public EditWorkPlacesViewModel(ATimePlannerDataService service)
        {
            this.service = service;
            this.allPlaces = new ObservableCollection<WorkPlace>(service.AllPlaces);
        }

        public RelayCommand AddNewWorkplaceCommand
        {
            get
            {
                return this.addNewWorkplaceCommand
                       ?? (this.addNewWorkplaceCommand = new RelayCommand(this.ExecuteAddNewWorkplaceCommand));
            }
        }

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

        private void ExecuteAddNewWorkplaceCommand()
        {
            var id = this.AllPlaces.Any() ? (from p in this.AllPlaces select p.Id).Max() + 1 : 1;
            WorkPlace newWorkPlace = new WorkPlace
                                         {
                                             Id = id,
                                             DefaultStartTime = new DateTime(1, 1, 1, 8, 0, 0),
                                             DefaultEndTime = new DateTime(1, 1, 1, 17, 0, 0),
                                             Color = Colors.LightGreen,
                                             OneWayKilometers = 0,
                                             ReturnKilometers = 0,
                                             Name = "Nom temporaire"
                                         };
            this.AllPlaces.Add(newWorkPlace);
            this.SelectedWorkplace = newWorkPlace;
        }

        public WorkPlace SelectedWorkplace
        {
            get
            {
                return this.selectedWorkplace;
            }
            set
            {
                this.Set(SelectedWorkplacePropertyName, ref this.selectedWorkplace, value);
            }
        }
    }
}