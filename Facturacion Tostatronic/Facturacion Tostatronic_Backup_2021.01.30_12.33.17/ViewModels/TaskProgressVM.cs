using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels
{
    public class TaskProgressVM
    {
        public int Progress { get; set; }
        public TaskProgressVM(ref int progress)
        {
            Progress = 50;
        }
    }
}
