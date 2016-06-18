﻿// --------------------------------------------------------------------------------------------------------------------
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

    using TimePlannerNinject.Interfaces;
    using TimePlannerNinject.Model;

    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class InputDayViewModel : ViewModelBase
    {
        /// <summary>
        ///     The <see cref="DialogResult" /> property's name.
        /// </summary>
        public const string DialogResultPropertyName = "DialogResult";

        /// <summary>
        ///     The <see cref="Input" /> property's name.
        /// </summary>
        public const string InputPropertyName = "Input";

        /// <summary>
        /// The service.
        /// </summary>
        private readonly ATimePlannerDataService service;

        /// <summary>
        /// The all places selection change command.
        /// </summary>
        private RelayCommand<SelectionChangedEventArgs> allPlacesSelectionChangeCommand;

        /// <summary>
        /// The cancel command.
        /// </summary>
        private RelayCommand cancelCommand;

        /// <summary>
        /// The dialog result.
        /// </summary>
        private bool? dialogResult;

        /// <summary>
        /// The input.
        /// </summary>
        private InputDay input = new InputDay();

        /// <summary>
        /// The ok command.
        /// </summary>
        private RelayCommand okCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDayViewModel"/> class. 
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public InputDayViewModel(ATimePlannerDataService service, InputDay model)
        {
            this.service = service;
            this.input = (InputDay)model.Clone();
        }

        /// <summary>
        ///     Gets the AllPlacesSelectionChangeCommand.
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> AllPlacesSelectionChangeCommand
        {
            get
            {
                return this.allPlacesSelectionChangeCommand
                       ?? (this.allPlacesSelectionChangeCommand =
                           new RelayCommand<SelectionChangedEventArgs>(this.ExecuteAllPlacesSelectionChangeCommand));
            }
        }

        /// <summary>
        /// Gets or sets the all work place.
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
        ///     Gets the CancelCommand.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return this.cancelCommand ?? (this.cancelCommand = new RelayCommand(this.ExecuteCancelCommand));
            }
        }

        /// <summary>
        ///     Sets and gets the DialogResult property.
        ///     Changes to that property's value raise the PropertyChanged event.
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
        ///     Sets and gets the Input property.
        ///     Changes to that property's value raise the PropertyChanged event.
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
        ///     Gets the OkCommand.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return this.okCommand ?? (this.okCommand = new RelayCommand(this.ExecuteOkCommand));
            }
        }

        /// <summary>
        /// The execute all places selection change command.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ExecuteAllPlacesSelectionChangeCommand(SelectionChangedEventArgs e)
        {
            WorkPlace addedItem = (WorkPlace)e.AddedItems[0];
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

        /// <summary>
        /// The execute cancel command.
        /// </summary>
        private void ExecuteCancelCommand()
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// The execute ok command.
        /// </summary>
        private void ExecuteOkCommand()
        {
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
    }
}