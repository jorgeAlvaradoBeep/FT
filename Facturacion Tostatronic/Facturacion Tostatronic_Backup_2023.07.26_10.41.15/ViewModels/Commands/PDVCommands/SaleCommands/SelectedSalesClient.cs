using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SelectedSalesClient : ICommand
    {
        public SearchClientVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SelectedSalesClient(SearchClientVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Properties["SelectedClient"] = (ClientSale)parameter;
        }
    }
}
