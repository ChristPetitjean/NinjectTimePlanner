namespace TimePlannerNinject.Modules
{
   using Ninject.Modules;

   using TimePlannerNinject.Interfaces;
   using TimePlannerNinject.Model;

   /// <summary>
   /// Module utiliser au Runtime.
   /// </summary>
   public class RunTimeModule : NinjectModule
   {
      #region Public Methods and Operators

      /// <summary>
      /// Chargement du module.
      /// </summary>
      public override void Load()
      {
         this.Bind<ATimePlannerDataService>().To<TimePlannerDataService>().InSingletonScope();
      }

      #endregion
   }
}