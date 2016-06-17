using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Model
{
    using GalaSoft.MvvmLight.Views;

    using TimePlannerNinject.Interfaces;

    public class ModalDialogService : IModalDialogService
    {
        public void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel, Action<TDialogViewModel> onDialogClose)
        {
            view.DataContext = viewModel;
            if (onDialogClose != null)
            {
                view.Closed += (sender, e) => onDialogClose(viewModel);
            }

            view.Show();
        }

        public void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel)
        {
            this.ShowDialog(view, viewModel, null);
        }
    }
}
