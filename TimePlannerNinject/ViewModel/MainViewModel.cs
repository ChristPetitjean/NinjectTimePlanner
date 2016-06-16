// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.ViewModel
{
   using GalaSoft.MvvmLight;

   using TimePlannerNinject.Interfaces;

   /// <summary>
   ///    ViewModel principal
   /// </summary>
   public class MainViewModel : ViewModelBase
   {
      #region Fields

      /// <summary>
      ///    Service de données du planner
      /// </summary>
      private readonly ATimePlannerDataService service;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="MainViewModel"/>.
      /// </summary>
      /// <param name="service">
      /// Le service de données.
      /// </param>
      public MainViewModel(ATimePlannerDataService service)
      {
         this.service = service;
      }

      #endregion
   }
}