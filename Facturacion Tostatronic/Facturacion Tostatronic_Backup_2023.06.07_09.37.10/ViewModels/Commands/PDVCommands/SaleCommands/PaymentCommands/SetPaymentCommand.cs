using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands.PaymentCommands
{
    public class SetPaymentCommand : ICommand
    {
        public PaymentVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SetPaymentCommand(PaymentVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(VM.Payment.Remaining>0 || VM.Payment.Payment==0)
            {
                DialogParameters parameters = new DialogParameters();
                parameters.Content = "La venta aun no se paga por completo, desea continuar?"+
                    Environment.NewLine+
                    "***La venta se marcara como pendiente";
                parameters.DefaultFocusedButton = ResponseButton.Accept;
                RadWindow.Confirm("La venta aun no se paga por completo, desea conyinuar?" +
                    Environment.NewLine +
                    "***La venta se marcara como pendiente", this.OnClosed);
            }
            else
            {
                VM.Payment.Pagado = true;
                Application.Current.Properties["PaymentResult"] = VM.Payment;
            }
        }

        private void OnClosed(object sender, WindowClosedEventArgs e)
        {
            var result = e.DialogResult;
            if (result != true)
                VM.Payment.Cancel = true;
            else
            {
                VM.Payment.Cancel = false;
                VM.Payment.Pagado = false;
            }
            Application.Current.Properties["PaymentResult"] = VM.Payment;
        }
    }
}
