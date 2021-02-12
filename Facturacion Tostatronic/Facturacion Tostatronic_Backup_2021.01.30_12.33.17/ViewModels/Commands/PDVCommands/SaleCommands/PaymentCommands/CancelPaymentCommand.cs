using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands.PaymentCommands
{
    public class CancelPaymentCommand : ICommand
    {
        public PaymentVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public CancelPaymentCommand(PaymentVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Properties["PaymentResult"] = null;
        }
    }
}
