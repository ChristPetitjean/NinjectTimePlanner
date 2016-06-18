﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelLocator.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.ViewModel
{
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
      public CalendrierViewModel Calendrier
       {
           get
           {
               return KernelTimePlanner.Get<CalendrierViewModel>();
           }
       }

       /// <summary>
      ///    Obtient Le ViewModel de modification des lieux de travail.
      /// </summary>
      public EditWorkPlacesViewModel EditWorkPlaces
       {
           get
           {
               return KernelTimePlanner.Get<EditWorkPlacesViewModel>();
           }
       }

        /// <summary>
        ///    Obtient Le ViewModel de modification des lieux de travail.
        /// </summary>
        public InputDayViewModel InputDay
        {
            get
            {
                return KernelTimePlanner.Get<InputDayViewModel>();
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