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
   public class KernelTimePlanner : StandardKernel
   {
      #region Constructors and Destructors

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="KernelTimePlanner"/>.
      /// </summary>
      /// <param name="modules">
      /// Les modules.
      /// </param>
      public KernelTimePlanner(params INinjectModule[] modules)
         : base(modules)
      {
      }

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="KernelTimePlanner"/>.
      /// </summary>
      /// <param name="settings">
      /// Les paramètres.
      /// </param>
      /// <param name="modules">
      /// Les modules.
      /// </param>
      public KernelTimePlanner(INinjectSettings settings, params INinjectModule[] modules)
         : base(settings, modules)
      {
      }

      #endregion
   }
}