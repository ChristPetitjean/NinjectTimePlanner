﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonthView.xaml.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.UserControl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    using TimePlannerNinject.Extensions;
    using TimePlannerNinject.Model;

    using Xceed.Wpf.AvalonDock.Controls;

    using Calendar = System.Globalization.Calendar;

    /// <summary>
    ///     Logique d'interaction pour MonthView.xaml
    /// </summary>
    public partial class MonthView
    {
        #region Static Fields

        /// <summary>
        ///     Identifies la propriété de dependance <see cref="Appointments" />.
        /// </summary>
        public static readonly DependencyProperty AppointmentsProperty = DependencyProperty.Register(
            nameof(Appointments), 
            typeof(ObservableCollection<InputDay>), 
            typeof(MonthView), 
            new PropertyMetadata(OnAppointmentsChanged));

        /// <summary>
        ///     Identifies la propriété de dependance <see cref="DisplayStartDate" />.
        /// </summary>
        public static readonly DependencyProperty DisplayStartDateProperty = DependencyProperty.Register(
            nameof(DisplayStartDate), 
            typeof(DateTime), 
            typeof(MonthView), 
            new PropertyMetadata(OnDisplayStartDateChanged));

        /// <summary>
        ///     Propriété de dépendance des dates sélectionnées
        /// </summary>
        public static readonly DependencyProperty SelectedDatesProperty = DependencyProperty.Register(
            "SelectedDates", 
            typeof(ObservableCollection<DateTime>), 
            typeof(MonthView), 
            new PropertyMetadata(OnSelectedIdsChanged));

        /// <summary>
        ///     Identifies la propriété de dependance <see cref="IsAllEventsSelected" />.
        /// </summary>
        public static readonly DependencyProperty IsAllEventsSelectedProperty = DependencyProperty.Register(
            nameof(IsAllEventsSelected), 
            typeof(bool), 
            typeof(MonthView), 
            new PropertyMetadata(OnIsAllEventsSelectedChanged));

        #endregion

        #region Fields

        /// <summary>
        ///     Pinceau du fond d'un emplacement de jour.
        /// </summary>
        private readonly Brush dayBackBrush;

        /// <summary>
        ///     Calendrier system.
        /// </summary>
        private readonly Calendar sysCal;

        /// <summary>
        ///     Pinceau du fond de la cible.
        /// </summary>
        private readonly Brush targetBackBrush;

        /// <summary>
        ///     Pinceau du fond du jour sélectionné.
        /// </summary>
        private readonly Brush todayBackBrush;

        /// <summary>
        ///     Pinceau du fond du jour sélectionné.
        /// </summary>
        private readonly Brush todayStackBrush;

        /// <summary>
        ///     Mois affiché.
        /// </summary>
        private int displayMonth;

        /// <summary>
        ///     Année affichée.
        /// </summary>
        private int displayYear;

        /// <summary>
        ///     Définit si l'initialisation de la sélection se fait depuis cette meme classe
        /// </summary>
        private bool isInitializedFromThis;

        /// <summary>
        ///     Le dernier jour sur lequel l'utilisateur à cliqué
        /// </summary>
        private int lastDayClicked;

        /// <summary>
        ///     Définit le sens de la derniere sélection multiple par shift
        /// </summary>
        private bool lastDayClickIsUp;
        /// <summary>
        ///     Définit le dernier jour selectionner par appui clavier
        /// </summary>
        private int lastDayKeyPressed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="MonthView" />.
        /// </summary>
        public MonthView()
        {
            this.todayBackBrush = (Brush)this.TryFindResource("TodayBackBrush");
            this.dayBackBrush = (Brush)this.TryFindResource("DayBackBrush");
            this.targetBackBrush = (Brush)this.TryFindResource("TargetBackBrush");
            this.todayStackBrush = (Brush)this.TryFindResource("TodayStackBackBrush");

            this.displayMonth = this.DisplayStartDate.Month;
            this.displayYear = this.DisplayStartDate.Year;
            var cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            this.sysCal = cultureInfo.Calendar;
            this.Appointments = new ObservableCollection<InputDay>();
            this.DisplayStartDate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1));
            this.IsAllEventsSelected = false;

            this.Loaded += this.OnMonthViewLoaded;

            this.InitializeComponent();

            FocusManager.SetFocusedElement(Application.Current.MainWindow, this);
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Levé lors du double click sur un évènement.
        /// </summary>
        public event EventHandler<AppointmentDblClickedEvenArgs> AppointmentDblClicked;

        /// <summary>
        ///     Levé lors du déplacement d'un évènement.
        /// </summary>
        public event EventHandler<AppointmentMovedEvenArgs> AppointmentMoved;

        /// <summary>
        ///     Levé lors du double click sur un emplacement vide.
        /// </summary>
        public event EventHandler<NewAppointmentEventArgs> DayBoxDoubleClicked;

        /// <summary>
        ///     Levé lors du changement de mois affiché.
        /// </summary>
        public event EventHandler<MonthChangedEventArgs> DisplayMonthChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Obtient ou définit la liste des <see cref="Appointments" />
        /// </summary>
        public ObservableCollection<InputDay> Appointments
        {
            get
            {
                return (ObservableCollection<InputDay>)this.GetValue(AppointmentsProperty);
            }

            set
            {
                this.SetValue(AppointmentsProperty, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit la date de début
        /// </summary>
        public DateTime DisplayStartDate
        {
            get
            {
                return (DateTime)this.GetValue(DisplayStartDateProperty);
            }

            set
            {
                this.SetValue(DisplayStartDateProperty, value);
            }
        }

        /// <summary>
        ///     Obtient ou modifie une valeur indiquant si tout les évènements sont sélectionnés ou non
        /// </summary>
        public bool IsAllEventsSelected
        {
            get
            {
                return (bool)this.GetValue(IsAllEventsSelectedProperty);
            }

            set
            {
                this.SetValue(IsAllEventsSelectedProperty, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit les dates sélectionnées
        /// </summary>
        public ObservableCollection<DateTime> SelectedDates
        {
            get
            {
                return (ObservableCollection<DateTime>)this.GetValue(SelectedDatesProperty);
            }

            set
            {
                this.SetValue(SelectedDatesProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Levée lors d'une modification de <see cref="Appointments" />.
        /// </summary>
        /// <param name="d">La propriété de dépendance.</param>
        /// <param name="e">L'instance de <see cref="DependencyPropertyChangedEventArgs" /> contenant les données de l'évènement.</param>
        private static void OnAppointmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var action = new NotifyCollectionChangedEventHandler((o, args) => { RebuildUI(d); });

            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged)e.OldValue;
                coll.CollectionChanged -= action;
            }

            if (e.NewValue != null)
            {
                var coll = (INotifyCollectionChanged)e.NewValue;
                coll.CollectionChanged += action;
            }

            RebuildUI(d);
        }

        /// <summary>
        ///     Levée lors d'une modification de <see cref="DisplayStartDate" />.
        /// </summary>
        /// <param name="d">La propriété de dépendance.</param>
        /// <param name="e">L'instance de <see cref="DependencyPropertyChangedEventArgs" /> contenant les données de l'évènement.</param>
        private static void OnDisplayStartDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as MonthView;
            if (me != null)
            {
                me.displayMonth = ((DateTime)e.NewValue).Month;
                me.displayYear = ((DateTime)e.NewValue).Year;
            }
        }

        /// <summary>
        ///     Levée lors d'une modification de <see cref="IsAllEventsSelected" />.
        /// </summary>
        /// <param name="d">La propriété de dépendance.</param>
        /// <param name="e">L'instance de <see cref="DependencyPropertyChangedEventArgs" /> contenant les données de l'évènement.</param>
        private static void OnIsAllEventsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as MonthView;
            if (me?.SelectedDates != null)
            {
                if (!me.isInitializedFromThis)
                {
                    me.SelectedDates.Clear();
                    if ((bool)e.NewValue)
                    {
                        for (var i = 1; i <= me.sysCal.GetDaysInMonth(me.displayYear, me.displayMonth); i++)
                        {
                            me.SelectedDates.Add(new DateTime(me.displayYear, me.displayMonth, i));
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Levée lors d'un changement dans la propriété de dépendance <see cref="SelectedDates" />.
        /// </summary>
        /// <param name="d">La propriété de dépendance.</param>
        /// <param name="e">L'instance de <see cref="DependencyPropertyChangedEventArgs" /> contenant les données de l'évènement.</param>
        private static void OnSelectedIdsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var action = new NotifyCollectionChangedEventHandler(
                (o, args) =>
                    {
                        var me = d as MonthView;
                        if (me != null && me.IsLoaded)
                        {
                            var isAllSelected = true;
                            foreach (var dayBoxControl in me.FindVisualChildren<DayBoxControl>())
                            {
                                if (
                                    me.SelectedDates.Any(
                                        s => s.Date.Day == (int)dayBoxControl.Tag && s.Date.Month == me.displayMonth && s.Date.Year == me.displayYear))
                                {
                                    dayBoxControl.MainBorder.BorderThickness = new Thickness(2);
                                }
                                else
                                {
                                    dayBoxControl.MainBorder.BorderThickness = new Thickness(0);
                                    isAllSelected = false;
                                }
                            }

                            if (me.IsAllEventsSelected != isAllSelected)
                            {
                                me.isInitializedFromThis = true;
                                me.IsAllEventsSelected = isAllSelected;
                                me.isInitializedFromThis = false;
                            }
                        }
                    });

            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged)e.OldValue;
                coll.CollectionChanged -= action;
            }

            if (e.NewValue != null)
            {
                var coll = (INotifyCollectionChanged)e.NewValue;
                coll.CollectionChanged += action;
            }
        }

        /// <summary>
        ///     Lance une regénération de l'UI
        /// </summary>
        /// <param name="d">la propriété de dépandance contenant <see cref="MonthView" />.</param>
        // ReSharper disable once InconsistentNaming
        private static void RebuildUI(DependencyObject d)
        {
            var me = d as MonthView;
            if (me != null && me.IsLoaded)
            {
                me.BuildCalendarUI();
            }
        }

        /// <summary>
        ///     Ajoute les lignes correspondants aux semaines dans la grille des mois.
        /// </summary>
        /// <param name="dayInMounth">
        ///     Nombre de jours dans le mois.
        /// </param>
        /// <param name="offsetDays">
        ///     Le décalage de jours par rapport a l'enumeration de jours.
        /// </param>
        private void AddRowsToMonthGrid(int dayInMounth, int offsetDays)
        {
            this.MonthViewGrid.RowDefinitions.Clear();

            var rowHeigth = new GridLength(60, GridUnitType.Star);
            var endOffsetDays = 7
                                - (int)
                                  Enum.ToObject(
                                      typeof(DayOfWeek), 
                                      new DateTime(this.displayYear, this.displayMonth, 1).AddDays(dayInMounth - 1).DayOfWeek) + 1;

            for (var i = 0; i < (dayInMounth + offsetDays + endOffsetDays) / 7; i++)
            {
                var rowDef = new RowDefinition { Height = rowHeigth };
                this.MonthViewGrid.RowDefinitions.Add(rowDef);
            }
        }

        /// <summary>
        ///     Construit l'UI.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private void BuildCalendarUI()
        {
            var daysInMonth = this.sysCal.GetDaysInMonth(this.displayYear, this.displayMonth);
            var offsetDays = (int)Enum.ToObject(typeof(DayOfWeek), new DateTime(this.displayYear, this.displayMonth, 1).DayOfWeek) - 1;
            if (offsetDays == -1)
            {
                offsetDays = 6;
            }

            var weekCount = 0;
            var weekRowCtrl = new WeekOfDaysControls();

            // On efface tous les controles enfant et les enregistremetns
            this.MonthViewGrid.Children.Clear();
            NameScope.SetNameScope(this, new NameScope());

            this.AddRowsToMonthGrid(daysInMonth, offsetDays);
            this.MonthYearLabel.Content = this.DisplayStartDate.ToString("MMMM yyyy").ToPascalCase();

            for (var i = 1; i <= daysInMonth; i++)
            {
                if (i != 1 && Math.Abs(Math.IEEERemainder(i + offsetDays - 1, 7)) < 0.001)
                {
                    Grid.SetRow(weekRowCtrl, weekCount);
                    this.MonthViewGrid.Children.Add(weekRowCtrl);

                    weekRowCtrl = new WeekOfDaysControls();
                    weekCount += 1;
                }

                var contextextMenuItem = new MenuItem { Header = "Ajouter / Modifier la sélection" };

                var dayBox = new DayBoxControl { Name = "DayBox" + i, DayNumberLabel = { Content = i.ToString() }, Tag = i };
                dayBox.MouseDoubleClick += this.OnDayBoxMouseDoubleClick;
                dayBox.PreviewDragEnter += this.OnDayBoxPreviewDragEnter;
                dayBox.PreviewDragLeave += this.OnDayBoxPreviewDragLeave;
                dayBox.MouseUp += this.OnDayBoxMouseUp;

                contextextMenuItem.Click +=
                    (sender, e) => { this.OnDayBoxMouseDoubleClick(dayBox, new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)); };

                dayBox.ContextMenu = new ContextMenu { Items = { contextextMenuItem } };

                NameScope.SetNameScope(dayBox, new NameScope());
                this.RegisterName("DayBox" + i, dayBox);

                NameScope.SetNameScope(dayBox, new NameScope());
                this.RegisterName("DayBox" + i, dayBox);

                if (new DateTime(this.displayYear, this.displayMonth, i) == DateTime.Today)
                {
                    dayBox.DayLabelRowBorder.Background = this.todayBackBrush;
                    dayBox.DayAppointmentsStack.Background = this.todayStackBrush;
                }

                if (this.Appointments != null)
                {
                    var day = i;
                    var aptDay = from d in this.Appointments
                                 where
                                     d.WorkStartTime.HasValue && d.WorkStartTime.Value.Day == day && d.WorkStartTime.Value.Month == this.displayMonth
                                     && d.WorkStartTime.Value.Year == this.displayYear
                                 select d;

                    foreach (var a in aptDay)
                    {
                        var apt = new DayBoxAppointmentControl { Name = "Apt" + a.ID, DataContext = a };
                        apt.MouseDoubleClick += this.OnAptMouseDoubleClick;
                        dayBox.DayAppointmentsStack.Children.Add(apt);
                        dayBox.RegisterName("Apt" + a.ID, apt);
                    }
                }

                Grid.SetColumn(dayBox, i - weekCount * 7 + offsetDays);
                weekRowCtrl.WeekRowGrid.Children.Add(dayBox);
            }

            Grid.SetRow(weekRowCtrl, weekCount);
            this.MonthViewGrid.Children.Add(weekRowCtrl);
        }

        /// <summary>
        ///     Gère la multisélection à l'appui d'une touche lorsque qu'aucune touche n'est maintenue
        /// </summary>
        /// <param name="key">
        ///     Touche appuyée
        /// </param>
        private void MultiSelectOnArrowKeyDownNormal(Key key)
        {
            int nbDays = this.sysCal.GetDaysInMonth(this.displayYear, this.displayMonth);
            int day;
            if (key == Key.Right)
            {
                day = this.lastDayClicked != 0 ? this.lastDayClicked + 1 : 1;
                if (day > nbDays)
                {
                    return;
                }
            }
            else if (key == Key.Left)
            {
                day = this.lastDayClicked != 0 ? this.lastDayClicked - 1 : 1;
                if (day < 1)
                {
                    return;
                }
            }
            else if (key == Key.Up)
            {
                day = this.lastDayClicked != 0 ? this.lastDayClicked - 7 : 1;
                if (day < 1)
                {
                    return;
                }
            }
            else if (key == Key.Down)
            {
                day = this.lastDayClicked != 0 ? this.lastDayClicked + 7 : 1;
                if (day > nbDays)
                {
                    return;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            this.SelectedDates.Clear();
            this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
            this.lastDayClicked = day;
            this.lastDayKeyPressed = day;
        }

        /// <summary>
        ///     Gère la multisélection à l'appui d'une touche lorsque que la touche Ctrl est maintenue
        /// </summary>
        /// <param name="key">
        ///     Touche appuyée
        /// </param>
        private void MultiSelectOnArrowKeyDownUsingCtrl(Key key)
        {
            int nbDays = this.sysCal.GetDaysInMonth(this.displayYear, this.displayMonth);
            int day;
            if (key == Key.Right)
            {
                day = this.lastDayClicked != 0 ? this.lastDayClicked + 1 : 1;
                if (day > nbDays)
                {
                    return;
                }
            }
            else if (key == Key.Left)
            {
                day = this.lastDayClicked != 0 ? this.lastDayClicked - 1 : 1;
                if (day < 1)
                {
                    return;
                }
            }
            else if (key == Key.Up)
            {
                day = this.lastDayClicked != 0 ? this.lastDayClicked - 7 : 1;
                if (day < 1)
                {
                    return;
                }
            }
            else if (key == Key.Down)
            {
                day = this.lastDayClicked != 0 ? this.lastDayClicked + 7 : 1;
                if (day > nbDays)
                {
                    return;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            var dateToSelect = new DateTime(this.displayYear, this.displayMonth, day);

            var firstOrDefault = this.SelectedDates.FirstOrDefault(d => d.Date == dateToSelect.Date);
            if (firstOrDefault != default(DateTime))
            {
                this.SelectedDates.Remove(firstOrDefault);
            }
            else
            {
                this.SelectedDates.Add(dateToSelect);
            }

            this.lastDayClicked = day;
            this.lastDayKeyPressed = day;
        }

        /// <summary>
        ///     Gère la multisélection à l'appui d'une touche lorsque que la touche Shift est maintenue
        /// </summary>
        /// <param name="key">
        ///     Touche appuyée
        /// </param>
        private void MultiSelectOnArrowKeyDownUsingShift(Key key)
        {
            int nbDays = this.sysCal.GetDaysInMonth(this.displayYear, this.displayMonth);
            int day;
            if (key == Key.Right)
            {
                day = this.lastDayKeyPressed != 0 ? this.lastDayKeyPressed + 1 : this.lastDayClicked != 0 ? this.lastDayClicked : nbDays;
                if (day > nbDays)
                {
                    return;
                }
            }
            else if (key == Key.Left)
            {
                day = this.lastDayKeyPressed != 0 ? this.lastDayKeyPressed - 1 : this.lastDayClicked != 0 ? this.lastDayClicked : 1;
                if (day < 1)
                {
                    return;
                }
            }
            else if (key == Key.Up)
            {
                day = this.lastDayKeyPressed != 0 ? this.lastDayKeyPressed - 7 : this.lastDayClicked != 0 ? this.lastDayClicked : 1;
                if (day < 1)
                {
                    return;
                }
            }
            else if (key == Key.Down)
            {
                day = this.lastDayKeyPressed != 0 ? this.lastDayKeyPressed + 7 : this.lastDayClicked != 0 ? this.lastDayClicked : nbDays;
                if (day > nbDays)
                {
                    return;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            if (this.lastDayClicked != 0)
            {
                if (this.lastDayClicked < day)
                {
                    if (!this.lastDayClickIsUp)
                    {
                        this.SelectedDates.Clear();
                    }

                    for (var i = this.lastDayClicked; i <= day; i++)
                    {
                        this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, i));
                    }

                    this.lastDayClickIsUp = true;
                }
                else if (this.lastDayClicked > day)
                {
                    if (this.lastDayClickIsUp)
                    {
                        this.SelectedDates.Clear();
                    }

                    for (var i = day; i <= this.lastDayClicked; i++)
                    {
                        this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, i));
                    }

                    this.lastDayClickIsUp = false;
                }
                else
                {
                    this.SelectedDates.Clear();
                    this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
                }
            }
            else
            {
                this.SelectedDates.Clear();
                this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
            }
            this.lastDayClickIsUp = key == Key.Up || key == Key.Right;
            this.lastDayKeyPressed = day;
        }

        /// <summary>
        ///     Gère la multisélection au click lorsque qu'aucune touche n'est maintenue
        /// </summary>
        /// <param name="day">
        ///     Le jour cliqué
        /// </param>
        private void MultiSelectOnClickNormal(int day)
        {
            this.SelectedDates.Clear();
            this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
            this.lastDayClicked = day;
            this.lastDayKeyPressed = day;
        }

        /// <summary>
        ///     Gère la multisélection au click lorsque que la touche Ctrl est maintenue
        /// </summary>
        /// <param name="day">
        ///     Le jour cliqué
        /// </param>
        private void MultiSelectOnClickUsingCtrl(int day)
        {
            if (!this.SelectedDates.Any(s => s.Date.Day == day && s.Date.Month == this.displayMonth && s.Date.Year == this.displayYear))
            {
                this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
            }
            else
            {
                var dateTime = this.SelectedDates.First(d => d.Year == this.displayYear && d.Month == this.displayMonth && d.Day == day);
                this.SelectedDates.Remove(dateTime);
            }

            this.lastDayClicked = day;
            this.lastDayKeyPressed = day;
        }

        /// <summary>
        ///     Gère la multisélection au click lorsque que la touche Shift est maintenue
        /// </summary>
        /// <param name="day">
        ///     Le jour cliqué
        /// </param>
        private void MultiSelectOnClickUsingShift(int day)
        {
            if (this.lastDayClicked != 0)
            {
                if (this.lastDayClicked < day)
                {
                    if (!this.lastDayClickIsUp)
                    {
                        this.SelectedDates.Clear();
                    }

                    for (var i = this.lastDayClicked; i <= day; i++)
                    {
                        this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, i));
                    }

                    this.lastDayClickIsUp = true;
                }
                else if (this.lastDayClicked > day)
                {
                    if (this.lastDayClickIsUp)
                    {
                        this.SelectedDates.Clear();
                    }

                    for (var i = day; i <= this.lastDayClicked; i++)
                    {
                        this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, i));
                    }

                    this.lastDayClickIsUp = false;
                }
                else
                {
                    this.SelectedDates.Clear();
                    this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
                }
            }
            else
            {
                this.SelectedDates.Clear();
                this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
            }

            this.lastDayKeyPressed = day;
        }

        /// <summary>
        ///     Levé lors du double click sur un évènement.
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement.
        /// </param>
        /// <param name="e">
        ///     Arguments de l'évènement.
        /// </param>
        private void OnAptMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((e.Source as DayBoxAppointmentControl)?.Tag != null)
            {
                if (this.AppointmentDblClicked != null)
                {
                    var idApt = (int)((DayBoxAppointmentControl)e.Source).Tag;
                    var arg = new AppointmentDblClickedEvenArgs { Id = idApt };
                    this.AppointmentDblClicked(e.Source, arg);
                }
            }

            e.Handled = true;
        }

        /// <summary>
        ///     Levé lors du double click sur un emplacement d'évènement.
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement.
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="MouseButtonEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnDayBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DayBoxControl && e.ChangedButton == MouseButton.Right)
            {
                var dayBoxControl = (DayBoxControl)e.Source;
                var day = (int)((DayBoxControl)e.Source).Tag;
                dayBoxControl.ContextMenu.Visibility = this.SelectedDates != null && this.SelectedDates.Any(d => d.Day == day)
                                                           ? Visibility.Visible
                                                           : Visibility.Hidden;
            }
            else if (e.Source is DayBoxControl && ((Visual)e.OriginalSource).FindVisualAncestor<DayBoxAppointmentControl>() == null
                     && e.LeftButton == MouseButtonState.Pressed)
            {
                var ev = new NewAppointmentEventArgs();
                if (((DayBoxControl)e.Source).Tag != null)
                {
                    ev.StartDate = new DateTime(this.displayYear, this.displayMonth, (int)((DayBoxControl)e.Source).Tag);
                    ev.EndDate = (DateTime)ev.StartDate;
                }

                this.DayBoxDoubleClicked?.Invoke(sender, ev);

                e.Handled = true;
            }
            else if (sender is DayBoxControl)
            {
                var dayBoxControl = (DayBoxControl)sender;
                var ev = new NewAppointmentEventArgs();
                if (dayBoxControl.Tag != null)
                {
                    ev.StartDate = new DateTime(this.displayYear, this.displayMonth, (int)dayBoxControl.Tag);
                    ev.EndDate = (DateTime)ev.StartDate;
                }

                this.DayBoxDoubleClicked?.Invoke(sender, ev);
            }
        }

        /// <summary>
        ///     Levé lors du click sur un emplacement d'évènement
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement.
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="MouseButtonEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnDayBoxMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DayBoxControl)
            {
                var day = (int)((DayBoxControl)e.Source).Tag;
                if (e.ChangedButton == MouseButton.Left)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        this.MultiSelectOnClickUsingShift(day);
                    }
                    else if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        this.MultiSelectOnClickUsingCtrl(day);
                    }
                    else
                    {
                        this.MultiSelectOnClickNormal(day);
                    }
                }

                // Si on est sur le clic droit ou sur le clic gauche mais sans correspondance de selection, on arrete l'execution des évènement de gestion des clic
                if (e.ChangedButton == MouseButton.Left
                    || (e.ChangedButton == MouseButton.Right
                        && !this.SelectedDates.Any(d => d.Year == this.displayYear && d.Month == this.displayMonth && d.Day == day)))
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        ///     Levé lors du DragEnter.
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement.
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="System.Windows.DragEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnDayBoxPreviewDragEnter(object sender, DragEventArgs e)
        {
            if (sender is DayBoxControl && e.Data.GetFormats().Contains(typeof(InputDay).FullName))
            {
                ((DayBoxControl)sender).DayAppointmentsStack.Background = this.targetBackBrush;
                e.Handled = true;
            }
        }

        /// <summary>
        ///     Levé lors du DragLeave de l'évènement.
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement.
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="DragEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnDayBoxPreviewDragLeave(object sender, DragEventArgs e)
        {
            if (sender is DayBoxControl)
            {
                this.RestoreDayBoxBackground((DayBoxControl)sender);
            }
        }

        /// <summary>
        ///     Levé lors du click sur le bouton de passage au mois suivant.
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement.
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="RoutedEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnMonthGoNextMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            this.UpdateMonth(1);
        }

        /// <summary>
        ///     Levé lors du click sur le bouton de passage au mois précédent.
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement..
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="RoutedEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnMonthGoPrevMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            this.UpdateMonth(-1);
        }

        /// <summary>
        ///     Levé lors du DragDrop de l'évènement
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement.
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="DragEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnMonthViewGridPreviewDrop(object sender, DragEventArgs e)
        {
            var apt = (InputDay)e.Data.GetData(typeof(InputDay).FullName, false);
            if (apt != null)
            {
                Debug.Assert(apt.WorkStartTime != null, "apt.WorkStartTime != null");
                var dayBoxOld = (DayBoxControl)this.MonthViewGrid.FindName("DayBox" + apt.WorkStartTime.Value.Day);
                if (dayBoxOld != null)
                {
                    var dayBoxNew = ((Visual)e.OriginalSource).FindVisualAncestor<DayBoxControl>();
                    if (dayBoxNew != null)
                    {
                        var aptBox = (DayBoxAppointmentControl)dayBoxOld.FindName("Apt" + apt.ID);
                        if (aptBox != null)
                        {
                            dayBoxOld.DayAppointmentsStack.Children.Remove(aptBox);
                            dayBoxOld.UnregisterName("Apt" + apt.ID);

                            dayBoxNew.RegisterName("Apt" + apt.ID, aptBox);
                            this.RestoreDayBoxBackground(dayBoxNew);

                            var moveDays = (int)dayBoxNew.Tag - apt.WorkStartTime.Value.Day;
                            apt.WorkStartTime = apt.WorkStartTime.Value.AddDays(moveDays);
                            Debug.Assert(apt.WorkEndTime != null, "apt.WorkEndTime != null");
                            apt.WorkEndTime = apt.WorkEndTime.Value.AddDays(moveDays);

                            this.AppointmentMoved?.Invoke(
                                sender, 
                                new AppointmentMovedEvenArgs
                                    {
                                        AppointmentId = apt.ID, 
                                        NewDay = apt.WorkStartTime.Value.Day, 
                                        OldDay = (int)dayBoxOld.Tag
                                    });
                        }

                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        ///     Levé lors de la fin du chargement de la grille des mois.
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement..
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="RoutedEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnMonthViewLoaded(object sender, RoutedEventArgs e)
        {
            this.BuildCalendarUI();
        }

        /// <summary>
        ///     Levé lors de l'appui d'une touche de clavier.
        /// </summary>
        /// <param name="sender">
        ///     Objet ayant levé l'évènement..
        /// </param>
        /// <param name="e">
        ///     L'instance de <see cref="System.Windows.Input.KeyEventArgs" /> contenant les données de l'évènement.
        /// </param>
        private void OnMonthViewPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                case Key.Left:
                case Key.Up:
                case Key.Down:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        this.MultiSelectOnArrowKeyDownUsingCtrl(e.Key);
                    }
                    else if (Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        this.MultiSelectOnArrowKeyDownUsingShift(e.Key);
                    }
                    else
                    {
                        this.MultiSelectOnArrowKeyDownNormal(e.Key);
                    }

                    break;
                case Key.A:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        this.IsAllEventsSelected = true;
                    }
                    else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                    {
                        this.IsAllEventsSelected = false;
                    }

                    break;
            }
        }

        /// <summary>
        ///     Restaure le fond des emplacement d'évènement.
        /// </summary>
        /// <param name="dayBox">
        ///     L'emplacement a restaurer.
        /// </param>
        private void RestoreDayBoxBackground(DayBoxControl dayBox)
        {
            dayBox.DayAppointmentsStack.Background = (int)dayBox.Tag == DateTime.Today.Day ? this.todayBackBrush : this.dayBackBrush;
        }

        /// <summary>
        ///     Met à jours le mois.
        /// </summary>
        /// <param name="monthsToAdd">
        ///     Les mois à ajouter.
        /// </param>
        private void UpdateMonth(int monthsToAdd)
        {
            this.lastDayClicked = 0;
            this.SelectedDates.Clear();

            var ev = new MonthChangedEventArgs { OldDisplayStartDate = this.DisplayStartDate };
            this.DisplayStartDate = this.DisplayStartDate.AddMonths(monthsToAdd);
            ev.NewDisplayStartDate = this.DisplayStartDate;
            this.DisplayMonthChanged?.Invoke(this, ev);

            this.BuildCalendarUI();
        }

        #endregion
    }
}