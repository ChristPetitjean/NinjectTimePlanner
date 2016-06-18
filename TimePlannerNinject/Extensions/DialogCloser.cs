using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Extensions
{
    using System.Windows;

    using TimePlannerNinject.Model;

    public class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as WindowView;
            if (window != null)
            {
                bool? result = e.NewValue as bool?;
                if (result.HasValue)
                {
                    window.DialogResult = result;
                    window.Close();
                }
            }
        }

        public static void SetDialogResult(Window element, Boolean value)
        {
            element.SetValue(DialogResultProperty, value);
        }
        public static bool? GetDialogResult(Window element)
        {
            return (bool?)element.GetValue(DialogResultProperty);
        }
    }
}
