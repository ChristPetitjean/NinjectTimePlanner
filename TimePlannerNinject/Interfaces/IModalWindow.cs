using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Interfaces
{
    public interface IModalWindow
    {
        bool? DialogResult { get; set; }
        event EventHandler Closed;
        void Show();
        object DataContext { get; set; }
        void Close();

    }
}
