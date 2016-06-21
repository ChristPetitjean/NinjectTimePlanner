using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimePlannerNinject.UserControl
{
   /// <summary>
   /// Logique d'interaction pour DayBoxControl.xaml
   /// </summary>
   public partial class DayBoxControl
   {
      public DayBoxControl()
      {
         this.InitializeComponent();
      }

      /// <summary>
      /// Lève l'évènement de click.
      /// </summary>
      public void RaiseEventClick()
      {
         this.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) { RoutedEvent = Mouse.MouseUpEvent, Source = this });
      }

      /// <summary>
      /// Lève l'évènement de double Click.
      /// </summary>
      public void RaiseDoubleClickEvent()
      {
         this.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) { RoutedEvent = MouseDoubleClickEvent, Source = this });
      }
   }
}
