namespace TimePlannerNinject.View
{
   using System.Windows;

   using TimePlannerNinject.ViewModel;

   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
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