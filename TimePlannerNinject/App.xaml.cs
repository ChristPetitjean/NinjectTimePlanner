﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject
{
   using System.Windows;

   using GalaSoft.MvvmLight.Threading;

   using TimePlannerNinject.Kernel;
   using TimePlannerNinject.Modules;

   /// <summary>
   ///    Interaction logic for App.xaml
   /// </summary>
   public partial class App
   {
      #region Constructors and Destructors

      /// <summary>
      /// Initialise les membres statiques de la classe <see cref="App"/>.
      /// </summary>
      static App()
      {
         DispatcherHelper.Initialize();
      }

      #endregion

      /// <summary>
      /// Déclenche l'événement <see cref="E:System.Windows.Application.Startup" />.
      /// </summary>
      /// <param name="e"><see cref="T:System.Windows.StartupEventArgs" /> qui contient les données d'événement.</param>
      protected override void OnStartup(StartupEventArgs e)
      {
         if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
         {
            KernelTimePlanner.Initialize(new DesignTimeModule());
         }
         else
         {
             KernelTimePlanner.Initialize(new RunTimeModule());
         }

         base.OnStartup(e);
      }
   }
}