// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelModule.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Module de liaison des viewmodels.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Modules
{
   using Ninject.Modules;

   using TimePlannerNinject.ViewModel;

    /// <summary>
    /// Module de liaison des viewmodels.
    /// </summary>
    public class ViewModelModule: NinjectModule
   {
       /// <summary>
       /// Méthode de chargement.
       /// </summary>
       public override void Load()
       {
           this.Bind<MainViewModel>().ToSelf().InSingletonScope();
           this.Bind<CalendrierViewModel>().ToSelf().InTransientScope();
           this.Bind<MenuPrincipalViewModel>().ToSelf().InSingletonScope();
           this.Bind<StatusBarViewModel>().ToSelf().InSingletonScope();
           this.Bind<EditWorkPlacesViewModel>().ToSelf().InTransientScope();
           this.Bind<InputDayViewModel>().ToSelf().InTransientScope();
       }
   }
}
