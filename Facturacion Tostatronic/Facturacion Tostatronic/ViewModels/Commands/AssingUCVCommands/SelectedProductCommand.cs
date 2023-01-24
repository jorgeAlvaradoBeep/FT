using Facturacion_Tostatronic.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.AssingUCVCommands
{
    public class SelectedProductCommand : ICommand
    {
        public AssingUCVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SelectedProductCommand(AssingUCVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            //VM.SelectedProduct = (ProductCodes)parameter;
        }
    }
}
