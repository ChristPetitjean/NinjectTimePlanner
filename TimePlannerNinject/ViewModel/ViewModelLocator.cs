namespace TimePlannerNinject.ViewModel
{
   using System.Diagnostics.CodeAnalysis;

   using GalaSoft.MvvmLight;

   using Ninject;

   using TimePlannerNinject.Kernel;
   using TimePlannerNinject.Modules;

   /// <summary>
   ///    The view model locator.
   /// </summary>
   public class ViewModelLocator
   {
      #region Static Fields

      /// <summary>
      ///    The kernel.
      /// </summary>
      private static KernelTimePlanner kernel;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      ///    Initialise une nouvelle instance de la classe <see cref="ViewModelLocator" />.
      /// </summary>
      public ViewModelLocator()
      {
         // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
         if (ViewModelBase.IsInDesignModeStatic)
         {
            kernel = new KernelTimePlanner(new DesignTimeModule());
         }
         else
         {
            //kernel = new KernelTimePlanner(new RunTimeModule());
            kernel = new KernelTimePlanner(new DesignTimeModule());
         }
      }

      #endregion

      #region Public Properties

      /// <summary>
      ///    Obtient Le ViewModel Principal.
      /// </summary>
      public MainViewModel Main
      {
         get
         {
            return kernel.Get<MainViewModel>();
         }
      }

      /// <summary>
      ///    Obtient Le ViewModel de menu pricipal.
      /// </summary>
      public MenuPrincipalViewModel MenuPrincipal
      {
         get
         {
            return kernel.Get<MenuPrincipalViewModel>();
         }
      }

      /// <summary>
      ///    Obtient Le ViewModel de la barre de status.
      /// </summary>
      public StatusBarViewModel StatusBar
      {
         get
         {
            return kernel.Get<StatusBarViewModel>();
         }
      }

      /// <summary>
      ///    Obtient Le ViewModel de contenu des jour au format datagrid.
      /// </summary>
      public DataGridInputDayViewModel DataGridInputDay
      {
         get
         {
            return kernel.Get<DataGridInputDayViewModel>();
         }
      }

      /// <summary>
      ///    Obtient Le ViewModel de contenu des jour au format datagrid.
      /// </summary>
      public CalendrierViewModel Calendrier
      {
         get
         {
            return kernel.Get<CalendrierViewModel>();
         }
      }

      #endregion

      #region Public Methods and Operators

      /// <summary>
      ///    Netoie les resources.
      /// </summary>
      public static void Cleanup()
      {
      }

      #endregion
   }
}