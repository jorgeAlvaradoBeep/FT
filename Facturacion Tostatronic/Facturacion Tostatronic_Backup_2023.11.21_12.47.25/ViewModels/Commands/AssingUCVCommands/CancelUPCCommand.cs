using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.AssingUCVCommands
{
    public class CancelUPCCommand : ICommand
    {
        private object Messagebox;

        public AssingUCVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public CancelUPCCommand(AssingUCVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Seguro que desea cancelar\nSi cancela, nada se guardara", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                VM.SelectedProduct = null;
                VM.ProductToModify = new Models.Products.ProductCodesEF();
            }

        }
    }
}
