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
   using System.Linq;
   using System.Reflection;

   using GalaSoft.MvvmLight;

   using Ninject.Parameters;

   using TimePlannerNinject.Extensions;
   using TimePlannerNinject.Kernel;

   /// <summary>
   ///    Service d'affichage des fenêtre en MvvM pur.
   /// </summary>
   public class WindowService : IWindowService
   {
      #region Public Methods and Operators

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

      #endregion

      #region Methods

      /// <summary>
      /// Localise la <see cref="WindowView"/> pour un ViewModel donné
      /// </summary>
      /// <typeparam name="T">
      /// Le type de ViewModel
      /// </typeparam>
      /// <param name="viewName">
      /// Le nom de la vue
      /// </param>
      /// <param name="model">
      /// Le model
      /// </param>
      /// <returns>
      /// La fenêtre correspondante
      /// </returns>
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
            windowName = $"{viewModelName.Substring(0, viewModelName.Length - 9)}Window";
         }

         var windowType =
            Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => typeof(WindowView).IsAssignableFrom(t) && t.Name == windowName);
         if (windowType == null)
         {
            throw new ArgumentOutOfRangeException($"Unable to find Window for view model {typeof(T)}");
         }

         var modelArgument = new ConstructorArgument(nameof(model), model);

         var window = (WindowView)Assembly.GetExecutingAssembly().CreateInstance(windowType.FullName);
         if (window != null)
         {
            window.Initialize(KernelTimePlanner.Get<T>(modelArgument));

            return window;
         }

         return null;
      }

      #endregion
   }
}