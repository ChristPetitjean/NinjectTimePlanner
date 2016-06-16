// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuPrincipalViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.ViewModel
{
   using GalaSoft.MvvmLight;

   using TimePlannerNinject.Interfaces;

   /// <summary>
   /// The menu view model.
   /// </summary>
   public class MenuPrincipalViewModel : ViewModelBase
   {
      #region Fields

      /// <summary>
      /// Service de données.
      /// </summary>
      private readonly ATimePlannerDataService service;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="MenuPrincipalViewModel"/>.
      /// </summary>
      /// <param name="service">
      /// Service de données.
      /// </param>
      public MenuPrincipalViewModel(ATimePlannerDataService service)
      {
         this.service = service; 
      }

      #endregion
   }
}