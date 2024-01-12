using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class NeedFacturaCommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public NeedFacturaCommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.CompleteSale.NeedFactura = (bool)parameter;
            VM.CompleteSale.GetSubtotal();
        }
    }
}
