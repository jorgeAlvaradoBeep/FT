using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SeeInvoiceCommands
{
    public class SearchSaleSICommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeInvoiceVM VM { get; set; }
        public SearchSaleSICommand(SeeInvoiceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if ((bool)parameter)
            {
                MessageBox.Show("Tiene Errores en la busqueda", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GetSalesData("sale_number", VM.InvoiceNumber);
        }
    }
}
