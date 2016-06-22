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
    using Ninject.Modules;

    using TimePlannerNinject.Services;

    /// <summary>
    ///     Module utiliser pour l'enregistrement des services ninject.
    /// </summary>
    public class ServiceModule : NinjectModule
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Chargement du module.
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