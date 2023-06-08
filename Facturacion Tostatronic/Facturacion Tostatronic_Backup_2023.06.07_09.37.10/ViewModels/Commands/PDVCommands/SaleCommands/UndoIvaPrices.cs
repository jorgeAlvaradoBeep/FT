using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class UndoIvaPrices : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SaleVM VM { get; set; }
        public UndoIvaPrices(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (VM.IvaPricesSet)
            {
                VM.CompleteSale.UndoIVAPrices();
                VM.IvaPricesSet = false;
            }
        }
    }
}
