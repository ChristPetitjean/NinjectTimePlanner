// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KernelTimePlanner.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Kernel
{
   using Ninject;
   using Ninject.Modules;

   /// <summary>
   ///    Kernel du time planner.
   /// </summary>
   public static class KernelTimePlanner
   {
      /// <summary>
      /// Le kernel
      /// </summary>
      private static StandardKernel kernel;

      /// <summary>
      /// Obtient l'instance du service spécifié.
      /// </summary>
      /// <typeparam name="T">Service a rétourner</typeparam>
      /// <returns>L'instance du service correspondant</returns>
      public static T Get<T>()
      {
         return kernel.Get<T>();
      }

      /// <summary>
      /// Initialize le kernel.
      /// </summary>
      /// <param name="modules">Les modules à charger.</param>
      public static void Initialize(params INinjectModule[] modules)
      {
         if (kernel == null)
         {
            kernel = new StandardKernel(modules);
         }
      }
   }
}