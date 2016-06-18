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
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using GalaSoft.MvvmLight;

    using Ninject.Parameters;

    using TimePlannerNinject.Extensions;
    using TimePlannerNinject.Kernel;
    using TimePlannerNinject.Model;

    /// <summary>
    ///     Provides window management services to WPF apps in a MVVM compliment way.
    /// </summary>
    public class WindowService : IWindowService
    {
        /// <inheritdoc />
        public void OpenDialog<T>(string viewName, object model = null) where T : ViewModelBase
        {
            this.FindWindow<T>(viewName, model).ShowDialog();
        }

        /// <inheritdoc />
        public void OpenDialog<T>(object model = null) where T : ViewModelBase
        {
            this.FindWindow<T>(null, model).ShowDialog();
        }

        /// <inheritdoc />
        public void OpenWindow<T>(string viewName, object model = null) where T : ViewModelBase
        {
            this.FindWindow<T>(viewName, model).Show();
        }

        /// <inheritdoc />
        public void OpenWindow<T>(object model = null) where T : ViewModelBase
        {
            this.FindWindow<T>(null, model).Show();
        }

        /// <summary>
        ///     Locates the <see cref="WindowView" /> for a given view model type
        /// </summary>
        /// <typeparam name="T">
        ///     The type of view model
        /// </typeparam>
        /// <param name="viewName">
        ///     The view name
        /// </param>
        /// <param name="model">
        ///     The model
        /// </param>
        /// <returns>The window</returns>
        private WindowView FindWindow<T>(string viewName, object model) where T : ViewModelBase
        {
            string windowName;
            var viewModelName = typeof(T).Name;
            if (!string.IsNullOrEmpty(viewName))
            {
                windowName = viewName;
            }
            else
            {
                windowName = viewModelName.Substring(0, viewModelName.Length - 9) + "Window";
            }

            var windowType =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(t => typeof(WindowView).IsAssignableFrom(t) && t.Name == windowName);
            if (windowType == null)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Unable to find Window for view model {0}", typeof(T)));
            }

            // Inject the model into the ViewModel's constructor
            var modelArgument = new ConstructorArgument("model", model);

            var window = (WindowView)Assembly.GetExecutingAssembly().CreateInstance(windowType.FullName);
            if (window != null)
            {
                window.Initialize(KernelTimePlanner.Get<T>(modelArgument));

                return window;
            }

            return null;
        }
    }
}