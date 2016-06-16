﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonthView.xaml.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.UserControl
{
   using System;
   using System.Collections.ObjectModel;
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
      ///    The _ display month.
      /// </summary>
      private int displayMonth;

      /// <summary>
      ///    The _ display start date.
      /// </summary>
      private DateTime displayStartDate;

      /// <summary>
      ///    The _ display year.
      /// </summary>
      private int displayYear;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      ///    Initialise une nouvelle instance de la classe <see cref="MonthView" />.
      /// </summary>
      public MonthView()
      {
         this.todayBackBrush = (Brush)this.TryFindResource("OrangeGradientBrush");
         this.dayBackBrush = Brushes.White;
         this.targetBackBrush = Brushes.LightSlateGray;

         this.displayStartDate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1));
         this.displayMonth = this.displayStartDate.Month;
         this.displayYear = this.displayStartDate.Year;
         var cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
         this.sysCal = cultureInfo.Calendar;
         this.Appointments = new ObservableCollection<InputDay>();

         this.Loaded += this.MonthViewLoaded;

         this.InitializeComponent();
      }

      #endregion

      #region Public Events

      /// <summary>
      ///    The appointment dbl clicked.
      /// </summary>
      public event EventHandler<int> AppointmentDblClicked;

      /// <summary>
      ///    The appointment moved.
      /// </summary>
      public event EventHandler<AppointmentMovedEvenArgs> AppointmentMoved;

      /// <summary>
      ///    The day box double clicked.
      /// </summary>
      public event EventHandler<NewAppointmentEventArgs> DayBoxDoubleClicked;

      /// <summary>
      ///    The display month changed.
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
            if (this.IsLoaded)
            {
               this.BuildCalendarUI(); 
            }
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
            this.displayMonth = this.displayStartDate.Month;
            this.displayYear = this.displayStartDate.Year;
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
         new UIPropertyMetadata(new ObservableCollection<InputDay>()));

      /// <summary>
      ///    Identifies the <see cref="DisplayStartDate" /> dependency property.
      /// </summary>
      public static readonly DependencyProperty DisplayStartDateProperty = DependencyProperty.Register(
         DisplayStartDatePropertyName, 
         typeof(DateTime), 
         typeof(MonthView), 
         new UIPropertyMetadata(DateTime.Today));

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
         var endOffsetDays = 7 - (int)Enum.ToObject(typeof(DayOfWeek), this.displayStartDate.AddDays(dayInMounth - 1).DayOfWeek) + 1;

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
                  this.AppointmentDblClicked(sender, (int)((DayBoxAppointmentControl)e.Source).Tag);
               }
            }

            e.Handled = true;
         }
      }

      /// <summary>
      ///    The build calendar ui.
      /// </summary>
      // ReSharper disable once InconsistentNaming
      private void BuildCalendarUI()
      {
         var daysInMonth = this.sysCal.GetDaysInMonth(this.displayYear, this.displayMonth);
         var offsetDays = (int)Enum.ToObject(typeof(DayOfWeek), this.displayStartDate.DayOfWeek);
         var weekCount = 0;
         var weekRowCtrl = new WeekOfDaysControls();

         // On efface tous les controles enfant et les enregistremetns
         this.MonthViewGrid.Children.Clear();
         NameScope.SetNameScope(this, new NameScope());

         this.AddRowsToMonthGrid(daysInMonth, offsetDays);
         this.MonthYearLabel.Content = this.displayStartDate.ToString("MMMM yyyy");

         for (var i = 1; i <= daysInMonth; i++)
         {
            if (i != 1 && Math.Abs(Math.IEEERemainder(i + offsetDays - 1, 7)) < 0.001)
            {
               Grid.SetRow(weekRowCtrl, weekCount);
               this.MonthViewGrid.Children.Add(weekRowCtrl);

               weekRowCtrl = new WeekOfDaysControls();
               weekCount += 1;
            }

            var dayBox = new DayBoxControl { Name = "DayBox" + i, DayNumberLabel = { Content = i.ToString() }, Tag = i };
            dayBox.MouseDoubleClick += this.DayBoxOnMouseDoubleClick;
            dayBox.PreviewDragEnter += this.DayBoxOnPreviewDragEnter;
            dayBox.PreviewDragLeave += this.DayBoxOnPreviewDragLeave;

            NameScope.SetNameScope(dayBox, new NameScope());
            this.RegisterName("DayBox" + i, dayBox);

            NameScope.SetNameScope(dayBox, new NameScope());
            this.RegisterName("DayBox" + i, dayBox);

            if (new DateTime(this.displayYear, this.displayMonth, i) == DateTime.Today)
            {
               dayBox.DayLabelRowBorder.Background = this.todayBackBrush;
               dayBox.DayAppointmentsStack.Background = Brushes.Wheat;
            }

            if (this.Appointments != null)
            {
               var day = i;
               var aptDay = this.Appointments.ToList().FindAll(d => d.WorkStartTime.HasValue && d.WorkStartTime.Value.Day == day);

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
         if (e.Source is DayBoxControl && ((Visual)e.OriginalSource).FindVisualAncestor(typeof(DayBoxAppointmentControl)) != null)
         {
            var ev = new NewAppointmentEventArgs();
            if (((DayBoxControl)e.Source).Tag != null)
            {
               ev.StratDate = new DateTime(this.displayYear, this.displayMonth, (int)((DayBoxControl)e.Source).Tag, 10, 0, 0);
               ev.EndDate = ((DateTime)ev.StratDate).AddHours(2);
            }

            if (this.DayBoxDoubleClicked != null)
            {
               this.DayBoxDoubleClicked(sender, ev);
            }

            e.Handled = true;
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
      /// The get dummy apt.
      /// </summary>
      /// <param name="day">
      /// The day.
      /// </param>
      /// <returns>
      /// The <see cref="Border"/>.
      /// </returns>
      private Border GetDummyApt(int day)
      {
         var brd = new Border
                      {
                         CornerRadius = new CornerRadius(5), 
                         BorderBrush = Brushes.DarkOliveGreen, 
                         Background = Brushes.LightGreen, 
                         Margin = new Thickness(2, 2, 2, 1), 
                         BorderThickness = new Thickness(1), 
                         Child = new TextBlock(new Run("Evènement le" + day)) { Padding = new Thickness(2), FontSize = 10 }
                      };

         return brd;
      }

      /// <summary>
      /// The month go next_ mouse left button up.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The e.
      /// </param>
      private void MonthGoNextMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         this.UpdateMonth(1);
      }

      /// <summary>
      /// The month go prev_ mouse left button up.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The e.
      /// </param>
      private void MonthGoPrevMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
               var dayBoxNew = (DayBoxControl)((Visual)e.OriginalSource).FindVisualAncestor(typeof(DayBoxControl));
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
         if (this.Appointments == null)
         {
            this.BuildCalendarUI();
         }
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
         var ev = new MonthChangedEventArgs { OldDisplayStartDate = this.displayStartDate };
         this.DisplayStartDate = this.DisplayStartDate.AddMonths(monthsToAdd);
         ev.NewDisplayStartDate = this.DisplayStartDate;
         if (this.DisplayMonthChanged != null)
         {
            this.DisplayMonthChanged(this, ev);
         }
      }

      #endregion

      /// <summary>
      ///    The appointment moved even args.
      /// </summary>
      public class AppointmentMovedEvenArgs : EventArgs
      {
         #region Public Properties

         /// <summary>
         ///    Obtient ou définit l'dentifiant du rendez-vous.
         /// </summary>
         public int AppointmentId { get; set; }

         /// <summary>
         ///    Obtient ou définit le numéro du nouveau jour.
         /// </summary>
         public int NewDay { get; set; }

         /// <summary>
         ///    Obtient ou définit le numéro de l'ancien jour.
         /// </summary>
         public int OldDay { get; set; }

         #endregion
      }
   }
}