using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.MenuCommands
{
    public class MenuToPageOneCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public MenuToPageOneCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.MenuPageOne = System.Windows.Visibility.Visible;
            VM.MenuPageTwo = System.Windows.Visibility.Hidden;
        }
    }
}
