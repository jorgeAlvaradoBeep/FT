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
                CFDIUse index = (CFDIUse)parameter;
                VM.CompleteSale.InvoiceData.UsoCFDI = index.CFDIUseP;
            }
            catch(Exception e) { }
            try
            {
                PaymentMethod index = (PaymentMethod)parameter;
                VM.CompleteSale.InvoiceData.MetodoDePago = index.PaymentMethodP;
            }
            catch (Exception e) { }
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
            try
            {
                RegimenFiscal index = (RegimenFiscal)parameter;
                
                VM.CompleteSale.InvoiceData.RegimenFiscal = index.RegimenFiscalP;
            }
            catch (Exception e) { }

        }
    }
}
