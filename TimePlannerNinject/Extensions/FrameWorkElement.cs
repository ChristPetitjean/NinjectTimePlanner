using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Extensions
{
   using System.Windows;
   using System.Windows.Media;

   public static class FrameWorkElement
   {
      public static FrameworkElement FindVisualAncestor(this Visual visual, Type ancestorType)
      {
         while (visual != null && !ancestorType.IsInstanceOfType(visual))
         {
            visual = (Visual)VisualTreeHelper.GetParent(visual);
         }

         return visual as FrameworkElement;
      }
   }
}
