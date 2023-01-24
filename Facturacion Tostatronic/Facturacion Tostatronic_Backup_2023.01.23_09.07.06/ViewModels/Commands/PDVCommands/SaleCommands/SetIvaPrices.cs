using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SetIvaPrices : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SaleVM VM { get; set; }
        public SetIvaPrices(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(!VM.IvaPricesSet)
            {
                VM.CompleteSale.SetIVAPrices();
                VM.IvaPricesSet = true;
            }
        }
    }
}
