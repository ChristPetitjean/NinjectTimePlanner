// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelModule.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Module de chargement des ViewModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Modules
{
    using Ninject.Modules;

    using TimePlannerNinject.ViewModel;

    /// <summary>
    ///     Module de chargement des ViewModel.
    /// </summary>
    public class ViewModelModule : NinjectModule
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Méthode de chargement.
        /// </summary>
        public override void Load()
        {
            this.Bind<MainViewModel>().ToSelf().InSingletonScope();
            this.Bind<CalendrierViewModel>().ToSelf().InSingletonScope();
            this.Bind<MenuPrincipalViewModel>().ToSelf().InSingletonScope();
            this.Bind<StatusBarViewModel>().ToSelf().InSingletonScope();
            this.Bind<EditWorkPlacesViewModel>().ToSelf().InSingletonScope();
            this.Bind<InputDayViewModel>().ToSelf().InTransientScope();
            this.Bind<PreviewPrintViewModel>().ToSelf().InTransientScope();
        }

        #endregion
    }
}