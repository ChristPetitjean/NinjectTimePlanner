// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelLocator.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.ViewModel
{
   using System.Windows;

   using TimePlannerNinject.Kernel;

   /// <summary>
   ///    The view model locator.
   /// </summary>
   public class ViewModelLocator
   {
      #region Public Properties

      /// <summary>
      ///    Obtient Le ViewModel Principal.
      /// </summary>
      public MainViewModel Main
      {
         get
         {
            return KernelTimePlanner.Get<MainViewModel>();
         }
      }

      /// <summary>
      ///    Obtient Le ViewModel de menu pricipal.
      /// </summary>
      public MenuPrincipalViewModel MenuPrincipal
      {
         get
         {
            return KernelTimePlanner.Get<MenuPrincipalViewModel>();
         }
      }

      /// <summary>
      ///    Obtient Le ViewModel de la barre de status.
      /// </summary>
      public StatusBarViewModel StatusBar
      {
         get
         {
            return KernelTimePlanner.Get<StatusBarViewModel>();
         }
      }

      /// <summary>
      ///    Obtient Le ViewModel de contenu des jour au format datagrid.
      /// </summary>
      public DataGridInputDayViewModel DataGridInputDay
      {
         get
         {
            return KernelTimePlanner.Get<DataGridInputDayViewModel>();
         }
      }

      /// <summary>
      ///    Obtient Le ViewModel de contenu des jour au format datagrid.
      /// </summary>
      public CalendrierViewModel Calendrier
      {
         get
         {
            return KernelTimePlanner.Get<CalendrierViewModel>();
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