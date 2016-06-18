using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Model
{
    using System.Windows;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// Base class for a window displayed in the bugView application
    /// </summary>
    public abstract class WindowView : Window
    {
        /// <summary>
        /// Initializes the data context of the window.
        /// </summary>
        /// <param name="viewModel">
        /// The view model
        /// </param>
        public void Initialize(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            if (this != Application.Current.MainWindow)
            {
                this.Owner = Application.Current.MainWindow;
            }
            
        }

        /// <summary>
        /// Cleans up the bound view model when the window is closed. 
        /// Override Cleanup() in the view model if you need to dispose custom objects.
        /// </summary>
        /// <param name="e">
        /// The closed event args.
        /// </param>
        protected override void OnClosed(EventArgs e)
        {
            var viewModel = this.DataContext as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.Cleanup();
            }

            base.OnClosed(e);
        }
    }
}
