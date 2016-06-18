// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceModule.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Module utiliser au Runtime.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Modules
{
    using System.Windows;

    using Ninject.Modules;

   using TimePlannerNinject.Interfaces;
   using TimePlannerNinject.Model;
    using TimePlannerNinject.Services;
    using TimePlannerNinject.View;

    /// <summary>
   /// Module utiliser au Runtime.
   /// </summary>
   public class ServiceModule : NinjectModule
   {
      #region Public Methods and Operators

       /// <summary>
       /// Chargement du module.
       /// </summary>
       public override void Load()
       {
           this.Bind<ATimePlannerDataService>().To<TimePlannerDataService>().InSingletonScope();
           this.Bind<IWindowService>().To<WindowService>();
           this.Bind<IMessageboxService>().To<MessageboxService>();
       }

       #endregion
   }
}