using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands
{
    public class InvoiceConfigCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MenuVM VM { get; set; }
        public InvoiceConfigCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.SetInvoiceConfiguration();
        }
    }
}
