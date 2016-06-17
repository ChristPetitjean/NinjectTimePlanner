// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDialogService.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Defines the IDialogService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Interfaces
{
    using System;

    using TimePlannerNinject.Model;

    /// <summary>
    /// Service de gestion des fenêtres.
    /// </summary>
    public interface IModalDialogService
    {
        void ShowDialog<TViewModel>(IModalWindow view, TViewModel viewModel, Action<TViewModel> onDialogClose);

        void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel);
    }
}