// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject
{
    using System;
    using System.Windows;

    using GalaSoft.MvvmLight.Threading;

    using TimePlannerNinject.Kernel;
    using TimePlannerNinject.Modules;
    using TimePlannerNinject.View;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initialise les membres statiques de la classe <see cref="App" />.
        /// </summary>
        static App()
        {
            DispatcherHelper.Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Déclenche l'événement <see cref="E:System.Windows.Application.Startup" />.
        /// </summary>
        /// <param name="e"><see cref="T:System.Windows.StartupEventArgs" /> qui contient les données d'événement.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            KernelTimePlanner.Initialize(new ServiceModule(), new ViewModelModule());
            base.OnStartup(e);

            Current.MainWindow = KernelTimePlanner.Get<MainWindow>();
            Current.MainWindow.Show();
        }

        #endregion
    }
}