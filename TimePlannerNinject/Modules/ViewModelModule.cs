using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Modules
{
   using Ninject.Modules;

   using TimePlannerNinject.ViewModel;

   public class ViewModelModule: NinjectModule
   {
      public override void Load()
      {
         this.Bind<MainViewModel>().ToSelf().InTransientScope();
         this.Bind<CalendrierViewModel>().ToSelf().InTransientScope();
         this.Bind<DataGridInputDayViewModel>().ToSelf().InTransientScope();
         this.Bind<MenuPrincipalViewModel>().ToSelf().InTransientScope();
         this.Bind<StatusBarViewModel>().ToSelf().InTransientScope();
      }
   }
}
