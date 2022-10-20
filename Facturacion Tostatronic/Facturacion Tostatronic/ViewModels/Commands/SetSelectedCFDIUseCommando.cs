using Facturacion_Tostatronic.Models.CFDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands
{
    public class SetSelectedCFDIUseCommando : ICommand
    {
        public CreateInvoiceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SetSelectedCFDIUseCommando(CreateInvoiceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                PaymentForm index = (PaymentForm)parameter;
                string aux = "0";
                int aucN;
                string fp = index.PaymentFormP;
                int.TryParse(fp, out aucN);
                if (aucN < 10)
                    fp = aux + fp;
                VM.CompleteSale.InvoiceData.FormaDePago = fp;
            }
            catch (Exception e) { }

        }
    }
}
