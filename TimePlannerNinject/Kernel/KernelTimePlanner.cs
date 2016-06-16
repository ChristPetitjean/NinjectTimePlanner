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
      private static StandardKernel _kernel;

      public static T Get<T>()
      {
         return _kernel.Get<T>();
      }

      public static void Initialize(params INinjectModule[] modules)
      {
         if (_kernel == null)
         {
            _kernel = new StandardKernel(modules);
         }
      }
   }
}