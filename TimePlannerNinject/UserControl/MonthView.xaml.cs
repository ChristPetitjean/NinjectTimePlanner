// --------------------------------------------------------------------------------------------------------------------
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
   using System.ComponentModel;
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
   ///    Logique d'interaction pour MonthView.xaml
   /// </summary>
   public partial class MonthView
   {
      #region Constants

      /// <summary>
      ///    The <see cref="Appointments" /> dependency property's name.
      /// </summary>
      public const string AppointmentsPropertyName = "Appointments";

      /// <summary>
      ///    The <see cref="DisplayStartDate" /> dependency property's name.
      /// </summary>
      public const string DisplayStartDatePropertyName = "DisplayStartDate";

      #endregion

      #region Fields

      /// <summary>
      ///    The _ display month.
      /// </summary>
      private int displayMonth;

      /// <summary>
      ///    The _ display year.
      /// </summary>
      private int displayYear;

      /// <summary>
      ///    The _day back brush.
      /// </summary>
      private readonly Brush dayBackBrush;

      /// <summary>
      ///    The sys cal.
      /// </summary>
      private readonly Calendar sysCal;

      /// <summary>
      ///    The _target back brush.
      /// </summary>
      private readonly Brush targetBackBrush;

      /// <summary>
      ///    The _today back brush.
      /// </summary>
      private readonly Brush todayBackBrush;

      /// <summary>
      ///    The _today back brush.
      /// </summary>
      private readonly Brush todayStackBrush;

      /// <summary>
      /// The selected days border brush
      /// </summary>
      private readonly Brush selectedDaysBorderBrush;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      ///    Initialise une nouvelle instance de la classe <see cref="MonthView" />.
      /// </summary>
      public MonthView()
      {
         this.todayBackBrush = (Brush)this.TryFindResource("TodayBackBrush");
         this.dayBackBrush = (Brush)this.TryFindResource("DayBackBrush");
         this.targetBackBrush = (Brush)this.TryFindResource("TargetBackBrush");
         this.todayStackBrush = (Brush)this.TryFindResource("TodayStackBackBrush");
         this.selectedDaysBorderBrush = (Brush)this.TryFindResource("SelectedDayBorderBrush");

         this.displayMonth = this.DisplayStartDate.Month;
         this.displayYear = this.DisplayStartDate.Year;
         var cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
         this.sysCal = cultureInfo.Calendar;
         this.Appointments = new ObservableCollection<InputDay>();
         this.DisplayStartDate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1));

         this.Loaded += this.MonthViewLoaded;
         this.KeyDown += this.OnAppointmentKeyDown;
         this.Focus();

         this.InitializeComponent();
      }

      void OnAppointmentKeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
         {
            if (this.DayBoxDoubleClicked != null)
            {
               NewAppointmentEventArgs arg = new NewAppointmentEventArgs();
               arg.StartDate = this.SelectedDates.Any() ? this.SelectedDates.OrderBy(d => d).First() : (DateTime?)null;
               arg.EndDate = this.SelectedDates.Any() ? this.SelectedDates.OrderBy(d => d).Last() : (DateTime?)null;
               this.DayBoxDoubleClicked(sender, arg); 
            }
         }
      }

      #endregion

      #region Public Events

      /// <summary>
      ///    Levé lors du double click sur un évènement.
      /// </summary>
      public event EventHandler<AppointmentDblClickedEvenArgs> AppointmentDblClicked;

      /// <summary>
      ///    Levé lors du déplacement d'un évènement.
      /// </summary>
      public event EventHandler<AppointmentMovedEvenArgs> AppointmentMoved;

      /// <summary>
      ///    Levé lors du double click sur un emplacement vide.
      /// </summary>
      public event EventHandler<NewAppointmentEventArgs> DayBoxDoubleClicked;

      /// <summary>
      ///    Levé lors du changement de mois affiché.
      /// </summary>
      public event EventHandler<MonthChangedEventArgs> DisplayMonthChanged;

      #endregion

      #region Public Properties

      /// <summary>
      ///    Obtient ou définit la liste des <see cref="Appointments" />
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
      ///    Obtient ou définit la date de début
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

      #endregion

      /// <summary>
      ///    Identifies the <see cref="Appointments" /> dependency property.
      /// </summary>
      public static readonly DependencyProperty AppointmentsProperty = DependencyProperty.Register(
         AppointmentsPropertyName,
         typeof(ObservableCollection<InputDay>),
         typeof(MonthView),
         new PropertyMetadata(OnAppointmentsChanged));

      private static void OnAppointmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var action = new NotifyCollectionChangedEventHandler(
            (o, args) =>
               {
                  MonthView me = d as MonthView;
                  if (me != null && me.IsLoaded)
                  {
                     me.BuildCalendarUI();
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

         MonthView mv = d as MonthView;
         if (mv != null && mv.IsLoaded)
         {
            mv.BuildCalendarUI();
         }
      }

      /// <summary>
      ///    Identifies the <see cref="DisplayStartDate" /> dependency property.
      /// </summary>
      public static readonly DependencyProperty DisplayStartDateProperty = DependencyProperty.Register(
         DisplayStartDatePropertyName,
         typeof(DateTime),
         typeof(MonthView),
         new PropertyMetadata(OnDisplayStartDAteChanged));

      private static void OnDisplayStartDAteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         MonthView me = d as MonthView;
         me.displayMonth = ((DateTime)e.NewValue).Month;
         me.displayYear = ((DateTime)e.NewValue).Year;
      }

      #region Methods

      /// <summary>
      /// The add rows to month grid.
      /// </summary>
      /// <param name="dayInMounth">
      /// The day in mounth.
      /// </param>
      /// <param name="offsetDays">
      /// The offset days.
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
      /// The apt on mouse double click.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The e.
      /// </param>
      private void AptOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
      {
         if (e.Source is DayBoxAppointmentControl)
         {
            if (((DayBoxAppointmentControl)e.Source).Tag != null)
            {
               if (this.AppointmentDblClicked != null)
               {
                  var idApt = (int)((DayBoxAppointmentControl)e.Source).Tag;
                  var arg = new AppointmentDblClickedEvenArgs { Id = idApt };
                  this.AppointmentDblClicked(e.Source, arg);
               }
            }
         }

         e.Handled = true;
      }

      /// <summary>
      ///    The build calendar ui.
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
         this.MonthYearLabel.Content = this.DisplayStartDate.ToString("MMMM yyyy");

         for (var i = 1; i <= daysInMonth; i++)
         {
            if (i != 1 && Math.Abs(Math.IEEERemainder(i + offsetDays - 1, 7)) < 0.001)
            {
               Grid.SetRow(weekRowCtrl, weekCount);
               this.MonthViewGrid.Children.Add(weekRowCtrl);

               weekRowCtrl = new WeekOfDaysControls();
               weekCount += 1;
            }

            var contextextMenuItem = new MenuItem() { Header = "Ajouter / Modifier la sélection", };

            var dayBox = new DayBoxControl { Name = "DayBox" + i, DayNumberLabel = { Content = i.ToString() }, Tag = i };
            dayBox.MouseDoubleClick += this.DayBoxOnMouseDoubleClick;
            dayBox.PreviewDragEnter += this.DayBoxOnPreviewDragEnter;
            dayBox.PreviewDragLeave += this.DayBoxOnPreviewDragLeave;
            dayBox.MouseUp += this.DayBoxOnMouseUp;

            contextextMenuItem.Click += (sender, e) =>
            {
                this.DayBoxOnMouseDoubleClick(dayBox, new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left));
            };

                dayBox.ContextMenu = new ContextMenu { Items = { contextextMenuItem } };

            if (this.SelectedDates.Any(s => s.Date.Day == i && s.Date.Month == this.displayMonth && s.Date.Year == this.displayYear))
            {
               dayBox.MainBorder.BorderBrush = this.selectedDaysBorderBrush;
               dayBox.MainBorder.BorderThickness = new Thickness(2);
            }

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
                  apt.MouseDoubleClick += this.AptOnMouseDoubleClick;
                  dayBox.DayAppointmentsStack.Children.Add(apt);
                  dayBox.RegisterName("Apt" + a.ID, apt);
               }
            }

            Grid.SetColumn(dayBox, (i - (weekCount * 7)) + offsetDays);
            weekRowCtrl.WeekRowGrid.Children.Add(dayBox);
         }

         Grid.SetRow(weekRowCtrl, weekCount);
         this.MonthViewGrid.Children.Add(weekRowCtrl);
      }

       private int lastDayClicked;
      private void DayBoxOnMouseUp(object sender, MouseButtonEventArgs e)
      {
         if (e.Source is DayBoxControl && e.ChangedButton == MouseButton.Left)
         {
            int day = (int)((DayBoxControl)e.Source).Tag;
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
               this.SelectedDates.Clear();
               if (this.lastDayClicked != 0)
               {
                  if (this.lastDayClicked < day)
                  {
                     for (int i = this.lastDayClicked; i <= day; i++)
                     {
                        this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, i));
                     } 
                  }
                  else if (this.lastDayClicked > day)
                  {
                     for (int i = day; i <= this.lastDayClicked; i++)
                     {
                        this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, i));
                     } 
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
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control)
            {
               if (!this.SelectedDates.Any(s => s.Date.Day == day && s.Date.Month == this.displayMonth && s.Date.Year == this.displayYear))
               {
                  this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
               }
               else
               {
                  DateTime dateTime = this.SelectedDates.First(d => d.Year == this.displayYear && d.Month == this.displayMonth && d.Day == day);
                  this.SelectedDates.Remove(dateTime);
               }
               this.lastDayClicked = day;
            }
            else
            {
               this.SelectedDates.Clear();
               this.SelectedDates.Add(new DateTime(this.displayYear, this.displayMonth, day));
               this.lastDayClicked = day;
            }

            e.Handled = true;
         }
      }

      public static readonly DependencyProperty SelectedDatesProperty = DependencyProperty.Register(
         "SelectedDates",
         typeof(ObservableCollection<DateTime>),
         typeof(MonthView),
         new PropertyMetadata(OnSelectedIdsChanged));

      private static void OnSelectedIdsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var action = new NotifyCollectionChangedEventHandler(
           (o, args) =>
           {
              MonthView me = d as MonthView;
              if (me != null && me.IsLoaded)
              {
                 me.BuildCalendarUI();
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

         MonthView mv = d as MonthView;
         if (mv != null && mv.IsLoaded)
         {
            mv.BuildCalendarUI();
         }
      }

      public ObservableCollection<DateTime> SelectedDates
      {
         get
         {
            return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty);
         }
         set
         {
            SetValue(SelectedDatesProperty, value);
         }
      }


      /// <summary>
      /// The day box on mouse double click.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The e.
      /// </param>
      private void DayBoxOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
      {
         if (e.Source is DayBoxControl && ((Visual)e.OriginalSource).FindVisualAncestor<DayBoxAppointmentControl>() == null && e.LeftButton == MouseButtonState.Pressed)
         {
            var ev = new NewAppointmentEventArgs();
            if (((DayBoxControl)e.Source).Tag != null)
            {
               ev.StartDate = new DateTime(this.displayYear, this.displayMonth, (int)((DayBoxControl)e.Source).Tag);
               ev.EndDate = ((DateTime)ev.StartDate);
            }

            if (this.DayBoxDoubleClicked != null)
            {
               this.DayBoxDoubleClicked(sender, ev);
            }

            e.Handled = true;
         }
         else if (sender is DayBoxControl)
         {
             DayBoxControl dayBoxControl = (DayBoxControl)sender;
             var ev = new NewAppointmentEventArgs();
             if (dayBoxControl.Tag != null)
             {
                 ev.StartDate = new DateTime(this.displayYear, this.displayMonth, (int)dayBoxControl.Tag);
                 ev.EndDate = ((DateTime)ev.StartDate);
             }

             if (this.DayBoxDoubleClicked != null)
             {
                 this.DayBoxDoubleClicked(sender, ev);
             }
         }
      }

      /// <summary>
      /// The day box on preview drag enter.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The e.
      /// </param>
      private void DayBoxOnPreviewDragEnter(object sender, DragEventArgs e)
      {
         if (sender is DayBoxControl && e.Data.GetFormats().Contains(typeof(InputDay).FullName))
         {
            ((DayBoxControl)sender).DayAppointmentsStack.Background = this.targetBackBrush;
            e.Handled = true;
         }
      }

      /// <summary>
      /// The day box on preview drag leave.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The e.
      /// </param>
      private void DayBoxOnPreviewDragLeave(object sender, DragEventArgs e)
      {
         if (sender is DayBoxControl)
         {
            this.RestoreDayBoxBackground((DayBoxControl)sender);
         }
      }

      /// <summary>
      /// The month go next_ mouse left button up.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="routedEventArgs">
      /// The routed Event Args.
      /// </param>
      private void MonthGoNextMouseLeftButtonUp(object sender, RoutedEventArgs routedEventArgs)
      {
         this.UpdateMonth(1);
      }

      /// <summary>
      /// The month go prev_ mouse left button up.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="routedEventArgs">
      /// The routed Event Args.
      /// </param>
      private void MonthGoPrevMouseLeftButtonUp(object sender, RoutedEventArgs routedEventArgs)
      {
         this.UpdateMonth(-1);
      }

      /// <summary>
      /// The month view grid_ preview drop.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The e.
      /// </param>
      private void MonthViewGridPreviewDrop(object sender, DragEventArgs e)
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

                     var moveDays = ((int)dayBoxNew.Tag) - apt.WorkStartTime.Value.Day;
                     apt.WorkStartTime = apt.WorkStartTime.Value.AddDays(moveDays);
                     Debug.Assert(apt.WorkEndTime != null, "apt.WorkEndTime != null");
                     apt.WorkEndTime = apt.WorkEndTime.Value.AddDays(moveDays);

                     if (this.AppointmentMoved != null)
                     {
                        this.AppointmentMoved(
                           sender,
                           new AppointmentMovedEvenArgs { AppointmentId = apt.ID, NewDay = apt.WorkStartTime.Value.Day, OldDay = (int)dayBoxOld.Tag });
                     }
                  }

                  e.Handled = true;
               }
            }
         }
      }

      /// <summary>
      /// The month view loaded.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="routedEventArgs">
      /// The routed event args.
      /// </param>
      private void MonthViewLoaded(object sender, RoutedEventArgs routedEventArgs)
      {
         this.BuildCalendarUI();
      }

      /// <summary>
      /// The restore day box background.
      /// </summary>
      /// <param name="dayBox">
      /// The day box.
      /// </param>
      private void RestoreDayBoxBackground(DayBoxControl dayBox)
      {
         dayBox.DayAppointmentsStack.Background = (int)dayBox.Tag == DateTime.Today.Day ? this.todayBackBrush : this.dayBackBrush;
      }

      /// <summary>
      /// The update month.
      /// </summary>
      /// <param name="monthsToAdd">
      /// The months to add.
      /// </param>
      private void UpdateMonth(int monthsToAdd)
      {
         this.lastDayClicked = 0;
         this.SelectedDates.Clear();

         var ev = new MonthChangedEventArgs { OldDisplayStartDate = this.DisplayStartDate };
         this.DisplayStartDate = this.DisplayStartDate.AddMonths(monthsToAdd);
         ev.NewDisplayStartDate = this.DisplayStartDate;
         if (this.DisplayMonthChanged != null)
         {
            this.DisplayMonthChanged(this, ev);
         }

         this.BuildCalendarUI();
      }

      #endregion
   }
}