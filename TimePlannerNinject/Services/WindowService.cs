// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowService.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Provides window management services to WPF apps in a MVVM compliment way.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    using GalaSoft.MvvmLight;

    using Ninject.Parameters;

    using TimePlannerNinject.Extensions;
    using TimePlannerNinject.Kernel;

    /// <summary>
    ///     Service d'affichage des fenêtre en MvvM pur.
    /// </summary>
    public class WindowService : IWindowService
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public void OpenDialog<T>(string viewName, object model = null, Window owner = null) where T : ViewModelBase
        {
            WindowView windowView = this.FindWindow<T>(viewName, model);
            if (windowView.GetType() != Application.Current.MainWindow.GetType())
            {
                windowView.Owner = owner ?? Application.Current.MainWindow;
            }

            windowView.ShowDialog();
        }

        /// <inheritdoc />
        public void OpenDialog<T>(object model = null, Window owner = null) where T : ViewModelBase
        {
            WindowView windowView = this.FindWindow<T>(null, model);
            if (windowView.GetType() != Application.Current.MainWindow.GetType())
            {
                windowView.Owner = owner ?? Application.Current.MainWindow;
            }

            windowView.ShowDialog();
        }

        /// <inheritdoc />
        public void OpenWindow<T>(string viewName, object model = null, Window owner = null) where T : ViewModelBase
        {
            WindowView windowView = this.FindWindow<T>(viewName, model);
            if (windowView.GetType() != Application.Current.MainWindow.GetType())
            {
                windowView.Owner = owner ?? Application.Current.MainWindow;
            }

            windowView.Show();
        }

        /// <inheritdoc />
        public void OpenWindow<T>(object model = null, Window owner = null) where T : ViewModelBase
        {
            WindowView windowView = this.FindWindow<T>(null, model);
            if (windowView.GetType() != Application.Current.MainWindow.GetType())
            {
                windowView.Owner = owner ?? Application.Current.MainWindow;
            }

            windowView.Show();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Localise la <see cref="WindowView" /> pour un ViewModel donné
        /// </summary>
        /// <typeparam name="T">
        ///     Le type de ViewModel
        /// </typeparam>
        /// <param name="viewName">
        ///     Le nom de la vue
        /// </param>
        /// <param name="model">
        ///     Le model
        /// </param>
        /// <returns>
        ///     La fenêtre correspondante
        /// </returns>
        private WindowView FindWindow<T>(string viewName, object model) where T : ViewModelBase
        {
            var modelArgument = new ConstructorArgument(nameof(model), model);

            if (!string.IsNullOrEmpty(viewName))
            {
                var windowType =
                    Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => typeof(WindowView).IsAssignableFrom(t) && t.Name == viewName);
                if (windowType == null)
                {
                    throw new ArgumentOutOfRangeException($"Unable to find Window for view model {typeof(T)}");
                }

                var window = (WindowView)Assembly.GetExecutingAssembly().CreateInstance(windowType.FullName);
                if (window != null)
                {
                    window.Initialize(KernelTimePlanner.Get<T>(modelArgument));

                    return window;
                }
            }
            else
            {
                IEnumerable<Type> windowViewTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(WindowView).IsAssignableFrom(t));
                foreach (Type windowViewType in windowViewTypes)
                {
                    var window = (WindowView)Assembly.GetExecutingAssembly().CreateInstance(windowViewType.FullName);
                    if (window != null)
                    {
                        PropertyInfo propertyInfo = windowViewType.GetProperty("DataContext");

                        var value = propertyInfo.GetValue(window);
                        if (value.GetType() == typeof(T))
                        {
                            
                            window.Initialize(KernelTimePlanner.Get<T>(modelArgument));

                            return window;
                        }
                    }
                }
            }

            throw new ArgumentOutOfRangeException($"Unable to find Window for view model {typeof(T)}");
        }

        #endregion
    }
}