namespace TimePlannerNinject.View
{
   using System.Windows;

   using TimePlannerNinject.Extensions;
   using TimePlannerNinject.Model;
   using TimePlannerNinject.ViewModel;

   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : WindowView
   {
      /// <summary>
      /// Initializes a new instance of the MainWindow class.
      /// </summary>
      public MainWindow()
      {
         InitializeComponent();
         this.Closing += (s, e) => ViewModelLocator.Cleanup();
      }
   }
}