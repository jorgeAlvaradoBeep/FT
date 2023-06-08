using Facturacion_Tostatronic.Views.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class CallSucursalPriceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public CallSucursalPriceCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SucursalPrice dP = new SucursalPrice();
            dP.ShowDialog();
        }
    }
}
