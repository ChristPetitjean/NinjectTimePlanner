namespace TimePlannerNinject.Modules
{
   using Ninject.Modules;

   using TimePlannerNinject.Design;
   using TimePlannerNinject.Interfaces;

   /// <summary>
   ///    Module utiliser pour le design uniquement.
   /// </summary>
   public class DesignTimeModule : NinjectModule
   {
      #region Public Methods and Operators

      /// <summary>
      ///    Chargement du module.
      /// </summary>
      public override void Load()
      {
         this.Bind<ATimePlannerDataService>().To<TimePlannerDataServiceDesign>().InSingletonScope();
      }

      #endregion
   }
}