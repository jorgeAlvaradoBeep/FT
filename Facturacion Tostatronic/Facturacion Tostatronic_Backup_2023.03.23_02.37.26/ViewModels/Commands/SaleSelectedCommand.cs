using Facturacion_Tostatronic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands
{
    public class SaleSelectedCommand : ICommand
    {
        public CreateInvoiceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SaleSelectedCommand(CreateInvoiceVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Sale s = (Sale)parameter;
            VM.CompleteSale.Folio = s.idSale.ToString();
            VM.GetProductsFromSale("sale_number", s.idSale.ToString(), s.rfc);
        }
    }
}
