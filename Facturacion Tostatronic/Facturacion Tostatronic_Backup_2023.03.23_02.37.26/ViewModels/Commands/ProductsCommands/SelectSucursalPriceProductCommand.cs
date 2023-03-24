using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.Products.SucursalProduct;
using Facturacion_Tostatronic.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class SelectSucursalPriceProductCommand : ICommand
    {
        public SucursalPricesVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SelectSucursalPriceProductCommand(SucursalPricesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SucursalProduct p = new SucursalProduct((ProductComplete)parameter);
            if (p == null)
                return;
            VM.Product = p;

        }
    }
}
