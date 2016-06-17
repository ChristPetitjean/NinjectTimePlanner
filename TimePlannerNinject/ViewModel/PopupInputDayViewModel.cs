using GalaSoft.MvvmLight;

namespace TimePlannerNinject.ViewModel
{
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight.CommandWpf;

    using TimePlannerNinject.Interfaces;
    using TimePlannerNinject.Model;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PopupInputDayViewModel : ViewModelBase
    {
        private readonly ATimePlannerDataService service;
        /// <summary>
        /// Initializes a new instance of the PopupInputDayViewModel class.
        /// </summary>
        public PopupInputDayViewModel(ATimePlannerDataService service)
        {
            this.service = service;
        }

        /// <summary>
        /// The <see cref="Input" /> property's name.
        /// </summary>
        public const string InputPropertyName = "Input";

        private InputDay input = new InputDay();

        /// <summary>
        /// Sets and gets the Input property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public InputDay Input
        {
            get
            {
                return input;
            }
            set
            {
                Set(InputPropertyName, ref input, value);
            }
        }

        private RelayCommand okCommand;

        /// <summary>
        /// Gets the OkCommand.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return okCommand
                    ?? (okCommand = new RelayCommand(ExecuteOkCommand));
            }
        }

        private void ExecuteOkCommand()
        {
            this.DialogResult = true;
        }

        private RelayCommand cancelCommand;

        /// <summary>
        /// Gets the CancelCommand.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand
                    ?? (cancelCommand = new RelayCommand(ExecuteCancelCommand));
            }
        }

        private void ExecuteCancelCommand()
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// The <see cref="DialogResult" /> property's name.
        /// </summary>
        public const string DialogResultPropertyName = "DialogResult";

        private bool? dialogResult = null;

        /// <summary>
        /// Sets and gets the DialogResult property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }
            set
            {
                Set(DialogResultPropertyName, ref dialogResult, value);
            }
        }

        public ObservableCollection<WorkPlace> AllWorkPlace
        {
            get
            {
                return this.service.AllPlaces;
            }
        }
    }
}