// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowView.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Classe de base permettant l'affichage d'une fenêtre en MvvM
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Extensions
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using GalaSoft.MvvmLight;

    /// <summary>
    ///     Classe de base permettant l'affichage d'une fenêtre en MvvM
    /// </summary>
    public abstract class WindowView : Window
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initialise le contexte de données de la vue
        /// </summary>
        /// <param name="viewModel">
        ///     Le VM correspondant à la vue
        /// </param>
        public void Initialize(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Déclenche l'événement <see cref="E:System.Windows.Window.Closed" />.
        /// </summary>
        /// <param name="e">
        ///     <see cref="T:System.EventArgs" /> qui contient les données de l'événement.
        /// </param>
        protected override void OnClosed(EventArgs e)
        {
            var viewModel = this.DataContext as ViewModelBase;
            viewModel?.Cleanup();

            base.OnClosed(e);
        }

        #endregion
    }
}