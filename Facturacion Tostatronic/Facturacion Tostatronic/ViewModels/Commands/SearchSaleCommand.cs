using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands
{
    public class SearchSaleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public CreateInvoiceVM VM { get; set; }
        public SearchSaleCommand(CreateInvoiceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if((bool)parameter)
            {
                MessageBox.Show("Tiene Errores en la busqueda", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GetSalesData("sale_number", VM.InvoiceNumber);
        }
    }
}
