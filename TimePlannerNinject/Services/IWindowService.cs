// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWindowService.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Interface du service permettant l'ouverture de fenêtre en MvvM pur.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Services
{
    using System.Windows;

    using GalaSoft.MvvmLight;

    /// <summary>
    ///     Interface du service permettant l'ouverture de fenêtre en MvvM pur.
    /// </summary>
    public interface IWindowService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Ouvre la fenêtre bloquante correspondant au ViewModel
        /// </summary>
        /// <typeparam name="T">
        ///     Type de ViewModel
        /// </typeparam>
        /// <param name="viewName">
        ///     Nom de la vue à ouvrir.
        /// </param>
        /// <param name="model">
        ///     Paramètre à passer au ViewModel
        ///     <remarks>
        ///         Le paramètre doit obligatoirement se nommer model pour correspondre
        ///     </remarks>
        /// </param>
        /// <param name="owner">
        ///  Le propriétaire de la fenêtre.
        /// </param>
        void OpenDialog<T>(string viewName, object model = null, Window owner = null) where T : ViewModelBase;

        /// <summary>
        ///     Ouvre la fenêtre bloquante correspondant au ViewModel
        /// </summary>
        /// <typeparam name="T">
        ///     Type de ViewModel
        /// </typeparam>
        /// <param name="model">
        ///     Paramètre à passer au ViewModel
        ///     <remarks>
        ///         Le paramètre doit obligatoirement se nommer model pour correspondre
        ///     </remarks>
        /// </param>
        /// <param name="owner">
        ///  Le propriétaire de la fenêtre.
        /// </param>
        void OpenDialog<T>(object model = null, Window owner = null) where T : ViewModelBase;

        /// <summary>
        ///     Ouvre la fenêtre correspondant au ViewModel
        /// </summary>
        /// <typeparam name="T">
        ///     Type de ViewModel
        /// </typeparam>
        /// <param name="viewName">
        ///     Nom de la vue à ouvrir.
        /// </param>
        /// <param name="model">
        ///     Paramètre à passer au ViewModel
        ///     <remarks>
        ///         Le paramètre doit obligatoirement se nommer model pour correspondre
        ///     </remarks>
        /// </param>
        /// <param name="owner">
        ///  Le propriétaire de la fenêtre.
        /// </param>
        void OpenWindow<T>(string viewName, object model = null, Window owner = null) where T : ViewModelBase;

        /// <summary>
        /// Ouvre la fenêtre correspondant au ViewModel
        /// </summary>
        /// <typeparam name="T">
        /// Type de ViewModel
        /// </typeparam>
        /// <param name="model">
        /// Paramètre à passer au ViewModel
        ///     <remarks>
        /// Le paramètre doit obligatoirement se nommer model pour correspondre
        ///     </remarks>
        /// </param>
        /// <param name="owner">
        ///  Le propriétaire de la fenêtre.
        /// </param>
        void OpenWindow<T>(object model = null, Window owner = null) where T : ViewModelBase;

        #endregion
    }
}